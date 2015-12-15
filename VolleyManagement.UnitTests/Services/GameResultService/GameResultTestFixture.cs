﻿namespace VolleyManagement.UnitTests.Services.GameResultService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.GameResultsAggregate;

    /// <summary>
    /// Generates test data for <see cref="GameResult"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class GameResultTestFixture
    {
        private List<GameResult> _gameResults = new List<GameResult>();

        /// <summary>
        /// Generates <see cref="GameResult"/> objects filled with test data.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultTestFixture"/>.</returns>
        public GameResultTestFixture TestGameResults()
        {
            _gameResults.Add(new GameResult
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                SetsScore = new Score(3, 2),
                IsTechnicalDefeat = false,
                SetScores = new List<Score>
                {
                    new Score(25, 20),
                    new Score(24, 26),
                    new Score(28, 30),
                    new Score(25, 22),
                    new Score(27, 25)
                }
            });
            _gameResults.Add(new GameResult
            {
                Id = 2,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 3,
                SetsScore = new Score(3, 1),
                IsTechnicalDefeat = false,
                SetScores = new List<Score>
                {
                    new Score(26, 28),
                    new Score(25, 15),
                    new Score(25, 21),
                    new Score(29, 27),
                    new Score(0, 0)
                }
            });
            _gameResults.Add(new GameResult
            {
                Id = 3,
                TournamentId = 1,
                HomeTeamId = 2,
                AwayTeamId = 3,
                SetsScore = new Score(0, 3),
                IsTechnicalDefeat = true,
                SetScores = new List<Score>
                {
                    new Score(0, 25),
                    new Score(0, 25),
                    new Score(0, 25),
                    new Score(0, 0),
                    new Score(0, 0)
                }
            });

            return this;
        }

        /// <summary>
        /// Adds <see cref="GameResult"/> object to collection.
        /// </summary>
        /// <param name="newGameResult"><see cref="GameResult"/> object to add.</param>
        /// <returns>Instance of <see cref="GameResultTestFixture"/>.</returns>
        public GameResultTestFixture Add(GameResult newGameResult)
        {
            _gameResults.Add(newGameResult);
            return this;
        }

        /// <summary>
        /// Adds game results with all possible scores to collection of <see cref="GameResult"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultTestFixture"/>.</returns>
        public GameResultTestFixture WithAllPossibleScores()
        {
            _gameResults.Clear();
            _gameResults.Add(new GameResult
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                SetsScore = new Score(3, 0),
                IsTechnicalDefeat = false,
                SetScores = new List<Score>
                {
                    new Score(25, 15),
                    new Score(25, 16),
                    new Score(25, 19),
                    new Score(0, 0),
                    new Score(0, 0)
                }
            });
            _gameResults.Add(new GameResult
            {
                Id = 2,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 3,
                SetsScore = new Score(3, 1),
                IsTechnicalDefeat = false,
                SetScores = new List<Score>
                {
                    new Score(24, 26),
                    new Score(25, 19),
                    new Score(25, 18),
                    new Score(25, 23),
                    new Score(0, 0)
                }
            });
            _gameResults.Add(new GameResult
            {
                Id = 3,
                TournamentId = 1,
                HomeTeamId = 2,
                AwayTeamId = 3,
                SetsScore = new Score(3, 2),
                IsTechnicalDefeat = false,
                SetScores = new List<Score>
                {
                    new Score(18, 25),
                    new Score(25, 10),
                    new Score(22, 25),
                    new Score(25, 15),
                    new Score(25, 12)
                }
            });
            _gameResults.Add(new GameResult
            {
                Id = 4,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                SetsScore = new Score(2, 3),
                IsTechnicalDefeat = false,
                SetScores = new List<Score>
                {
                    new Score(25, 22),
                    new Score(26, 24),
                    new Score(23, 25),
                    new Score(17, 25),
                    new Score(13, 25)
                }
            });
            _gameResults.Add(new GameResult
            {
                Id = 5,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 3,
                SetsScore = new Score(1, 3),
                IsTechnicalDefeat = false,
                SetScores = new List<Score>
                {
                    new Score(24, 26),
                    new Score(25, 22),
                    new Score(23, 25),
                    new Score(23, 25),
                    new Score(0, 0)
                }
            });
            _gameResults.Add(new GameResult
            {
                Id = 6,
                TournamentId = 1,
                HomeTeamId = 2,
                AwayTeamId = 3,
                SetsScore = new Score(0, 3),
                IsTechnicalDefeat = false,
                SetScores = new List<Score>
                {
                    new Score(14, 25),
                    new Score(27, 39),
                    new Score(22, 25),
                    new Score(0, 0),
                    new Score(0, 0)
                }
            });

            return this;
        }

        /// <summary>
        /// Adds game result with zero lost sets for the home team to collection of <see cref="GameResult"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultTestFixture"/>.</returns>
        public GameResultTestFixture WithNoLostSetsForHomeTeam()
        {
            _gameResults.Clear();
            _gameResults.Add(new GameResult
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                SetsScore = new Score(3, 0),
                IsTechnicalDefeat = false,
                SetScores = new List<Score>
                {
                    new Score(25, 20),
                    new Score(24, 26),
                    new Score(28, 30),
                    new Score(0, 0),
                    new Score(0, 0)
                }
            });

            return this;
        }

        /// <summary>
        /// Adds game result with zero lost sets for the away team to collection of <see cref="GameResult"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultTestFixture"/>.</returns>
        public GameResultTestFixture WithNoLostSetsForAwayTeam()
        {
            _gameResults.Clear();
            _gameResults.Add(new GameResult
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                SetsScore = new Score(0, 3),
                IsTechnicalDefeat = false,
                SetScores = new List<Score>
                {
                    new Score(14, 25),
                    new Score(27, 39),
                    new Score(22, 25),
                    new Score(0, 0),
                    new Score(0, 0)
                }
            });

            return this;
        }

        /// <summary>
        /// Adds game result with zero lost balls for the home team to collection of <see cref="GameResult"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultTestFixture"/>.</returns>
        public GameResultTestFixture WithNoLostBallsForHomeTeam()
        {
            _gameResults.Clear();
            _gameResults.Add(new GameResult
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                SetsScore = new Score(3, 0),
                IsTechnicalDefeat = true,
                SetScores = new List<Score>
                {
                    new Score(25, 0),
                    new Score(25, 0),
                    new Score(25, 0),
                    new Score(0, 0),
                    new Score(0, 0)
                }
            });

            return this;
        }

        /// <summary>
        /// Adds game result with zero lost balls for the away team to collection of <see cref="GameResult"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultTestFixture"/>.</returns>
        public GameResultTestFixture WithNoLostBallsForAwayTeam()
        {
            _gameResults.Clear();
            _gameResults.Add(new GameResult
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                SetsScore = new Score(0, 3),
                IsTechnicalDefeat = true,
                SetScores = new List<Score>
                {
                    new Score(0, 25),
                    new Score(0, 25),
                    new Score(0, 25),
                    new Score(0, 0),
                    new Score(0, 0)
                }
            });

            return this;
        }

        /// <summary>
        /// Adds game results which result in repetitive points for the teams to collection of <see cref="GameResult"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultTestFixture"/>.</returns>
        public GameResultTestFixture WithResultsForRepetitivePoints()
        {
            _gameResults.Add(new GameResult
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                SetsScore = new Score(3, 1),
                IsTechnicalDefeat = false,
                SetScores = new List<Score>
                {
                    new Score(25, 15),
                    new Score(25, 16),
                    new Score(25, 27),
                    new Score(25, 19),
                    new Score(0, 0)
                }
            });
            _gameResults.Add(new GameResult
            {
                Id = 2,
                TournamentId = 1,
                HomeTeamId = 2,
                AwayTeamId = 1,
                SetsScore = new Score(3, 0),
                IsTechnicalDefeat = false,
                SetScores = new List<Score>
                {
                    new Score(25, 12),
                    new Score(25, 18),
                    new Score(25, 21),
                    new Score(0, 0),
                    new Score(0, 0)
                }
            });

            return this;
        }

        /// <summary>
        /// Adds game results which result in repetitive points and sets ratio for the teams
        /// to collection of <see cref="GameResult"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultTestFixture"/>.</returns>
        public GameResultTestFixture WithResultsForRepetitivePointsAndSetsRatio()
        {
            _gameResults.Add(new GameResult
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                SetsScore = new Score(3, 0),
                IsTechnicalDefeat = false,
                SetScores = new List<Score>
                {
                    new Score(25, 15),
                    new Score(25, 16),
                    new Score(25, 19),
                    new Score(0, 0),
                    new Score(0, 0)
                }
            });
            _gameResults.Add(new GameResult
            {
                Id = 2,
                TournamentId = 1,
                HomeTeamId = 2,
                AwayTeamId = 1,
                SetsScore = new Score(3, 0),
                IsTechnicalDefeat = false,
                SetScores = new List<Score>
                {
                    new Score(25, 12),
                    new Score(25, 10),
                    new Score(25, 20),
                    new Score(0, 0),
                    new Score(0, 0)
                }
            });

            return this;
        }

        /// <summary>
        /// Builds instance of <see cref="GameResultTestFixture"/>.
        /// </summary>
        /// <returns>Collection of <see cref="GameResult"/> objects filled with test data.</returns>
        public List<GameResult> Build()
        {
            return _gameResults;
        }
    }
}
