﻿namespace VolleyManagement.UI.Areas.Mvc.ViewModels.GameReports
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a view model for tournament's standings.
    /// </summary>
    public class StandingsViewModel
    {
        /// <summary>
        /// Gets or sets the tournament's identifier.
        /// </summary>
        public int TournamentId { get; set; }

        /// <summary>
        /// Gets or sets the tournament's name.
        /// </summary>
        public string TournamentName { get; set; }

        /// <summary>
        /// Gets or sets the collection of entries in tournament's standings.
        /// </summary>
        public List<StandingsEntryViewModel> Entries { get; set; }
    }
}