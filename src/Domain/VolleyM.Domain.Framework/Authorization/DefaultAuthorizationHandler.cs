﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.IdentityAndAccess;
using VolleyM.Domain.IdentityAndAccess.Handlers;

namespace VolleyM.Domain.Framework.Authorization
{
    public class DefaultAuthorizationHandler : IAuthorizationHandler
    {
        private readonly UserId _predefinedAnonymousUserId = new UserId("anonym@volleym.idp");

        private readonly IRequestHandler<GetUser.Request, User> _getUserHandler;
        private readonly IRequestHandler<CreateUser.Request, Unit> _createUserHandler;
        private readonly ICurrentUserManager _currentUserManager;

        private readonly List<string> _idClaimTypes = new List<string>
        {
            "sub",
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
        };

        public DefaultAuthorizationHandler(
            IRequestHandler<CreateUser.Request, Unit> createUserHandler,
            ICurrentUserManager currentUserManager,
            IRequestHandler<GetUser.Request, User> getUserHandler)
        {
            _createUserHandler = createUserHandler;
            _currentUserManager = currentUserManager;
            _getUserHandler = getUserHandler;
        }

        public async Task<Result<Unit>> AuthorizeUser(ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
            {
                SetCurrentContext(_predefinedAnonymousUserId, TenantId.Default);
                return Unit.Value;
            }

            var idValue = GetUserIdFromClaims(user);

            if (idValue == null)
            {
                return Error.NotAuthorized("UserId claim is missing");
            }

            if (IsPredefinedSystemId(idValue))
            {
                return Error.NotAuthorized("User Id is invalid");
            }

            Result<Unit> result;
            var getRequest = new GetUser.Request
            {
                UserId = new UserId(idValue),
                Tenant = TenantId.Default
            };
            var getUser = await _getUserHandler.Handle(getRequest);

            if (getUser.IsSuccessful)
            {
                result = Unit.Value;
                SetCurrentContext(getUser.Value.Id, getUser.Value.Tenant);
            }
            else if (getUser.Error.Type == ErrorType.NotFound)
            {
                var createRequest = new CreateUser.Request
                {
                    UserId = new UserId(idValue),
                    Tenant = TenantId.Default
                };
                var createUser = await _createUserHandler.Handle(createRequest);
                if (createUser.IsSuccessful)
                {
                    result = Unit.Value;
                    SetCurrentContext(createRequest.UserId, createRequest.Tenant);
                }
                else
                {
                    result = createUser.Error;
                }
            }
            else
            {
                result = getUser.Error;
            }

            return result;
        }

        private void SetCurrentContext(UserId user, TenantId tenant)
        {
            var userCtx = new CurrentUserContext
            {
                Tenant = tenant,
                User = user
            };

            _currentUserManager.SetCurrentContext(userCtx);
        }

        private string GetUserIdFromClaims(ClaimsPrincipal user)
        {
            var idClaim = user.FindFirst(type => _idClaimTypes.Contains(type.Type));
            return idClaim?.Value;
        }

        private bool IsPredefinedSystemId(string idValue)
        {
            return _predefinedAnonymousUserId.ToString()
                .Equals(idValue, StringComparison.OrdinalIgnoreCase);
        }
    }
}