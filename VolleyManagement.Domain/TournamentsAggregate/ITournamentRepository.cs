﻿namespace VolleyManagement.Domain.TournamentsAggregate
{
    using Data.Contracts;

    /// <summary>
    /// Defines specific contract for TournamentRepository
    /// </summary>
    public interface ITournamentRepository : IGenericRepository<Tournament>
    {
        /// <summary>
        /// Adds selected teams to tournament
        /// </summary>
        /// <param name="teamId">Teams that will be added to tournament</param>
        /// <param name="tournamentId">Tournament to assign to it necessary team</param>
        /// <param name="groupId">Groups of tournament to assign to team</param>
        /// <param name="divisionId">Division to assign group</param>
        void AddTeamToTournament(int teamId, int tournamentId, int groupId, int divisionId);

        /// <summary>
        /// Removes team from tournament
        /// </summary>
        /// <param name="teamId">Team to remove</param>
        /// <param name="tournamentId">Tournament to un assign team</param>
        void RemoveTeamFromTournament(int teamId, int tournamentId);
    }
}
