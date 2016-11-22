﻿namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Contracts;
    using Contracts.Exceptions;
    using Crosscutting.Contracts.Providers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Domain.GamesAggregate;
    using VolleyManagement.Domain.RolesAggregate;
    using VolleyManagement.Domain.TeamsAggregate;
    using VolleyManagement.Domain.TournamentsAggregate;
    using VolleyManagement.UI.Areas.Mvc.Controllers;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.GameResults;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Teams;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments;
    using VolleyManagement.UnitTests.Mvc.ViewModels;
    using VolleyManagement.UnitTests.Services.GameService;
    using VolleyManagement.UnitTests.Services.TeamService;
    using VolleyManagement.UnitTests.Services.TournamentService;

    /// <summary>
    /// Tests for MVC TournamentController class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TournamentsControllerTests
    {
        private const int TEST_ID = 1;
        private const int TEST_TOURNAMENT_ID = 1;
        private const string TEST_TOURNAMENT_NAME = "Name";
        private const int TEST_ROUND_NUMBER = 2;
        private const int EMPTY_TEAMLIST_COUNT = 0;
        private const byte FIRST_ROUND_NUMBER = 1;
        private const byte SECOND_ROUND_NUMBER = 2;
        private const string ASSERT_FAIL_VIEW_MODEL_MESSAGE = "View model must be returned to user.";
        private const string ASSERT_FAIL_JSON_RESULT_MESSAGE = "Json result must be returned to user.";
        private const string INDEX_ACTION_NAME = "Index";
        private const string SHOW_SCHEDULE_ACTION_NAME = "ShowSchedule";
        private const string ROUTE_VALUES_KEY = "action";
        private const string MANAGE_TOURNAMENT_TEAMS = "/Teams/ManageTournamentTeams?tournamentId=";
        private const int DAYS_TO_APPLYING_PERIOD_START = 14;
        private const int DAYS_FOR_APPLYING_PERIOD = 14;
        private const int DAYS_FROM_APPLYING_PERIOD_END_TO_GAMES_START = 7;
        private const int DAYS_FOR_GAMES_PERIOD = 120;
        private const int DAYS_FROM_GAMES_START_TO_TRANSFER_START = 1;
        private const int DAYS_FOR_TRANSFER_PERIOD = 21;
        private static readonly DateTime _testDate = new DateTime(1996, 07, 25);

        private readonly List<AuthOperation> _allowedOperationsShowSchedule = new List<AuthOperation>
                {
                    AuthOperations.Games.Create,
                    AuthOperations.Games.Edit,
                    AuthOperations.Games.Delete,
                    AuthOperations.Games.SwapRounds,
                    AuthOperations.Games.EditResult
                };

        private readonly Mock<ITournamentService> _tournamentServiceMock = new Mock<ITournamentService>();
        private readonly Mock<IGameService> _gameServiceMock = new Mock<IGameService>();
        private readonly Mock<IAuthorizationService> _authServiceMock = new Mock<IAuthorizationService>();
        private readonly Mock<ICurrentUserService> _currentUserServiceMock = new Mock<ICurrentUserService>();
        private readonly Mock<ITournamentRequestService> _tournamentRequestServiceMock = new Mock<ITournamentRequestService>();
        private readonly Mock<ITeamService> _teamServiceMock = new Mock<ITeamService>();
        private readonly Mock<HttpContextBase> _httpContextMock = new Mock<HttpContextBase>();
        private readonly Mock<HttpRequestBase> _httpRequestMock = new Mock<HttpRequestBase>();
        private readonly Mock<TimeProvider> _timeMock = new Mock<TimeProvider>();

        private IKernel _kernel;
        private TournamentsController _sut;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            this._kernel = new StandardKernel();
            this._kernel.Bind<ITournamentService>().ToConstant(this._tournamentServiceMock.Object);
            this._kernel.Bind<IGameService>().ToConstant(this._gameServiceMock.Object);
            this._kernel.Bind<IAuthorizationService>().ToConstant(this._authServiceMock.Object);
            this._kernel.Bind<ICurrentUserService>().ToConstant(this._currentUserServiceMock.Object);
            this._kernel.Bind<ITournamentRequestService>().ToConstant(this._tournamentRequestServiceMock.Object);
            this._kernel.Bind<ITeamService>().ToConstant(this._teamServiceMock.Object);
            this._httpContextMock.SetupGet(c => c.Request).Returns(this._httpRequestMock.Object);
            this._sut = this._kernel.Get<TournamentsController>();
            TimeProvider.Current = _timeMock.Object;
            this._timeMock.Setup(tp => tp.UtcNow).Returns(_testDate);
        }

        /// <summary>
        /// Cleanup test data
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            TimeProvider.ResetToDefault();
        }

        #region Index
        /// <summary>
        /// Test for Index method. Actual tournaments (current and upcoming) are requested. Actual tournaments are returned.
        /// </summary>
        [TestMethod]
        public void Index_GetActualTournaments_ActualTournamentsAreReturned()
        {
            // Arrange
            var testData = MakeTestTournaments();
            var expectedCurrentTournaments = GetTournamentsWithState(testData, TournamentStateEnum.Current);
            var expectedUpcomingTournaments = GetTournamentsWithState(testData, TournamentStateEnum.Upcoming);
            SetupGetActual(testData);

            // Act
            var actualCurrentTournaments = TestExtensions.GetModel<TournamentsCollectionsViewModel>(this._sut.Index())
                .CurrentTournaments.ToList();
            var actualUpcomingTournaments = TestExtensions.GetModel<TournamentsCollectionsViewModel>(this._sut.Index())
                .UpcomingTournaments.ToList();

            // Assert
            CollectionAssert.AreEqual(expectedCurrentTournaments, actualCurrentTournaments, new TournamentComparer());
            CollectionAssert.AreEqual(expectedUpcomingTournaments, actualUpcomingTournaments, new TournamentComparer());
        }
        #endregion

        #region ManageTournamentTeams
        /// <summary>
        /// Test for ManageTournamentTeams.
        /// Actual tournament teams are requested. Actual tournament teams are returned.
        /// </summary>
        [TestMethod]
        public void ManageTournamentTeams_TournamentTeamsExist_TeamsInCurrentTournamentAreReturned()
        {
            // Arrange
            var testData = MakeTestTeams();
            SetupGetTournamentTeams(testData, TEST_TOURNAMENT_ID);
            var expectedTeamsList = new TournamentTeamsListViewModel(testData, TEST_TOURNAMENT_ID);
            SetupRequestRawUrl(MANAGE_TOURNAMENT_TEAMS + TEST_TOURNAMENT_ID);
            SetupControllerContext();

            // Act
            var returnedTeamsList = TestExtensions.GetModel<TournamentTeamsListReferrerViewModel>(
                this._sut.ManageTournamentTeams(TEST_TOURNAMENT_ID));

            // Assert
            Assert.IsTrue(new TournamentTeamsListViewModelComparer()
                .AreEqual(expectedTeamsList, returnedTeamsList.Model));
            Assert.AreEqual(returnedTeamsList.Referer, this._sut.Request.RawUrl);
        }

        /// <summary>
        /// Test for ManageTournamentTeams while there are no teams.
        /// Actual tournament teams are requested. Empty teams list is returned.
        /// </summary>
        [TestMethod]
        public void ManageTournamentTeams_NonExistTournamentTeams_EmptyTeamListIsReturned()
        {
            // Arrange
            var testData = new TeamServiceTestFixture().Build();
            SetupGetTournamentTeams(testData, TEST_TOURNAMENT_ID);
            SetupRequestRawUrl(MANAGE_TOURNAMENT_TEAMS + TEST_TOURNAMENT_ID);
            SetupControllerContext();

            // Act
            var returnedTeamsList = TestExtensions.GetModel<TournamentTeamsListReferrerViewModel>(
                this._sut.ManageTournamentTeams(TEST_TOURNAMENT_ID));

            // Assert
            Assert.AreEqual(returnedTeamsList.Model.List.Count, EMPTY_TEAMLIST_COUNT);
            Assert.AreEqual(returnedTeamsList.Referer, this._sut.Request.RawUrl);
        }
        #endregion

        #region ShowSchedule

        /// <summary>
        /// Test for ShowSchedule method.
        /// Wrong tournament Id passed. View with error message is returned.
        /// </summary>
        [TestMethod]
        public void ShowSchedule_NonExistentTournament_ErrorViewIsReturned()
        {
            // Arrange
            SetupGet(TEST_TOURNAMENT_ID, null as Tournament);

            // Act
            var result = TestExtensions.GetModel<ScheduleViewModel>(this._sut.ShowSchedule(TEST_TOURNAMENT_ID));

            // Assert
            Assert.IsFalse(_sut.ModelState.IsValid);
            Assert.IsTrue(_sut.ModelState.ContainsKey("LoadError"));
            Assert.IsNull(result, "Result should be null");

            VerifyGetAllowedOperations(_allowedOperationsShowSchedule, Times.Never());
        }

        /// <summary>
        /// Test for ShowSchedule method.
        /// Valid rounds is passed, no exception occurred.
        /// </summary>
        [TestMethod]
        public void ShowSchedule_TournamentHasGamesScheduled_RoundsCreatedCorrectly()
        {
            // Arrange
            const int TEST_ROUND_COUNT = 3;

            var tournament = new TournamentScheduleDto
            {
                Id = TEST_TOURNAMENT_ID,
                Name = TEST_TOURNAMENT_NAME,
                Scheme = TournamentSchemeEnum.One
            };

            var expected = new ScheduleViewModelBuilder().Build();

            SetupGetTournamentNumberOfRounds(tournament, TEST_ROUND_COUNT);
            SetupGetScheduleInfo(TEST_TOURNAMENT_ID, tournament);
            SetupGetTournamentResults(
                TEST_TOURNAMENT_ID,
                new GameServiceTestFixture().TestGameResults().Build());

            // Act
            var actual = TestExtensions.GetModel<ScheduleViewModel>(this._sut.ShowSchedule(TEST_TOURNAMENT_ID));

            // Assert
            Assert.IsTrue(new ScheduleViewModelComparer().AreRoundsEqual(actual.Rounds, expected.Rounds));
            VerifyGetAllowedOperations(_allowedOperationsShowSchedule, Times.Once());
        }

        /// <summary>
        /// Test for ShowSchedule method.
        /// Valid schedule is passed, no exception occurred.
        /// </summary>
        [TestMethod]
        public void ShowSchedule_ValidScheduleViewModel_ScheduleViewModelIsReturned()
        {
            // Arrange
            const int TEST_ROUND_COUNT = 3;
            var tournament = new TournamentScheduleDto
            {
                Id = TEST_TOURNAMENT_ID,
                Name = TEST_TOURNAMENT_NAME,
                Scheme = TournamentSchemeEnum.One
            };

            SetupGetTournamentNumberOfRounds(tournament, TEST_ROUND_COUNT);
            SetupGetScheduleInfo(
                TEST_TOURNAMENT_ID,
                tournament);
            SetupGetTournamentResults(
                TEST_TOURNAMENT_ID,
                new GameServiceTestFixture().TestGameResults().Build());

            var expected = new ScheduleViewModelBuilder().WithTournamentScheme(TournamentSchemeEnum.One).Build();

            // Act
            var actual = TestExtensions.GetModel<ScheduleViewModel>(this._sut.ShowSchedule(TEST_TOURNAMENT_ID));

            // Assert
            Assert.IsTrue(new ScheduleViewModelComparer().AreEqual(actual, expected));
            VerifyGetAllowedOperations(_allowedOperationsShowSchedule, Times.Once());
        }

        /// <summary>
        /// Test for ShowSchedule method.
        /// Valid schedule is passed, no exception occurred.
        /// </summary>
        [TestMethod]
        public void ShowSchedule_PlayoffScheme_RoundNamesAreCreated()
        {
            // Arrange
            const int TEST_ROUND_COUNT = 5;
            var tournament = new TournamentScheduleDto
            {
                Id = TEST_TOURNAMENT_ID,
                Name = TEST_TOURNAMENT_NAME,
                Scheme = TournamentSchemeEnum.PlayOff
            };

            SetupGetScheduleInfo(
                TEST_TOURNAMENT_ID,
                tournament);
            SetupGetTournamentResults(
                TEST_TOURNAMENT_ID,
                new GameServiceTestFixture().TestGameResults().Build());

            SetupGetTournamentNumberOfRounds(tournament, TEST_ROUND_COUNT);
            var expectedRoundNames = new string[] { "Round of 32", "Round of 16", "Quarter final", "Semifinal", "Final" };
            var expected = new ScheduleViewModelBuilder().WithRoundNames(expectedRoundNames).Build();

            // Act
            var actual = TestExtensions.GetModel<ScheduleViewModel>(this._sut.ShowSchedule(TEST_TOURNAMENT_ID));

            // Assert
            CollectionAssert.AreEqual(actual.RoundNames, expected.RoundNames);
        }
        #endregion

        #region AddTeamsToTournament
        /// <summary>
        /// Test for AddTeamsToTournament.
        /// Tournament teams list view model is valid and no exception is thrown during adding
        /// Teams are added successfully and json result is returned
        /// </summary>
        [TestMethod]
        public void AddTeamsToTournament_ValidTeamListViewModelNoException_JsonResultIsReturned()
        {
            // Arrange
            var testData = MakeTestTeams();
            var expectedDataResult = new TournamentTeamsListViewModel(testData, TEST_TOURNAMENT_ID);

            // Act
            var jsonResult = this._sut.AddTeamsToTournament(new TournamentTeamsListViewModel(testData, TEST_TOURNAMENT_ID));
            var returnedDataResult = jsonResult.Data as TournamentTeamsListViewModel;

            // Assert
            Assert.IsTrue(new TournamentTeamsListViewModelComparer()
                .AreEqual(returnedDataResult, expectedDataResult));
        }

        /// <summary>
        /// Test for AddTeamsToTournament.
        /// Tournament teams list view model is invalid and Argument exception is thrown during adding
        /// Teams are not added and json result  with model error is returned
        /// </summary>
        [TestMethod]
        public void AddTeamsToTournament_InValidTeamListViewModelWithException_JsonModelErrorReturned()
        {
            // Arrange
            var testData = MakeTestTeams();
            this._tournamentServiceMock
                .Setup(ts => ts.AddTeamsToTournament(It.IsAny<List<Team>>(), It.IsAny<int>()))
                .Throws(new ArgumentException(string.Empty));

            // Act
            var jsonResult = this._sut.AddTeamsToTournament(new TournamentTeamsListViewModel(testData, TEST_TOURNAMENT_ID));
            var modelResult = jsonResult.Data as TeamsAddToTournamentViewModel;

            // Assert
            Assert.IsNotNull(modelResult.Message);
        }
        #endregion

        #region ScheduleGameGetAction
        /// <summary>
        /// Test for ScheduleGame method (GET action). Wrong tournament Id passed. View with error message is returned.
        /// </summary>
        [TestMethod]
        public void ScheduleGameGetAction_NonExistentTournament_ErrorViewIsReturned()
        {
            // Arrange
            SetupGet(TEST_TOURNAMENT_ID, null as Tournament);

            // Act
            var result = TestExtensions.GetModel<GameViewModel>(this._sut.ScheduleGame(TEST_TOURNAMENT_ID));

            // Assert
            Assert.IsFalse(_sut.ModelState.IsValid);
            Assert.IsNull(result);
        }

        /// <summary>
        /// Test for ScheduleGame method (GET action).
        /// Tournament with scheme 1 and no teams passed. View with error message is returned.
        /// </summary>
        [TestMethod]
        public void ScheduleGameGetAction_NoTeamsAvailable_ErrorViewIsReturned()
        {
            // Arrange
            var testData = MakeTestTournament(TEST_TOURNAMENT_ID);
            SetupGet(TEST_TOURNAMENT_ID, testData);
            SetupGetTournamentTeams(new List<Team>(), TEST_TOURNAMENT_ID);

            // Act
            var result = TestExtensions.GetModel<GameViewModel>(this._sut.ScheduleGame(TEST_TOURNAMENT_ID));

            // Assert
            VerifyInvalidModelState("LoadError", result);
        }

        /// <summary>
        /// Test for ScheduleGame method (GET action). Tournament with scheme 1 and 3 teams passed. View with GameViewModel is returned.
        /// </summary>
        [TestMethod]
        public void ScheduleGameGetAction_TournamentExists_GameViewModelIsReturned()
        {
            // Arrange
            const int MIN_ROUND_NUMBER = 1;
            const int TEST_ROUND_COUNT = 3;

            var testTournament = new TournamentScheduleDto { Id = TEST_TOURNAMENT_ID, StartDate = _testDate };
            var testTeams = MakeTestTeams();
            SetupGetScheduleInfo(TEST_TOURNAMENT_ID, testTournament);
            SetupGetTournamentTeams(testTeams, TEST_TOURNAMENT_ID);
            SetupGetTournamentNumberOfRounds(testTournament, TEST_ROUND_COUNT);

            var expected = new GameViewModel
            {
                TournamentId = TEST_TOURNAMENT_ID,
                GameDate = _testDate,
                Teams = new SelectList(testTeams, "Id", "Name"),
                Rounds = new SelectList(Enumerable.Range(MIN_ROUND_NUMBER, TEST_ROUND_COUNT))
            };

            // Act
            var actual = TestExtensions.GetModel<GameViewModel>(this._sut.ScheduleGame(TEST_TOURNAMENT_ID));

            // Assert
            AssertEqual(actual, expected);
        }
        #endregion

        #region ScheduleGamePostAction
        /// <summary>
        /// Test for ScheduleGame method (POST action).
        /// Valid game is passed, no exception occurs.
        /// Game is created and browser is redirected to ShowSchedule action.
        /// </summary>
        [TestMethod]
        public void ScheduleGamePostAction_ValidGameViewModel_GameIsCreatedRedirectToSchedule()
        {
            // Arrange
            var testData = MakeTestGameViewModel();
            var redirect = true;

            // Act
            var result = this._sut.ScheduleGame(testData, redirect) as RedirectToRouteResult;

            // Assert
            VerifyCreateGame(Times.Once());
            VerifyRedirect(SHOW_SCHEDULE_ACTION_NAME, result);
        }

        /// <summary>
        /// Test for ScheduleGame method (POST action).
        /// Valid game is passed, no exception occurs.
        /// Game is created. Browser is not redirected.
        /// </summary>
        [TestMethod]
        public void ScheduleGamePostAction_ValidGameViewModel_GameIsCreated()
        {
            // Arrange
            var testData = MakeTestGameViewModel();
            var redirect = false;

            // Act
            var result = this._sut.ScheduleGame(testData, redirect) as ViewResult;

            // Assert
            VerifyCreateGame(Times.Once());
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Test for ScheduleGame method (POST action).
        /// Valid game is passed, but ArgumentException occurs.
        /// Game is not created. Browser is redirected to ScheduleGame action.
        /// </summary>
        [TestMethod]
        public void ScheduleGamePostAction_ServiceValidationFails_ScheduleGameViewIsReturned()
        {
            // Arrange
            var testData = MakeTestGameViewModel();
            var redirect = false;
            this._gameServiceMock.Setup(ts => ts.Create(It.IsAny<Game>()))
                            .Throws(new ArgumentException(string.Empty));

            // Act
            var result = TestExtensions.GetModel<GameViewModel>(this._sut.ScheduleGame(testData, redirect));

            // Assert
            VerifyInvalidModelState("ValidationError", result);
        }

        /// <summary>
        /// Test for ScheduleGame method (POST action). Invalid game view model is passed, HttpNotFound returned
        /// </summary>
        [TestMethod]
        public void ScheduleGamePostAction_InvalidGameViewModel_ScheduleGameViewIsReturned()
        {
            // Arrange
            var testData = MakeTestGameViewModel();
            var redirect = false;
            this._sut.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = this._sut.ScheduleGame(testData, redirect) as ViewResult;

            // Assert
            VerifyCreateGame(Times.Never());
            Assert.IsNotNull(result);
        }
        #endregion

        #region EditScheduledGameGetAction
        /// <summary>
        /// Test for EditScheduledGame method (GET action). Correct game id passed. View with GameViewModel is returned.
        /// </summary>
        [TestMethod]
        public void EditScheduledGameGetAction_GameExists_GameViewModelIsReturned()
        {
            // Arrange
            const int MIN_ROUND_NUMBER = 1;
            const int TEST_ROUND_COUNT = 3;

            var testGame = new GameResultDto
            {
                Id = TEST_ID,
                HomeTeamId = TEST_ID,
                AwayTeamId = TEST_ID,
                TournamentId = TEST_TOURNAMENT_ID,
                Round = TEST_ROUND_NUMBER,
                GameDate = _testDate
            };
            SetupGetGame(TEST_ID, testGame);

            var testTournament = new TournamentScheduleDto { Id = TEST_TOURNAMENT_ID, StartDate = _testDate };
            var testTeams = MakeTestTeams();
            SetupGetScheduleInfo(TEST_TOURNAMENT_ID, testTournament);
            SetupGetTournamentTeams(testTeams, TEST_TOURNAMENT_ID);
            SetupGetTournamentNumberOfRounds(testTournament, TEST_ROUND_COUNT);

            var expected = GameViewModel.Map(testGame);
            expected.Teams = new SelectList(testTeams, "Id", "Name");
            expected.Rounds = new SelectList(Enumerable.Range(MIN_ROUND_NUMBER, TEST_ROUND_COUNT));

            // Act
            var actual = TestExtensions.GetModel<GameViewModel>(this._sut.EditScheduledGame(TEST_ID));

            // Assert
            AssertEqual(actual, expected);
        }

        /// <summary>
        /// Test for EditScheduledGame method (GET action). Wrong game Id passed. View with error message is returned.
        /// </summary>
        [TestMethod]
        public void EditScheduledGameGetAction_NonExistentGame_ErrorViewIsReturned()
        {
            // Arrange
            _gameServiceMock.Setup(gs => gs.Get(TEST_ID)).Returns(null as GameResultDto);

            // Act
            var result = TestExtensions.GetModel<GameViewModel>(this._sut.EditScheduledGame(TEST_ID));

            // Assert
            VerifyInvalidModelState("LoadError", result);
        }
        #endregion

        #region EditScheduledGamePostAction
        /// <summary>
        /// Test for EditScheduledGame method (POST action).
        /// Valid gameViewModel is passed, no exception occurs.
        /// Game is updated. Browser is redirected to ShowSchedule action.
        /// </summary>
        [TestMethod]
        public void EditScheduledGamePostAction_ValidGameViewModel_GameIsUpdated()
        {
            // Arrange
            var testData = MakeTestGameViewModel();

            // Act
            var result = this._sut.EditScheduledGame(testData) as RedirectToRouteResult;

            // Assert
            VerifyEditGame(Times.Once());
            VerifyRedirect("ShowSchedule", result);
        }

        /// <summary>
        /// Test for EditScheduledGame method (POST action).
        /// Valid game is passed, but ArgumentException occurs.
        /// Game is not updated. View with GameViewModel is returned.
        /// </summary>
        [TestMethod]
        public void EditScheduledGamePostAction_ServiceValidationFails_GameViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestGameViewModel();
            _gameServiceMock.Setup(gs => gs.Edit(It.IsAny<Game>())).Throws<ArgumentException>();

            // Act
            var result = TestExtensions.GetModel<GameViewModel>(this._sut.EditScheduledGame(testData));

            // Assert
            VerifyInvalidModelState("ValidationError", result);
        }

        /// <summary>
        /// Test for EditScheduledGame method (POST action).
        /// Valid game is passed, but MissingEntityException occurs.
        /// Game is not updated. View with error message is returned.
        /// </summary>
        [TestMethod]
        public void EditScheduledGamePostAction_NonExistentGameIsPassed_ErrorViewIsReturned()
        {
            // Arrange
            var testData = MakeTestGameViewModel();
            _gameServiceMock.Setup(gs => gs.Edit(It.IsAny<Game>())).Throws<MissingEntityException>();

            // Act
            var result = TestExtensions.GetModel<GameViewModel>(this._sut.EditScheduledGame(testData));

            // Assert
            VerifyInvalidModelState("LoadError", result);
        }

        /// <summary>
        /// Test for EditScheduledGame method (POST action). Invalid game view model is passed, view with GameViewModel is returned.
        /// </summary>
        [TestMethod]
        public void EditScheduledGamePostAction_InvalidGameViewModel_GameViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestGameViewModel();
            this._sut.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = this._sut.EditScheduledGame(testData) as ViewResult;

            // Assert
            VerifyCreateGame(Times.Never());
            Assert.IsNotNull(result);
        }
        #endregion

        #region GetFinished
        /// <summary>
        /// Test for GetFinished method. Finished tournaments are requested. JsonResult with finished tournaments is returned.
        /// </summary>
        [TestMethod]
        public void GetFinished_GetFinishedTournaments_JsonResultIsReturned()
        {
            // Arrange
            var testData = MakeTestTournaments();
            SetupGetFinished(testData);

            // Act
            var result = this._sut.GetFinished();

            // Assert
            Assert.IsNotNull(result, ASSERT_FAIL_JSON_RESULT_MESSAGE);
        }
        #endregion

        #region Details
        /// <summary>
        /// Test for Details method. Tournament with specified identifier does not exist. HttpNotFoundResult is returned.
        /// </summary>
        [TestMethod]
        public void Details_NonExistentTournament_HttpNotFoundResultIsReturned()
        {
            // Arrange
            SetupGet(TEST_TOURNAMENT_ID, null as Tournament);

            // Act
            var result = this._sut.Details(TEST_TOURNAMENT_ID);

            // Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        /// <summary>
        /// Test for Details method. Tournament with specified identifier exists. View model of Tournament is returned.
        /// </summary>
        [TestMethod]
        public void Details_ExistingTournament_TournamentViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestTournament(TEST_TOURNAMENT_ID);
            var expected = MakeTestTournamentViewModel(TEST_TOURNAMENT_ID);
            SetupGet(TEST_TOURNAMENT_ID, testData);

            // Act
            var actual = TestExtensions.GetModel<TournamentViewModel>(this._sut.Details(TEST_TOURNAMENT_ID));

            // Assert
            TestHelper.AreEqual<TournamentViewModel>(expected, actual, new TournamentViewModelComparer());
            VerifyGetAllowedOperations(Times.Once());
        }
        #endregion

        #region CreateGetAction
        /// <summary>
        /// Test for Create method (GET action). Tournament view model is requested. Tournament view model is returned.
        /// </summary>
        [TestMethod]
        public void CreateGetAction_GetTournamentViewModel_TournamentViewModelIsReturned()
        {
            // Arrange
            var now = _testDate;

            var expected = new TournamentViewModel
            {
                ApplyingPeriodStart = now.AddDays(DAYS_TO_APPLYING_PERIOD_START),
                ApplyingPeriodEnd = now.AddDays(DAYS_TO_APPLYING_PERIOD_START
                                              + DAYS_FOR_APPLYING_PERIOD),
                GamesStart = now.AddDays(DAYS_TO_APPLYING_PERIOD_START
                                       + DAYS_FOR_APPLYING_PERIOD
                                       + DAYS_FROM_APPLYING_PERIOD_END_TO_GAMES_START),
                GamesEnd = now.AddDays(DAYS_TO_APPLYING_PERIOD_START
                                     + DAYS_FOR_APPLYING_PERIOD
                                     + DAYS_FROM_APPLYING_PERIOD_END_TO_GAMES_START
                                     + DAYS_FOR_GAMES_PERIOD),
                TransferStart = now.AddDays(DAYS_TO_APPLYING_PERIOD_START
                                          + DAYS_FOR_APPLYING_PERIOD
                                          + DAYS_FROM_APPLYING_PERIOD_END_TO_GAMES_START
                                          + DAYS_FROM_GAMES_START_TO_TRANSFER_START),
                TransferEnd = now.AddDays(DAYS_TO_APPLYING_PERIOD_START
                                        + DAYS_FOR_APPLYING_PERIOD
                                        + DAYS_FROM_APPLYING_PERIOD_END_TO_GAMES_START
                                        + DAYS_FROM_GAMES_START_TO_TRANSFER_START
                                        + DAYS_FOR_TRANSFER_PERIOD)
            };

            // Act
            var actual = TestExtensions.GetModel<TournamentViewModel>(this._sut.Create());

            // Assert
            TestHelper.AreEqual<TournamentViewModel>(expected, actual, new TournamentViewModelComparer());
        }
        #endregion

        #region CreatePostAction
        /// <summary>
        /// Test for Create method (POST action). Tournament view model is valid and no exception is thrown during creation.
        /// Tournament is created successfully and user is redirected to the Index page.
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ValidTournamentViewModelNoException_TournamentIsCreated()
        {
            // Arrange
            var testData = MakeTestTournamentViewModel();

            // Act
            var result = this._sut.Create(testData) as RedirectToRouteResult;

            // Assert
            VerifyCreate(Times.Once());
            VerifyRedirect(INDEX_ACTION_NAME, result);
        }

        /// <summary>
        /// Test for Create method (POST action). Tournament view model is valid, but exception is thrown during creation.
        /// Tournament view model is returned.
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ValidTournamentViewModelWithException_TournamentViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestTournamentViewModel();
            SetupCreateThrowsTournamentValidationException();

            // Act
            var result = TestExtensions.GetModel<TournamentViewModel>(this._sut.Create(testData));

            // Assert
            VerifyCreate(Times.Once());
            Assert.IsNotNull(result, ASSERT_FAIL_VIEW_MODEL_MESSAGE);
        }

        /// <summary>
        /// Test for Create method (POST action). Tournament view model is not valid.
        /// Tournament is not created and tournament view model is returned.
        /// </summary>
        [TestMethod]
        public void CreatePostAction_InvalidTournamentViewModel_TournamentViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestTournamentViewModel();
            this._sut.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = TestExtensions.GetModel<TournamentViewModel>(this._sut.Create(testData));

            // Assert
            VerifyCreate(Times.Never());
            Assert.IsNotNull(result, ASSERT_FAIL_VIEW_MODEL_MESSAGE);
        }
        #endregion

        #region EditGetAction
        /// <summary>
        /// Test for Edit method (GET action). Tournament with specified identifier does not exist. HttpNotFoundResult is returned.
        /// </summary>
        [TestMethod]
        public void EditGetAction_NonExistentTournament_HttpNotFoundResultIsReturned()
        {
            // Arrange
            SetupGet(TEST_TOURNAMENT_ID, null as Tournament);

            // Act
            var result = this._sut.Edit(TEST_TOURNAMENT_ID);

            // Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        /// <summary>
        /// Test for Edit method (GET action). Tournament with specified identifier exists. View model of Tournament is returned.
        /// </summary>
        [TestMethod]
        public void EditGetAction_ExistingTournament_TournamentViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestTournament(TEST_TOURNAMENT_ID);
            var expected = MakeTestTournamentViewModel(TEST_TOURNAMENT_ID);
            SetupGet(TEST_TOURNAMENT_ID, testData);

            // Act
            var actual = TestExtensions.GetModel<TournamentViewModel>(this._sut.Edit(TEST_TOURNAMENT_ID));

            // Assert
            TestHelper.AreEqual<TournamentViewModel>(expected, actual, new TournamentViewModelComparer());
        }
        #endregion

        #region EditPostAction
        /// <summary>
        /// Test for Edit method (POST action). Tournament view model is valid and no exception is thrown during editing.
        /// Tournament is updated successfully and user is redirected to the Index page.
        /// </summary>
        [TestMethod]
        public void EditPostAction_ValidTournamentViewModelNoException_TournamentIsUpdated()
        {
            // Arrange
            var testData = MakeTestTournamentViewModel();

            // Act
            var result = this._sut.Edit(testData) as RedirectToRouteResult;

            // Assert
            VerifyEdit(Times.Once());
            VerifyRedirect(INDEX_ACTION_NAME, result);
        }

        /// <summary>
        /// Test for Edit method (POST action). Tournament view model is valid, but exception is thrown during editing.
        /// Tournament view model is returned.
        /// </summary>
        [TestMethod]
        public void EditPostAction_ValidTournamentViewModelWithException_TournamentViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestTournamentViewModel();
            SetupEditThrowsTournamentValidationException();

            // Act
            var result = TestExtensions.GetModel<TournamentViewModel>(this._sut.Edit(testData));

            // Assert
            VerifyEdit(Times.Once());
            Assert.IsNotNull(result, ASSERT_FAIL_VIEW_MODEL_MESSAGE);
        }

        /// <summary>
        /// Test for Edit method (POST action). Tournament view model is not valid.
        /// Tournament is not updated and tournament view model is returned.
        /// </summary>
        [TestMethod]
        public void EditPostAction_InvalidTournamentViewModel_TournamentViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestTournamentViewModel();
            this._sut.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = TestExtensions.GetModel<TournamentViewModel>(this._sut.Edit(testData));

            // Assert
            VerifyEdit(Times.Never());
            Assert.IsNotNull(result, ASSERT_FAIL_VIEW_MODEL_MESSAGE);
        }
        #endregion

        #region DeleteTeamFromTournament
        /// <summary>
        /// Test for Delete team from tournament method (POST action)
        /// </summary>
        [TestMethod]
        public void DeleteTeamFromTournament_TeamExists_TeamDeleted()
        {
            // Arrange
            this._tournamentServiceMock
                .Setup(ts => ts.DeleteTeamFromTournament(It.IsAny<int>(), It.IsAny<int>()));

            // Act
            var jsonResult = this._sut.DeleteTeamFromTournament(TEST_TOURNAMENT_ID, TEST_ID);
            var result = jsonResult.Data as TeamDeleteFromTournamentViewModel;

            // Assert
            Assert.IsTrue(result.HasDeleted);
        }

        /// <summary>
        /// Test for Delete team from tournament method (POST action)
        /// team is not exists
        /// </summary>
        [TestMethod]
        public void DeleteTeamFromTournament_NonExistTeam_TeamIsNotDeleted()
        {
            // Arrange
            this._tournamentServiceMock
                .Setup(ts => ts.DeleteTeamFromTournament(It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new MissingEntityException());

            // Act
            var jsonResult = this._sut.DeleteTeamFromTournament(TEST_TOURNAMENT_ID, TEST_ID);
            var result = jsonResult.Data as TeamDeleteFromTournamentViewModel;

            // Assert
            Assert.IsFalse(result.HasDeleted);
        }
        #endregion

        #region DeleteGetAction
        /// <summary>
        /// Test for Delete method (GET action). Tournament with specified identifier does not exist. HttpNotFoundResult is returned.
        /// </summary>
        [TestMethod]
        public void DeleteGetAction_NonExistentTournament_HttpNotFoundResultIsReturned()
        {
            // Arrange
            SetupGet(TEST_TOURNAMENT_ID, null as Tournament);

            // Act
            var result = this._sut.Delete(TEST_TOURNAMENT_ID);

            // Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        /// <summary>
        /// Test for Delete method (GET action). Tournament with specified identifier exists. View model of Tournament is returned.
        /// </summary>
        [TestMethod]
        public void DeleteGetAction_ExistingTournament_TournamentViewModelIsReturned()
        {
            // Arrange
            var testData = MakeTestTournament(TEST_TOURNAMENT_ID);
            var expected = MakeTestTournamentViewModel(TEST_TOURNAMENT_ID);
            SetupGet(TEST_TOURNAMENT_ID, testData);

            // Act
            var actual = TestExtensions.GetModel<TournamentViewModel>(this._sut.Delete(TEST_TOURNAMENT_ID));

            // Assert
            TestHelper.AreEqual<TournamentViewModel>(expected, actual, new TournamentViewModelComparer());
        }
        #endregion

        #region DeletePostAction
        /// <summary>
        /// Test for DeleteConfirmed method (delete POST action). Tournament with specified identifier does not exist.
        /// HttpNotFoundResult is returned.
        /// </summary>
        [TestMethod]
        public void DeletePostAction_NonExistentTournament_HttpNotFoundResultIsReturned()
        {
            // Arrange
            SetupGet(TEST_TOURNAMENT_ID, null as Tournament);

            // Act
            var result = this._sut.DeleteConfirmed(TEST_TOURNAMENT_ID);

            // Assert
            VerifyDelete(TEST_TOURNAMENT_ID, Times.Never());
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        /// <summary>
        /// Test for DeleteConfirmed method (delete POST action). Tournament with specified identifier exists.
        /// Tournament is deleted successfully and user is redirected to the Index page.
        /// </summary>
        [TestMethod]
        public void DeletePostAction_ExistingTournament_TournamentIsDeleted()
        {
            // Arrange
            var testData = MakeTestTournament(TEST_TOURNAMENT_ID);
            SetupGet(TEST_TOURNAMENT_ID, testData);

            // Act
            var result = this._sut.DeleteConfirmed(TEST_TOURNAMENT_ID) as RedirectToRouteResult;

            // Assert
            VerifyDelete(TEST_TOURNAMENT_ID, Times.Once());
            VerifyRedirect(INDEX_ACTION_NAME, result);
        }
        #endregion

        #region SwapRounds
        /// <summary>
        /// Test for SwapRounds method.
        /// Wrong tournament Id passed. View with error message is returned.
        /// </summary>
        [TestMethod]
        public void SwapRounds_NonExistentTournament_ErrorViewIsReturned()
        {
            // Arrange
            SetupGet(TEST_TOURNAMENT_ID, null as Tournament);

            // Act
            var result = TestExtensions
                            .GetModel<ScheduleViewModel>(this._sut.SwapRounds(
                                                            TEST_TOURNAMENT_ID,
                                                            FIRST_ROUND_NUMBER,
                                                            SECOND_ROUND_NUMBER));

            // Assert
            Assert.IsFalse(_sut.ModelState.IsValid);
            Assert.IsTrue(_sut.ModelState.ContainsKey("LoadError"));
            Assert.IsNull(result);
        }

        /// <summary>
        /// Test for SwapRounds method. Games are exist.
        /// All games are swapped.
        /// </summary>
        [TestMethod]
        public void SwapRounds_ExistentGames_GamesIsSwapped()
        {
            // Arrange
            var tournament = new TournamentScheduleDtoBuilder().Build();

            SetupGetScheduleInfo(TEST_TOURNAMENT_ID, tournament);

            // Act
            var result = this._sut
                .SwapRounds(TEST_TOURNAMENT_ID, FIRST_ROUND_NUMBER, SECOND_ROUND_NUMBER) as RedirectToRouteResult;

            // Assert
            VerifyRedirect("ShowSchedule", result);
        }

        /// <summary>
        /// Test for SwapRounds method. Some game are not exist.
        /// All games are swapped.
        /// </summary>
        [TestMethod]
        public void SwapRounds_NonExistentGames_ErrorViewIsReturned()
        {
            // Arrange
            var tournament = new TournamentScheduleDtoBuilder().Build();
            SetupGetScheduleInfo(TEST_TOURNAMENT_ID, tournament);
            this._gameServiceMock.Setup(tr => tr.SwapRounds(
                                                            TEST_TOURNAMENT_ID,
                                                            FIRST_ROUND_NUMBER,
                                                            SECOND_ROUND_NUMBER))
                                                                   .Throws(new MissingEntityException());

            // Act
            var result = TestExtensions.GetModel<ScheduleViewModel>(this._sut.SwapRounds(
                                                                              TEST_TOURNAMENT_ID,
                                                                              FIRST_ROUND_NUMBER,
                                                                              SECOND_ROUND_NUMBER));

            // Assert
            Assert.IsFalse(_sut.ModelState.IsValid);
            Assert.IsTrue(_sut.ModelState.ContainsKey("LoadError"));
            Assert.IsNull(result);
        }
        #endregion

        #region Private
        private List<Tournament> MakeTestTournaments()
        {
            return new TournamentServiceTestFixture().TestTournaments().Build();
        }

        private List<Team> MakeTestTeams()
        {
            return new TeamServiceTestFixture().TestTeams().Build();
        }

        private Tournament MakeTestTournament(int tournamentId)
        {
            return new TournamentBuilder().WithId(tournamentId).Build();
        }

        private TournamentViewModel MakeTestTournamentViewModel()
        {
            return new TournamentMvcViewModelBuilder().Build();
        }

        private GameViewModel MakeTestGameViewModel()
        {
            return new GameViewModelBuilder().Build();
        }

        private Game MakeTestGame()
        {
            return new GameBuilder().Build();
        }

        private TournamentViewModel MakeTestTournamentViewModel(int tournamentId)
        {
            return new TournamentMvcViewModelBuilder().WithId(tournamentId).Build();
        }

        private List<Tournament> GetTournamentsWithState(List<Tournament> tournaments, TournamentStateEnum state)
        {
            return tournaments.Where(tr => tr.State == state).ToList();
        }

        private void SetupGetActual(List<Tournament> tournaments)
        {
            this._tournamentServiceMock.Setup(tr => tr.GetActual()).Returns(tournaments);
        }

        private void SetupGetFinished(List<Tournament> tournaments)
        {
            this._tournamentServiceMock.Setup(tr => tr.GetFinished()).Returns(tournaments);
        }

        private void SetupGetTournamentTeams(List<Team> teams, int tournamentId)
        {
            this._tournamentServiceMock
                .Setup(tr => tr.GetAllTournamentTeams(tournamentId))
                .Returns(teams);
        }

        private void SetupGetTournamentNumberOfRounds(TournamentScheduleDto tournament, byte numberOfRounds)
        {
            this._tournamentServiceMock
                .Setup(tr => tr.GetNumberOfRounds(tournament))
                .Returns(numberOfRounds);
        }

        private void SetupGet(int tournamentId, Tournament tournament)
        {
            this._tournamentServiceMock.Setup(tr => tr.Get(tournamentId)).Returns(tournament);
        }

        private void SetupGetScheduleInfo(int tournamentId, TournamentScheduleDto tournament)
        {
            this._tournamentServiceMock.Setup(tr => tr.GetTournamentScheduleInfo(tournamentId)).Returns(tournament);
        }

        private void SetupGetGame(int gameId, GameResultDto game)
        {
            this._gameServiceMock.Setup(gs => gs.Get(gameId)).Returns(game);
        }

        private void SetupGetTournamentResults(int tournamentId, List<GameResultDto> expectedGames)
        {
            this._gameServiceMock.Setup(t => t.GetTournamentResults(It.IsAny<int>())).Returns(expectedGames);
        }

        private void SetupCreateThrowsTournamentValidationException()
        {
            this._tournamentServiceMock.Setup(ts => ts.Create(It.IsAny<Tournament>()))
                .Throws(new TournamentValidationException(string.Empty, string.Empty, string.Empty));
        }

        private void SetupEditThrowsTournamentValidationException()
        {
            this._tournamentServiceMock.Setup(ts => ts.Edit(It.IsAny<Tournament>()))
                .Throws(new TournamentValidationException(string.Empty, string.Empty, string.Empty));
        }

        private void SetupControllerContext()
        {
            this._sut.ControllerContext = new ControllerContext(this._httpContextMock.Object, new RouteData(), this._sut);
        }

        private void SetupRequestRawUrl(string rawUrl)
        {
            this._httpRequestMock.Setup(x => x.RawUrl).Returns(rawUrl);
        }

        private void VerifyCreate(Times times)
        {
            this._tournamentServiceMock.Verify(ts => ts.Create(It.IsAny<Tournament>()), times);
        }

        private void VerifyEdit(Times times)
        {
            this._tournamentServiceMock.Verify(ts => ts.Edit(It.IsAny<Tournament>()), times);
        }

        private void VerifyDelete(int tournamentId, Times times)
        {
            this._tournamentServiceMock.Verify(ts => ts.Delete(tournamentId), times);
        }

        private void VerifyRedirect(string actionName, RedirectToRouteResult result)
        {
            Assert.AreEqual(actionName, result.RouteValues[ROUTE_VALUES_KEY]);
        }

        private void VerifyCreateGame(Times times)
        {
            this._gameServiceMock.Verify(gs => gs.Create(It.IsAny<Game>()), times);
        }

        private void VerifySwapRounds(int tournamentId, byte firstRoundNumber, byte secondRoundNumber)
        {
            this._gameServiceMock.Verify(gs => gs.SwapRounds(tournamentId, firstRoundNumber, secondRoundNumber));
        }

        private void VerifyEditGame(Times times)
        {
            this._gameServiceMock.Verify(gs => gs.Edit(It.IsAny<Game>()), times);
        }

        private void VerifyInvalidModelState(string expectedKey, GameViewModel gameViewModel)
        {
            Assert.IsFalse(_sut.ModelState.IsValid);
            Assert.IsTrue(_sut.ModelState.ContainsKey(expectedKey));
            Assert.IsNull(gameViewModel);
        }

        private void VerifyGetAllowedOperations(Times times)
        {
            _authServiceMock.Verify(tr => tr.GetAllowedOperations(It.IsAny<List<AuthOperation>>()), times);
        }

        private void VerifyGetAllowedOperations(List<AuthOperation> allowedOperations, Times times)
        {
            _authServiceMock.Verify(tr => tr.GetAllowedOperations(allowedOperations), times);
        }

        private void AssertEqual(GameViewModel x, GameViewModel y)
        {
            Assert.AreEqual(x.Id, y.Id, "Actual game Id doesn't match expected");
            Assert.AreEqual(x.TournamentId, y.TournamentId, "Actual TournamentId doesn't match expected");
            Assert.AreEqual(x.HomeTeamId, y.HomeTeamId, "Actual HomeTeamId doesn't match expected");
            Assert.AreEqual(x.AwayTeamId, y.AwayTeamId, "Actual AwayTeamId doesn't match expected");
            Assert.AreEqual(x.Round, y.Round, "Actual Round number doesn't match expected");
            Assert.AreEqual(x.GameDate, y.GameDate, "Actual GameDate doesn't match expected");
            Assert.AreEqual(x.GameNumber, y.GameNumber, "Actual GameNumber doesn't match expected");

            Assert.IsTrue(
                x.Teams != null &&
                y.Teams != null &&
                x.Teams.Select(
                    team => new { Text = team.Text, Value = team.Value }).SequenceEqual(
                    y.Teams.Select(team => new { Text = team.Text, Value = team.Value })),
                "Actual Teams list doesn't match expected");

            Assert.IsTrue(
                          x.Rounds != null &&
                          y.Rounds != null &&
                         (x.Rounds.Items as IEnumerable<int>).SequenceEqual(
                          y.Rounds.Items as IEnumerable<int>),
                          "Actual Rounds list doesn't match expected");
        }
        #endregion
    }
}
