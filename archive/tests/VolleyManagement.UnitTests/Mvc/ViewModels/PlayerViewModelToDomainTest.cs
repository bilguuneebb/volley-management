﻿namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using Domain.PlayersAggregate;
    using Xunit;
    using Services.PlayerService;

    /// <summary>
    /// View model player class test
    /// </summary> 
    public class PlayerViewModelToDomainTest
    {
        /// <summary>
        /// ToDomain() method test.
        /// Does correct player view model mapped to domain model.
        /// </summary>
        [Fact]
        public void ToDomain_PlayerViewModel_MappedToDomain()
        {
            // Arrange
            var testViewModel = new PlayerMvcViewModelBuilder()
                .WithId(1)
                .WithFirstName("FirstName")
                .WithLastName("LastName")
                .WithBirthYear(1983)
                .WithHeight(186)
                .WithWeight(95)
                .Build();

            var testDomainModel = new PlayerBuilder(1, "FirstName", "LastName")
                .WithBirthYear(1983)
                .WithHeight(186)
                .WithWeight(95)
                .Build();

            // Act
            var actual = testViewModel.ToDomain();

            // Assert
            Assert.Equal<Player>(testDomainModel, actual, new PlayerComparer());
        }
    }
}
