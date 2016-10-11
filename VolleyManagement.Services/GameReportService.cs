﻿namespace VolleyManagement.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Data.Queries.Tournament;
    using Domain.TournamentsAggregate;
    using VolleyManagement.Contracts;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Queries.GameResult;
    using VolleyManagement.Data.Queries.Team;
    using VolleyManagement.Domain.GameReportsAggregate;
    using VolleyManagement.Domain.GamesAggregate;
    using VolleyManagement.Domain.TeamsAggregate;

    /// <summary>
    /// Represents an implementation of IGameReportService contract.
    /// </summary>
    public class GameReportService : IGameReportService
    {
        #region Queries

        private readonly IQuery<List<GameResultDto>, TournamentGameResultsCriteria> _tournamentGameResultsQuery;
        private readonly IQuery<List<Team>, FindByTournamentIdCriteria> _tournamentTeamsQuery;
        private readonly IQuery<TournamentScheduleDto, TournamentScheduleInfoCriteria> _tournamentScheduleDtoByIdQuery;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GameReportService"/> class.
        /// </summary>
        /// <param name="tournamentGameResultsQuery">Query for getting tournament's game results.</param>
        /// <param name="tournamentTeamsQuery">Query for getting tournament's game teams.</param>
        /// <param name="tournamentScheduleDtoByIdQuery">Get tournament data transfer object query.</param>
        public GameReportService(
            IQuery<List<GameResultDto>, TournamentGameResultsCriteria> tournamentGameResultsQuery,
            IQuery<List<Team>, FindByTournamentIdCriteria> tournamentTeamsQuery,
            IQuery<TournamentScheduleDto, TournamentScheduleInfoCriteria> tournamentScheduleDtoByIdQuery)
        {
            _tournamentGameResultsQuery = tournamentGameResultsQuery;
            _tournamentTeamsQuery = tournamentTeamsQuery;
            _tournamentScheduleDtoByIdQuery = tournamentScheduleDtoByIdQuery;
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Gets standings of the tournament specified by identifier.
        /// </summary>
        /// <param name="tournamentId">Identifier of the tournament.</param>
        /// <returns>Standings of the tournament with specified identifier.</returns>
        public List<StandingsEntry> GetStandings(int tournamentId)
        {
            var gameResults = _tournamentGameResultsQuery.Execute(new TournamentGameResultsCriteria { TournamentId = tournamentId });
            var tournamentTeams = _tournamentTeamsQuery.Execute(new FindByTournamentIdCriteria { TournamentId = tournamentId });

            var standings = CreateEntriesForTeams(tournamentTeams);

            foreach (var gameResult in gameResults)
            {
                if (gameResult.AwayTeamId != null)
                {
                    StandingsEntry standingsHomeTeamEntry = standings.Single(se => se.TeamId == gameResult.HomeTeamId);
                    StandingsEntry standingsAwayTeamEntry = standings.Single(se => se.TeamId == gameResult.AwayTeamId);

                    CalculateGamesStatistics(standingsHomeTeamEntry, standingsAwayTeamEntry, gameResult);
                }
            }

            CalculateSetsStatistics(gameResults, standings);
            CalculateBallsStatistics(gameResults, standings);

            // order all standings entries by points, then by sets ratio and then by balls ratio in descending order
            return standings.OrderByDescending(ts => ts.Points)
                .ThenByDescending(ts => ts.SetsRatio)
                .ThenByDescending(ts => ts.BallsRatio)
                .ThenBy(ts => ts.TeamName)
                .ToList();
        }

        /// <summary>
        /// Gets pivot standings of the tournament specified by identifier.
        /// </summary>
        /// <param name="tournamentId">Identifier of the tournament.</param>
        /// <returns>Pivot standings of the tournament with specified identifier.</returns>
        public PivotStandingsDto GetPivotStandings(int tournamentId)
        {
            var gameResults = _tournamentGameResultsQuery.Execute(new TournamentGameResultsCriteria { TournamentId = tournamentId });
            var tournamentTeams = _tournamentTeamsQuery.Execute(new FindByTournamentIdCriteria { TournamentId = tournamentId });

            var teamStandings = CreateTeamStandings(tournamentTeams, gameResults);

            var shortGameResults = gameResults.Where(g => g.AwayTeamId != null).Select(
                g => new ShortGameResultDto
                {
                    HomeTeamId = g.HomeTeamId.Value,
                    AwayTeamId = g.AwayTeamId.Value,
                    HomeSetsScore = g.HomeSetsScore,
                    AwaySetsScore = g.AwaySetsScore,
                    IsTechnicalDefeat = g.IsTechnicalDefeat
                }).ToList();

            return new PivotStandingsDto(teamStandings, shortGameResults);
        }

        /// <summary>
        /// Check if the standing available in the tournament
        /// </summary>
        /// <param name="tournamentId">Identifier of the tournament.</param>
        /// <returns>True or false</returns>
        public bool IsStandingAvailable(int tournamentId)
        {
            var tournamentInfo = _tournamentScheduleDtoByIdQuery
                .Execute(new TournamentScheduleInfoCriteria { TournamentId = tournamentId });

            return tournamentInfo.Scheme != TournamentSchemeEnum.PlayOff;
        }

        #endregion

        #region Private methods

        private List<StandingsEntry> CreateEntriesForTeams(List<Team> tournamentTeams)
        {
            var entries = new List<StandingsEntry>();
            var teams = tournamentTeams.Select(gr => new { Id = gr.Id, Name = gr.Name });

            foreach (var team in teams)
            {
                entries.Add(new StandingsEntry
                {
                    TeamId = team.Id,
                    TeamName = team.Name
                });
            }

            return entries;
        }

        private List<TeamStandingsDto> CreateTeamStandings(List<Team> tournamentTeams, List<GameResultDto> gameResults)
        {
            var teamsStandings = tournamentTeams.Select(
                t => new TeamStandingsDto
                {
                    TeamId = t.Id,
                    TeamName = t.Name,
                    Points = 0,
                    SetsRatio = CalculateSetsRatio(GetTeamWonSets(t.Id, gameResults), GetTeamLostSets(t.Id, gameResults)),
                    BallsRatio = CalculateBallsRatio(GetTeamWonBalls(t.Id, gameResults), GetTeamLostBalls(t.Id, gameResults))
                })
                .ToList();

            foreach (var game in gameResults)
            {
                if (game.AwayTeamId != null)
                {
                    var homeTeam = new StandingsEntry { TeamId = game.HomeTeamId.Value };
                    var awayTeam = new StandingsEntry { TeamId = game.AwayTeamId.Value };

                    CalculateGamesStatistics(homeTeam, awayTeam, game);

                    teamsStandings.Single(t => t.TeamId == homeTeam.TeamId).Points += homeTeam.Points;
                    teamsStandings.Single(t => t.TeamId == awayTeam.TeamId).Points += awayTeam.Points;
                }
            }

            return teamsStandings
                 .OrderByDescending(t => t.Points)
                 .ThenByDescending(t => t.SetsRatio)
                 .ThenByDescending(ts => ts.BallsRatio)
                 .ThenBy(t => t.TeamName)
                 .ToList();
        }

        private void CalculateGamesStatistics(StandingsEntry homeTeamEntry, StandingsEntry awayTeamEntry, GameResultDto gameResult)
        {
            if (HasTeamPlayedGames(homeTeamEntry))
            {
                SetDataFromNullToZero(homeTeamEntry);
            }

            if (HasTeamPlayedGames(awayTeamEntry))
            {
                SetDataFromNullToZero(awayTeamEntry);
            }

            homeTeamEntry.GamesTotal++;
            awayTeamEntry.GamesTotal++;

            switch (gameResult.HomeSetsScore - gameResult.AwaySetsScore)
            {
                case 3: // sets score - 3:0
                    homeTeamEntry.Points += 3;
                    homeTeamEntry.GamesWon++;
                    homeTeamEntry.GamesWithScoreThreeNil++;
                    awayTeamEntry.GamesLost++;
                    awayTeamEntry.GamesWithScoreNilThree++;
                    break;
                case 2: // sets score - 3:1
                    homeTeamEntry.Points += 3;
                    homeTeamEntry.GamesWon++;
                    homeTeamEntry.GamesWithScoreThreeOne++;
                    awayTeamEntry.GamesLost++;
                    awayTeamEntry.GamesWithScoreOneThree++;
                    break;
                case 1: // sets score - 3:2
                    homeTeamEntry.Points += 2;
                    homeTeamEntry.GamesWon++;
                    homeTeamEntry.GamesWithScoreThreeTwo++;
                    awayTeamEntry.Points++;
                    awayTeamEntry.GamesLost++;
                    awayTeamEntry.GamesWithScoreTwoThree++;
                    break;
                case -1: // sets score - 2:3
                    homeTeamEntry.Points++;
                    homeTeamEntry.GamesLost++;
                    homeTeamEntry.GamesWithScoreTwoThree++;
                    awayTeamEntry.Points += 2;
                    awayTeamEntry.GamesWon++;
                    awayTeamEntry.GamesWithScoreThreeTwo++;
                    break;
                case -2: // sets score - 1:3
                    homeTeamEntry.GamesLost++;
                    homeTeamEntry.GamesWithScoreOneThree++;
                    awayTeamEntry.Points += 3;
                    awayTeamEntry.GamesWon++;
                    awayTeamEntry.GamesWithScoreThreeOne++;
                    break;
                case -3: // sets score - 0:3
                    homeTeamEntry.GamesLost++;
                    homeTeamEntry.GamesWithScoreNilThree++;
                    awayTeamEntry.Points += 3;
                    awayTeamEntry.GamesWon++;
                    awayTeamEntry.GamesWithScoreThreeNil++;
                    break;
            }
        }

        private void CalculateSetsStatistics(List<GameResultDto> gameResults, List<StandingsEntry> standings)
        {
            foreach (var item in standings)
            {
                item.SetsWon = GetTeamWonSets(item.TeamId, gameResults);
                item.SetsLost = GetTeamLostSets(item.TeamId, gameResults);
                item.SetsRatio = CalculateSetsRatio(item.SetsWon.Value, item.SetsLost.Value);
            }
        }

        private void CalculateBallsStatistics(List<GameResultDto> gameResults, List<StandingsEntry> standings)
        {
            foreach (var item in standings)
            {
                item.BallsWon = GetTeamWonBalls(item.TeamId, gameResults);
                item.BallsLost = GetTeamLostBalls(item.TeamId, gameResults);
                item.BallsRatio = CalculateBallsRatio(item.BallsWon.Value, item.BallsLost.Value);
            }
        }

        private int GetTeamWonSets(int teamId, List<GameResultDto> games)
        {
            int result = 0;
            result += games.Where(g => g.HomeTeamId == teamId).Select(g => (int)g.HomeSetsScore).Sum();
            result += games.Where(g => g.AwayTeamId == teamId).Select(g => (int)g.AwaySetsScore).Sum();
            return result;
        }

        private int GetTeamLostSets(int teamId, List<GameResultDto> games)
        {
            int result = 0;
            result += games.Where(g => g.HomeTeamId == teamId).Select(g => (int)g.AwaySetsScore).Sum();
            result += games.Where(g => g.AwayTeamId == teamId).Select(g => (int)g.HomeSetsScore).Sum();
            return result;
        }

        private float? CalculateSetsRatio(int gamesWon, int gamesLost)
        {
            var result = (float)gamesWon / gamesLost;
            if (float.IsNaN(result))
            {
                return null;
            }

            return result;
        }

        private int GetTeamWonBalls(int teamId, List<GameResultDto> games)
        {
            var results = games.Where(g => g.HomeTeamId == teamId).ToList();
            int wonBalls = results.Sum(
                item => item.HomeSet1Score
                + item.HomeSet2Score
                + item.HomeSet3Score
                + item.HomeSet4Score
                + item.HomeSet5Score);
            results = games.Where(g => g.AwayTeamId == teamId).ToList();
            wonBalls += results.Sum(
                item => item.AwaySet1Score
                + item.AwaySet2Score
                + item.AwaySet3Score
                + item.AwaySet4Score
                + item.AwaySet5Score);
            return wonBalls;
        }

        private int GetTeamLostBalls(int teamId, List<GameResultDto> games)
        {
            var results = games.Where(g => g.HomeTeamId == teamId).ToList();
            int lostBalls = results.Sum(
                item => item.AwaySet1Score
                + item.AwaySet2Score
                + item.AwaySet3Score
                + item.AwaySet4Score
                + item.AwaySet5Score);
            results = games.Where(g => g.AwayTeamId == teamId).ToList();
            lostBalls += results.Sum(
                item => item.HomeSet1Score
                + item.HomeSet2Score
                + item.HomeSet3Score
                + item.HomeSet4Score
                + item.HomeSet5Score);
            return lostBalls;
        }

        private float? CalculateBallsRatio(int ballsWon, int ballsLost)
        {
            var result = (float)ballsWon / ballsLost;
            if (float.IsNaN(result))
            {
                return null;
            }

            return result;
        }

        private void SetDataFromNullToZero(StandingsEntry entry)
        {
            entry.GamesWon = 0;
            entry.GamesLost = 0;
            entry.GamesWithScoreThreeNil = 0;
            entry.GamesWithScoreThreeOne = 0;
            entry.GamesWithScoreThreeTwo = 0;
            entry.GamesWithScoreTwoThree = 0;
            entry.GamesWithScoreOneThree = 0;
            entry.GamesWithScoreNilThree = 0;
            entry.BallsWon = 0;
            entry.BallsLost = 0;
            entry.SetsWon = 0;
            entry.SetsLost = 0;
        }

        private bool HasTeamPlayedGames(StandingsEntry entry)
        {
            return entry.GamesTotal == 0;
        }
        #endregion
    }
}
