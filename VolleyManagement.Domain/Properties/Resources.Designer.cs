﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VolleyManagement.Domain.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("VolleyManagement.Domain.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The number of divisions in the tournament must be between {0} and {1}.
        /// </summary>
        public static string DivisionCountOutOfRange {
            get {
                return ResourceManager.GetString("DivisionCountOutOfRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Divisions can not have the same name.
        /// </summary>
        public static string DivisionNamesNotUnique {
            get {
                return ResourceManager.GetString("DivisionNamesNotUnique", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to In the set, the number of points for one team must be at least {0} and the points difference should be at least {1}. If the score exceeds {0}, the points difference must be equal to {1}. In case of a technical defeat score of an optional game should be {2}: {3}.
        /// </summary>
        public static string GameResultOptionalSetScores {
            get {
                return ResourceManager.GetString("GameResultOptionalSetScores", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enter a score of the previous optional game.
        /// </summary>
        public static string GameResultPreviousOptionalSetUnplayed {
            get {
                return ResourceManager.GetString("GameResultPreviousOptionalSetUnplayed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to In the set, the number of points for one team must be at least {0} and the points difference should be at least {1}. If the score exceeds {0}, the points difference must be equal to {1}. In case of a technical defeat score of an optional game should be {2}: {3} or {3}: {2}.
        /// </summary>
        public static string GameResultRequiredSetScores {
            get {
                return ResourceManager.GetString("GameResultRequiredSetScores", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The team can not play with itself.
        /// </summary>
        public static string GameResultSameTeam {
            get {
                return ResourceManager.GetString("GameResultSameTeam", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The set scores are listed in the wrong order.
        /// </summary>
        public static string GameResultSetScoresNotOrdered {
            get {
                return ResourceManager.GetString("GameResultSetScoresNotOrdered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The set score can be one of the following: 3:0, 3:1, 3:2, 2:3, 1:3, 0:3. In case of a technical defeat set score must be {0}:{1} or {1}:{0}.
        /// </summary>
        public static string GameResultSetsScoreInvalid {
            get {
                return ResourceManager.GetString("GameResultSetsScoreInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Game score does not match set scores.
        /// </summary>
        public static string GameResultSetsScoreNoMatchSetScores {
            get {
                return ResourceManager.GetString("GameResultSetsScoreNoMatchSetScores", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An amount of groups in division should be between {0} and {1}.
        /// </summary>
        public static string GroupCountOutOfRange {
            get {
                return ResourceManager.GetString("GroupCountOutOfRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Group names should be unique.
        /// </summary>
        public static string GroupNamesNotUnique {
            get {
                return ResourceManager.GetString("GroupNamesNotUnique", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to End of the transfer period should be before the end of the tournament.
        /// </summary>
        public static string InvalidTransferEndpoint {
            get {
                return ResourceManager.GetString("InvalidTransferEndpoint", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Start of the applying period must be later than today.
        /// </summary>
        public static string LateRegistrationDates {
            get {
                return ResourceManager.GetString("LateRegistrationDates", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Team with such name already exists in tournament.
        /// </summary>
        public static string TeamNameInTournamentNotUnique {
            get {
                return ResourceManager.GetString("TeamNameInTournamentNotUnique", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tournament name should be unique.
        /// </summary>
        public static string TournamentNameMustBeUnique {
            get {
                return ResourceManager.GetString("TournamentNameMustBeUnique", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to If there is a transfer, it&apos;s necessary to specify the end date of the transfer period.
        /// </summary>
        public static string TransferEndMissing {
            get {
                return ResourceManager.GetString("TransferEndMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to If there is a transfer, it&apos;s necessary to specify the start date of the transfer period.
        /// </summary>
        public static string TransferStartMissing {
            get {
                return ResourceManager.GetString("TransferStartMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tournament name should be unique.
        /// </summary>
        public static string UniqueNameMessage {
            get {
                return ResourceManager.GetString("UniqueNameMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Пожалуйста, введите название дивизиона правильно.
        /// </summary>
        public static string ValidationDivisionName {
            get {
                return ResourceManager.GetString("ValidationDivisionName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Пожалуйста, введите название группы правильно.
        /// </summary>
        public static string ValidationGroupName {
            get {
                return ResourceManager.GetString("ValidationGroupName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Пожалуйста, введите дату рождения верно.
        /// </summary>
        public static string ValidationPlayerBirthYear {
            get {
                return ResourceManager.GetString("ValidationPlayerBirthYear", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Пожалуйста, введите имя корректно.
        /// </summary>
        public static string ValidationPlayerFirstName {
            get {
                return ResourceManager.GetString("ValidationPlayerFirstName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Пожалуйста, введите рост корректно.
        /// </summary>
        public static string ValidationPlayerHeight {
            get {
                return ResourceManager.GetString("ValidationPlayerHeight", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Пожалуйста, введите фамилию корректно.
        /// </summary>
        public static string ValidationPlayerLastName {
            get {
                return ResourceManager.GetString("ValidationPlayerLastName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Пожалуйта, введите вес корректно.
        /// </summary>
        public static string ValidationPlayerWeight {
            get {
                return ResourceManager.GetString("ValidationPlayerWeight", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Start of the applying period must be earlier than its end.
        /// </summary>
        public static string WrongRegistrationDatesPeriod {
            get {
                return ResourceManager.GetString("WrongRegistrationDatesPeriod", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Games should start after the end of the applying period.
        /// </summary>
        public static string WrongRegistrationGames {
            get {
                return ResourceManager.GetString("WrongRegistrationGames", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Start of the tournament must be eariler than its end.
        /// </summary>
        public static string WrongStartTournamentDates {
            get {
                return ResourceManager.GetString("WrongStartTournamentDates", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Applying period is less than 3 months.
        /// </summary>
        public static string WrongThreeMonthRule {
            get {
                return ResourceManager.GetString("WrongThreeMonthRule", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Start of the transfer period must be earlier than its end.
        /// </summary>
        public static string WrongTransferPeriod {
            get {
                return ResourceManager.GetString("WrongTransferPeriod", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Start of the transfer window should be later than the games start.
        /// </summary>
        public static string WrongTransferStart {
            get {
                return ResourceManager.GetString("WrongTransferStart", resourceCulture);
            }
        }
    }
}
