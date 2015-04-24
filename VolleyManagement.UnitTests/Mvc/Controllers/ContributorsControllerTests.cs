﻿namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using Contracts;
    using Contracts.Exceptions;
    using Domain.Contributors;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;

    using VolleyManagement.UI.Areas.Mvc.Controllers;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Contributors;
    using VolleyManagement.UnitTests.Mvc.ViewModels;
    using VolleyManagement.UnitTests.Services.ContributorService;

    /// <summary>
    /// Tests for MVC ContributorController class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ContributorsControllerTests
    {
        private readonly ContributorServiceTestFixture _testFixture = new ContributorServiceTestFixture();
        private readonly Mock<IContributorService> _contributorServiceMock = new Mock<IContributorService>();

        private IKernel _kernel;

        /// <summary>
        /// Initializes test data
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            this._kernel = new StandardKernel();
            this._kernel.Bind<IContributorService>()
                   .ToConstant(this._contributorServiceMock.Object);
        }

        /// <summary>
        /// Test for Index action. The action should return not empty contributors list
        /// </summary>
        [TestMethod]
        public void Index_ContributorsExist_ContributorsReturned()
        {
            // Arrange
            var testData = _testFixture.TestContributors()
                                       .Build();
            this.MockContributors(testData);

            var sut = this._kernel.Get<ContributorsController>();

            var expected = new ContributorServiceTestFixture()
                                            .TestContributors()
                                            .Build()
                                            .ToList();

            // Act
            var actual = TestExtensions.GetModel<IEnumerable<Contributor>>(sut.Index()).ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new ContributorComparer());
        }

        /// <summary>
        /// Test with negative scenario for Index action.
        /// The action should thrown Argument null exception
        /// </summary>
        [TestMethod]
        public void Index_ContributorsDoNotExist_ExceptionThrown()
        {
            // Arrange
            this._contributorServiceMock.Setup(tr => tr.Get())
                .Throws(new ArgumentNullException());

            var sut = this._kernel.Get<ContributorsController>();
            var expected = (int)HttpStatusCode.NotFound;

            // Act
            var actual = (sut.Index() as HttpNotFoundResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Mocks test data
        /// </summary>
        /// <param name="testData">Data to mock</param>
        private void MockContributors(IEnumerable<Contributor> testData)
        {
            this._contributorServiceMock.Setup(cn => cn.Get())
                .Returns(testData.AsQueryable());
        }
    }
}