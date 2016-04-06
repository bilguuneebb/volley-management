﻿namespace VolleyManagement.UI
{
    using System.Web.Optimization;

    /// <summary>
    /// Bundle configuration class
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// Registers bundles
        /// </summary>
        /// <param name="bundles">Collection of bundles</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css")
                   .Include(
                       "~/Content/bootstrap.min.css",
                       "~/Content/Site.css",
                       "~/Content/themes/base/all.css"));

            //// NOTE: Bundles {version} parameters doesn't work correctly with min files

            bundles.Add(new ScriptBundle("~/bundles/bootstrapscripts")
                        .Include(
                            "~/Scripts/jquery-{version}.js",
                            "~/Scripts/bootstrap.min.js",
                            "~/Scripts/VmScripts/VmScripts.js"));

            bundles.Add(new ScriptBundle("~/bundles/vmscripts")
                        .Include("~/Scripts/VmScripts/VmScripts.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui")
                        .Include("~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryvalidation")
                        .Include(
                            "~/Scripts/jquery.validate.min.js",
                            "~/Scripts/jquery.validate.unobtrusive.min.js")); 

            bundles.Add(new ScriptBundle("~/bundles/useractionscripts")
                .Include("~/Scripts/UserActions.js"));

            RegisterTeamScripts(bundles);
            RegisterTornamentScripts(bundles);
        }

        private static void RegisterTeamScripts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/teameditscripts")
                .Include("~/Scripts/VmScripts/TeamOperations/CreateEdit.js"));

            bundles.Add(new ScriptBundle("~/bundles/teamlistscripts")
                .Include("~/Scripts/VmScripts/TeamOperations/List.js"));
        }

        private static void RegisterTornamentScripts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/tournamentteamsscripts")
                        .Include("~/Scripts/VmScripts/TournamentOperations/AddTeams.js"));
        }
    }
}