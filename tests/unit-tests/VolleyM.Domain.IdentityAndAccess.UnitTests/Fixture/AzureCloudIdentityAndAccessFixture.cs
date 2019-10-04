﻿using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VolleyM.Domain.UnitTests.Framework;
using VolleyM.Infrastructure.IdentityAndAccess.AzureStorage.TableConfiguration;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests.Fixture
{
    public class AzureCloudIdentityAndAccessFixture : IIdentityAndAccessFixture
    {
        private readonly DomainPipelineFixtureBase _baseFixture;

        private TableConfiguration _tableConfig;

        public AzureCloudIdentityAndAccessFixture(DomainPipelineFixtureBase baseFixture)
        {
            _baseFixture = baseFixture;
        }

        public void Setup()
        {

        }

        public void ConfigureUserExists(TenantId tenant, UserId id, User user)
        {
            var repo = _baseFixture.Resolve<IUserRepository>();

            repo.Add(user);
        }

        public void ConfigureUserDoesNotExist(TenantId tenant, UserId id)
        {
            // do nothing
        }

        public void VerifyUserCreated(User user)
        {
            var repo = _baseFixture.Resolve<IUserRepository>();

            var savedUser = repo.Get(user.Tenant, user.Id).Result;

            savedUser.Should().BeSuccessful("user should be created");
            savedUser.Value.Should().BeEquivalentTo(user, "all attributes should be saved correctly");
        }

        public void CleanUpUsers(List<Tuple<TenantId, UserId>> usersToTeardown)
        {
            var repo = _baseFixture.Resolve<IUserRepository>();
            var deleteTasks = new List<Task>();
            foreach (var (tenant, user) in usersToTeardown)
            {
                deleteTasks.Add(repo.Delete(tenant, user));
            }

            Task.WaitAll(deleteTasks.ToArray());
        }

        public void OneTimeSetup(IConfiguration configuration)
        {
            var options = configuration.GetSection("IdentityContextTableStorageOptions")
                .Get<IdentityContextTableStorageOptions>();

            _tableConfig = new TableConfiguration(options);
            var result = _tableConfig.ConfigureTables().Result;

            result.Should().BeSuccessful("Azure Storage should be configured correctly");
        }

        public void OneTimeTearDown()
        {
            var result = _tableConfig.CleanTables().Result;
            result.Should().BeSuccessful("Azure Storage should be cleaned up correctly");
        }
    }
}