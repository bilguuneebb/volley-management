﻿namespace VolleyManagement.Domain.Tournaments
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Tournament domain class.
    /// </summary>
    public class Tournament
    {
        /// <summary>
        /// Gets or sets a value indicating where Id.
        /// </summary>
        /// <value>Id of tournament.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Name.
        /// </summary>
        /// <value>Name of tournament.</value>
        [Required(ErrorMessageResourceName = "TournamentNameRequired",
            ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "TournamentName", ResourceType = typeof(Resources))]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Description.
        /// </summary>
        /// <value>Description of tournament.</value>
        [Display(Name = "TournamentDescription", ResourceType = typeof(Resources))]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Season.
        /// </summary>
        /// <value>Season of tournament.</value>
        [Required(ErrorMessageResourceName = "TournamentSeasonRequired",
            ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "TournamentSeason", ResourceType = typeof(Resources))]
        public string Season { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Scheme.
        /// </summary>
        /// <value>Scheme of tournament.</value>
        [Required(ErrorMessageResourceName = "TournamentSchemeRequired",
            ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "TournamentScheme", ResourceType = typeof(Resources))]
        public string Scheme { get; set; }

        /// <summary>
        /// Gets or sets a value indicating regulations of tournament.
        /// </summary>
        /// <value>regulations of tournament.</value>
        [Display(Name = "TournamentRegulationsLink", ResourceType = typeof(Resources))]
        public string RegulationsLink { get; set; }
    }
}
