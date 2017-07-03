﻿namespace VolleyManagement.Services.Infrastructure
{
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Authentication;
    using VolleyManagement.Contracts.Authentication.Models;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Crosscutting.Contracts.Infrastructure.IOC;
    using VolleyManagement.Crosscutting.Contracts.MailService;
    using VolleyManagement.Services.Authentication;
    using VolleyManagement.Services.Authorization;

    public class IocServicesModule : IIocRegistrationModule
    {
        public void RegisterDependencies(IIocContainer container)
        {
            container
                .Register<ITournamentService, TournamentService>(IocLifetimeEnum.Scoped)
                .Register<IPlayerService, PlayerService>(IocLifetimeEnum.Scoped)
                .Register<IContributorTeamService, ContributorTeamService>(IocLifetimeEnum.Scoped)
                .Register<ITeamService, TeamService>(IocLifetimeEnum.Scoped)
                .Register<IVolleyUserManager<UserModel>, VolleyUserManager>(IocLifetimeEnum.Scoped)
                .Register<IVolleyUserStore, VolleyUserStore>(IocLifetimeEnum.Scoped)
                .Register<IRolesService, RolesService>(IocLifetimeEnum.Scoped)
                .Register<IGameService, GameService>(IocLifetimeEnum.Scoped)
                .Register<IGameReportService, GameReportService>(IocLifetimeEnum.Scoped)
                .Register<IAuthorizationService, AuthorizationService>(IocLifetimeEnum.Scoped)
                .Register<IUserService, UserService>(IocLifetimeEnum.Scoped)
                .Register<IFeedbackService, FeedbackService>(IocLifetimeEnum.Scoped)
                .Register<ITournamentRequestService, TournamentRequestService>(IocLifetimeEnum.Scoped)
                .Register<IRequestService, RequestService>(IocLifetimeEnum.Scoped)
                .Register<ICacheProvider, CacheProvider>(IocLifetimeEnum.Scoped)
                .Register<IMailService, GmailAccountMailService>(IocLifetimeEnum.Scoped);
        }
    }
}
