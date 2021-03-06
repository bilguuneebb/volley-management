﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.IdentityAndAccess;
using VolleyM.Domain.IdentityAndAccess.Handlers;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;

namespace VolleyM.Domain.Framework.Authorization
{
	public class DefaultAuthorizationHandler : IAuthorizationHandler
	{
		private static readonly UserId _predefinedAnonymousUserId = new UserId("anonym@volleym.idp");
		private static readonly UserId _authZUserId = new UserId("authz.user@volleym.idp");

		private static readonly RoleId _visitorRole = new RoleId("visitor");
		private static readonly RoleId _sysAdminRole = new RoleId("sysadmin");

		private readonly IRequestHandler<GetUser.Request, User> _getUserHandler;
		private readonly IRequestHandler<CreateUser.Request, User> _createUserHandler;
		private readonly ICurrentUserManager _currentUserManager;
		private readonly IApplicationInfo _applicationInfo;
		private readonly ApplicationTrustOptions _trustOptions;

		private readonly List<string> _idClaimTypes = new List<string>
		{
			"sub",
			"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
		};

		private const string AUTHORIZED_PARTY_CLAIM = "azp";
		private const string GRANT_TYPE_CLAIM = "gty";


		public DefaultAuthorizationHandler(
			IRequestHandler<CreateUser.Request, User> createUserHandler,
			ICurrentUserManager currentUserManager,
			IRequestHandler<GetUser.Request, User> getUserHandler,
			IApplicationInfo applicationInfo,
			ApplicationTrustOptions trustOptions)
		{
			_createUserHandler = createUserHandler;
			_currentUserManager = currentUserManager;
			_getUserHandler = getUserHandler;
			_applicationInfo = applicationInfo;
			_trustOptions = trustOptions;
		}

		public EitherAsync<Error, Unit> AuthorizeUser(ClaimsPrincipal user)
		{
			var idValueOption = GetUserIdFromClaims(user);

			var getUser =
				from anonUser in CheckUnauthenticatedUser(user)
				from id in idValueOption
					.ToEither(Error.NotAuthorized("UserId claim is missing")).ToAsync()
				from isSystem in IsNotPredefinedSystemId(id)
					.ToEither(Error.NotAuthorized("User Id is invalid")).ToAsync()
				from user1 in GetUser(id)
				select user1;

			var createUserMap = getUser.MapLeft(e => e switch
				{
					{ Type: ErrorType.NotAuthenticated }
						=> GetAnonymousUser(),
					{ Type: ErrorType.NotFound }
						=> CreateUser(idValueOption.ValueUnsafe(), user),
					var left => left
				}).MatchAsync<Either<Error, User>>(
					Right: u => u,
					LeftAsync: async left => await left.ToEither())
				.ToAsync();

			return createUserMap
				.Do(SetCurrentContext)
				.Map(_ => Unit.Default);
		}

		private static EitherAsync<Error, User> CheckUnauthenticatedUser(ClaimsPrincipal user)
		{
			if (user.Identity.IsAuthenticated)
				// it will be replaced by real user later
				return GetAnonymousVisitor(_predefinedAnonymousUserId);

			return Error.NotAuthenticated();
		}

		private static EitherAsync<Error, User> GetAnonymousUser()
		{
			return GetAnonymousVisitor(_predefinedAnonymousUserId);
		}

		private static Option<Unit> IsNotPredefinedSystemId(string idValue)
		{
			var systemUsers = new[] { _predefinedAnonymousUserId, _authZUserId }
				.Select(id => id.ToString().ToLowerInvariant());

			return systemUsers.Contains(idValue, StringComparer.OrdinalIgnoreCase)
				? Option<Unit>.None
				: Option<Unit>.Some(Unit.Default);
		}

		private static User GetAnonymousVisitor(UserId userId)
		{
			var result = new User(userId, TenantId.Default);

			result.AssignRole(_visitorRole);

			return result;
		}

		private static User GetAuthZHandlerUser()
		{
			var result = new User(_authZUserId, TenantId.Default);

			result.AssignRole(AuthorizationService._authZRoleId);

			return result;
		}

		private EitherAsync<Error, User> GetUser(string idValue)
		{
			var getRequest = new GetUser.Request
			{
				UserId = new UserId(idValue),
				Tenant = TenantId.Default
			};

			using var _ = BeginAuthZUserScope();
			return _getUserHandler.Handle(getRequest);
		}

		private EitherAsync<Error, User> CreateUser(string idValue, ClaimsPrincipal user)
		{
			var role = _visitorRole;
			if (IsTrustedApiClientAuthentication(idValue, user))
			{
				role = _sysAdminRole;
			}

			var createRequest = new CreateUser.Request
			{
				UserId = new UserId(idValue),
				Tenant = TenantId.Default,
				Role = role
			};
			using var _ = BeginAuthZUserScope();
			return _createUserHandler.Handle(createRequest);
		}

		private bool IsTrustedApiClientAuthentication(string idValue, ClaimsPrincipal user)
		{
			if (_applicationInfo.IsRunningInProduction) return false;

			static bool AreEqual(string x, string y)
			{
				return string.Compare(x, y, StringComparison.OrdinalIgnoreCase) == 0;
			}

			var azpClaim = GetClaimValue(user, AUTHORIZED_PARTY_CLAIM);
			var gtyClaim = GetClaimValue(user, GRANT_TYPE_CLAIM);

			return AreEqual($"{_trustOptions.Auth0ClientId}@clients", idValue)
				   && AreEqual(azpClaim.ValueUnsafe(), _trustOptions.Auth0ClientId)
				   && AreEqual(gtyClaim.ValueUnsafe(), "client-credentials");
		}

		private Option<string> GetUserIdFromClaims(ClaimsPrincipal user)
		{
			var idClaim = user.FindFirst(claim => _idClaimTypes.Contains(claim.Type));
			return idClaim != null
				? Option<string>.Some(idClaim.Value)
				: Option<string>.None;
		}

		private Option<string> GetClaimValue(ClaimsPrincipal user, string claimToFind)
		{
			var claim = user.FindFirst(c =>
				string.Compare(c.Type, claimToFind, StringComparison.OrdinalIgnoreCase) == 0);
			return claim != null
				? Option<string>.Some(claim.Value)
				: Option<string>.None;
		}

		/// <summary>
		/// Starts Current user scope with internal AuthZ handler user
		/// </summary>
		/// <returns></returns>
		private CurrentUserScope BeginAuthZUserScope()
		{
			return _currentUserManager.BeginScope(BuildCurrentUserContext(GetAuthZHandlerUser()));
		}

		private void SetCurrentContext(User user)
		{
			_currentUserManager.Context = BuildCurrentUserContext(user);
		}

		private static CurrentUserContext BuildCurrentUserContext(User user)
		{
			return new CurrentUserContext { User = user };
		}
	}
}