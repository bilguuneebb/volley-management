﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using SimpleInjector;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests.Fixture
{
    internal class UnitTestIdentityAndAccessFixture : IIdentityAndAccessFixture
    {
        private readonly DomainPipelineFixtureBase _baseFixture;
        private IUserRepository _repositoryMock;

        public UnitTestIdentityAndAccessFixture(DomainPipelineFixtureBase baseFixture)
        {
            _baseFixture = baseFixture;
        }

        public void Setup()
        {
            _repositoryMock = Substitute.For<IUserRepository>();

            _baseFixture.Register(() => _repositoryMock, Lifestyle.Scoped);
        }

        public void ConfigureUserExists(TenantId tenant, UserId id, User user)
        {
            _repositoryMock.Get(tenant, id).Returns(user);
        }

        public void ConfigureUserDoesNotExist(TenantId tenant, UserId id)
        {
            _repositoryMock.Get(tenant, id).Returns(Error.NotFound());
        }

        public void VerifyUserCreated(User user)
        {
            _repositoryMock.Received()
                .Add(Arg.Is<User>(u => u.Id == user.Id || u.Tenant == user.Tenant));
        }

        public void CleanUpUsers(List<Tuple<TenantId, UserId>> usersToTeardown)
        {
            // do nothing
        }

        public void OneTimeSetup(IConfiguration configuration)
        {
            // nothing to do
        }

        public void OneTimeTearDown()
        {
            // nothing to do
        }
    }
}