﻿namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using VolleyManagement.Contracts;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.ContributorsTeam;

    /// <summary>
    /// Defines contributor team controller
    /// </summary>
    public class ContributorsTeamController : Controller
    {
        /// <summary>
        /// Holds ContributorTeamService instance
        /// </summary>
        private readonly IContributorTeamService _contributorTeamService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContributorsTeamController"/> class
        /// </summary>
        /// <param name="contributorTeamService">Instance of the class that implements
        /// IContributorTeamService.</param>
        public ContributorsTeamController(IContributorTeamService contributorTeamService)
        {
            _contributorTeamService = contributorTeamService;
        }

        /// <summary>
        /// Gets all contributors teams from ContributorService
        /// </summary>
        /// <returns>View with collection of contributors teams</returns>
        [Authorize]
        public ActionResult Index()
        {
            try
            {
                var contributors = this._contributorTeamService.Get().ToList();
                var contributorsTeamView = new List<ContributorsTeamViewModel>();

                foreach (var item in contributors)
                {
                    contributorsTeamView.Add(ContributorsTeamViewModel.Map(item));
                }

               return View(contributorsTeamView);
            }
            catch (Exception)
            {
                return this.HttpNotFound();
            }
        }
    }
}