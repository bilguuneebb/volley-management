﻿namespace VolleyManagement.WebApi.Mappers
{
    using System;
    using VolleyManagement.Domain.Tournaments;
    using VolleyManagement.WebApi.ViewModels.Tournaments;

    /// <summary>
    /// Maps view models to domain models
    /// </summary>
    public static class ViewModelToDomain
    {
        /// <summary>
        /// Maps Tournament.
        /// </summary>
        /// <param name="tournamentViewModel">Tournament view model</param>
        /// <returns>Tournament Domain model</returns>
        public static Tournament Map(TournamentViewModel tournamentViewModel)
        {
            var tournament = new Tournament();
            tournament.Id = tournamentViewModel.Id;
            tournament.Name = tournamentViewModel.Name;
            tournament.Description = tournamentViewModel.Description;
            tournament.Season = tournamentViewModel.Season;
            int schemeValue;
            bool parsed = int.TryParse(tournamentViewModel.Scheme, out schemeValue);
            if (parsed)
            {
                switch (schemeValue)
                {
                    case 1: tournament.Scheme = TournamentSchemeEnum.One;
                        break;
                    case 2: tournament.Scheme = TournamentSchemeEnum.Two;
                        break;
                    case 3: tournament.Scheme = TournamentSchemeEnum.TwoAndHalf;
                        break;
                }
            }

            tournament.RegulationsLink = tournamentViewModel.RegulationsLink;
            return tournament;
        }
    }
}