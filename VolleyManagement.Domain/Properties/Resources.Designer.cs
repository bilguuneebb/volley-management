﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VolleyManagement.Domain.Properties {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
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
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
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
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
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
        ///   Ищет локализованную строку, похожую на The number of divisions in the tournament must be between {0} and {1}.
        /// </summary>
        public static string DivisionCountOutOfRange {
            get {
                return ResourceManager.GetString("DivisionCountOutOfRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Divisions can not have the same name.
        /// </summary>
        public static string DivisionNamesNotUnique {
            get {
                return ResourceManager.GetString("DivisionNamesNotUnique", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Content.
        /// </summary>
        public static string FeedbackContentParam {
            get {
                return ResourceManager.GetString("FeedbackContentParam", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Status.
        /// </summary>
        public static string FeedbackStatusParam {
            get {
                return ResourceManager.GetString("FeedbackStatusParam", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на UsersEmail.
        /// </summary>
        public static string FeedbackUsersEmailParam {
            get {
                return ResourceManager.GetString("FeedbackUsersEmailParam", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на In fifth set, the number of points for one team must be at least {0} and the points difference should be at least {1}. If the score exceeds {0}, the points difference must be equal to {1}..
        /// </summary>
        public static string GameResultFifthSetScoreInvalid {
            get {
                return ResourceManager.GetString("GameResultFifthSetScoreInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на In the set, the number of points for one team must be at least {0} and the points difference should be at least {1}. If the score exceeds {0}, the points difference must be equal to {1}. In case of a technical defeat score of an optional game should be {2}: {3}.
        /// </summary>
        public static string GameResultOptionalSetScores {
            get {
                return ResourceManager.GetString("GameResultOptionalSetScores", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Enter a score of the previous optional game.
        /// </summary>
        public static string GameResultPreviousOptionalSetUnplayed {
            get {
                return ResourceManager.GetString("GameResultPreviousOptionalSetUnplayed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на In the set, the number of points for one team must be at least {0} and the points difference should be at least {1}. If the score exceeds {0}, the points difference must be equal to {1}. In case of a technical defeat score of an optional game should be {2}: {3} or {3}: {2}.
        /// </summary>
        public static string GameResultRequiredSetScores {
            get {
                return ResourceManager.GetString("GameResultRequiredSetScores", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на The team can not play with itself.
        /// </summary>
        public static string GameResultSameTeam {
            get {
                return ResourceManager.GetString("GameResultSameTeam", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на The set scores are listed in the wrong order.
        /// </summary>
        public static string GameResultSetScoresNotOrdered {
            get {
                return ResourceManager.GetString("GameResultSetScoresNotOrdered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на The set score can be one of the following: 3:0, 3:1, 3:2, 2:3, 1:3, 0:3. In case of a technical defeat set score must be {0}:{1} or {1}:{0}.
        /// </summary>
        public static string GameResultSetsScoreInvalid {
            get {
                return ResourceManager.GetString("GameResultSetsScoreInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Game score does not match set scores.
        /// </summary>
        public static string GameResultSetsScoreNoMatchSetScores {
            get {
                return ResourceManager.GetString("GameResultSetsScoreNoMatchSetScores", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на An amount of groups in division should be between {0} and {1}.
        /// </summary>
        public static string GroupCountOutOfRange {
            get {
                return ResourceManager.GetString("GroupCountOutOfRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Group names should be unique.
        /// </summary>
        public static string GroupNamesNotUnique {
            get {
                return ResourceManager.GetString("GroupNamesNotUnique", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Home team can&apos;t be set to freeday.
        /// </summary>
        public static string HomeTeamNullId {
            get {
                return ResourceManager.GetString("HomeTeamNullId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на End of the transfer period should be before the end of the tournament.
        /// </summary>
        public static string InvalidTransferEndpoint {
            get {
                return ResourceManager.GetString("InvalidTransferEndpoint", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Start of the applying period must be later than today.
        /// </summary>
        public static string LateRegistrationDates {
            get {
                return ResourceManager.GetString("LateRegistrationDates", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Body.
        /// </summary>
        public static string MailBodyParam {
            get {
                return ResourceManager.GetString("MailBodyParam", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на From.
        /// </summary>
        public static string MailFromParam {
            get {
                return ResourceManager.GetString("MailFromParam", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на To.
        /// </summary>
        public static string MailToParam {
            get {
                return ResourceManager.GetString("MailToParam", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Game cannot be added to the tournament that doesn&apos;t exist.
        /// </summary>
        public static string NoSuchToruanment {
            get {
                return ResourceManager.GetString("NoSuchToruanment", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на No teams are specified for current game in round {0}.
        /// </summary>
        public static string NoTeamsInGame {
            get {
                return ResourceManager.GetString("NoTeamsInGame", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Editing of the old game is not allowed.
        /// </summary>
        public static string PlayoffGameEditingError {
            get {
                return ResourceManager.GetString("PlayoffGameEditingError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Game date not set..
        /// </summary>
        public static string RoundDateNotSet {
            get {
                return ResourceManager.GetString("RoundDateNotSet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Free day game has already been scheduled in this round.
        /// </summary>
        public static string SameFreeDayGameInRound {
            get {
                return ResourceManager.GetString("SameFreeDayGameInRound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Game between {0} and {1} has already been scheduled for round number {2}.
        /// </summary>
        public static string SameGameInRound {
            get {
                return ResourceManager.GetString("SameGameInRound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Game between {0} and {1} has already been scheduled in another round. Same teams cannot play twice in one round tournament.
        /// </summary>
        public static string SameGameInTournamentSchemeOne {
            get {
                return ResourceManager.GetString("SameGameInTournamentSchemeOne", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Game between {0} and {1} has already been scheduled in two other rounds. Same teams connot play more than 2 times in two round tournament.
        /// </summary>
        public static string SameGameInTournamentSchemeTwo {
            get {
                return ResourceManager.GetString("SameGameInTournamentSchemeTwo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Team {0} has game scheduled already in this round.
        /// </summary>
        public static string SameTeamInRound {
            get {
                return ResourceManager.GetString("SameTeamInRound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Team with such name already exists in tournament.
        /// </summary>
        public static string TeamNameInTournamentNotUnique {
            get {
                return ResourceManager.GetString("TeamNameInTournamentNotUnique", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Tournament name should be unique.
        /// </summary>
        public static string TournamentNameMustBeUnique {
            get {
                return ResourceManager.GetString("TournamentNameMustBeUnique", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на If there is a transfer, it&apos;s necessary to specify the end date of the transfer period.
        /// </summary>
        public static string TransferEndMissing {
            get {
                return ResourceManager.GetString("TransferEndMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на If there is a transfer, it&apos;s necessary to specify the start date of the transfer period.
        /// </summary>
        public static string TransferStartMissing {
            get {
                return ResourceManager.GetString("TransferStartMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Tournament name should be unique.
        /// </summary>
        public static string UniqueNameMessage {
            get {
                return ResourceManager.GetString("UniqueNameMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Coach name can not contain more than  {0} symbols; The field must contain only letters.
        /// </summary>
        public static string ValidationCoachName {
            get {
                return ResourceManager.GetString("ValidationCoachName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Please, enter the valid name of the division.
        /// </summary>
        public static string ValidationDivisionName {
            get {
                return ResourceManager.GetString("ValidationDivisionName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Content can&apos;t be empty or contains more than {0} symbols.
        /// </summary>
        public static string ValidationFeedbackContent {
            get {
                return ResourceManager.GetString("ValidationFeedbackContent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Feedback status can&apos;t be changed to this status.
        /// </summary>
        public static string ValidationFeedbackStatus {
            get {
                return ResourceManager.GetString("ValidationFeedbackStatus", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Email can&apos;t be empty or contains more than {0} symbols.
        /// </summary>
        public static string ValidationFeedbackUsersEmail {
            get {
                return ResourceManager.GetString("ValidationFeedbackUsersEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Please, enter the valid name of the group.
        /// </summary>
        public static string ValidationGroupName {
            get {
                return ResourceManager.GetString("ValidationGroupName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Body should be less than {0} symbols or not empty.
        /// </summary>
        public static string ValidationMailBody {
            get {
                return ResourceManager.GetString("ValidationMailBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Please, enter the valid email..
        /// </summary>
        public static string ValidationMailEmail {
            get {
                return ResourceManager.GetString("ValidationMailEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Please enter the valid birth date.
        /// </summary>
        public static string ValidationPlayerBirthYear {
            get {
                return ResourceManager.GetString("ValidationPlayerBirthYear", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Please, enter the valid first name.
        /// </summary>
        public static string ValidationPlayerFirstName {
            get {
                return ResourceManager.GetString("ValidationPlayerFirstName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Please, enter the valid height.
        /// </summary>
        public static string ValidationPlayerHeight {
            get {
                return ResourceManager.GetString("ValidationPlayerHeight", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Please, enter the valid last name.
        /// </summary>
        public static string ValidationPlayerLastName {
            get {
                return ResourceManager.GetString("ValidationPlayerLastName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Please, enter the valid weight.
        /// </summary>
        public static string ValidationPlayerWeight {
            get {
                return ResourceManager.GetString("ValidationPlayerWeight", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Team achievements can not contain more than  {0} symbols.
        /// </summary>
        public static string ValidationTeamAchievements {
            get {
                return ResourceManager.GetString("ValidationTeamAchievements", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Team&apos;s id is wrong.
        /// </summary>
        public static string ValidationTeamId {
            get {
                return ResourceManager.GetString("ValidationTeamId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Team name can not contain more than  {0} symbols; The field can not be empty.
        /// </summary>
        public static string ValidationTeamName {
            get {
                return ResourceManager.GetString("ValidationTeamName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Tournament&apos;s id is wrong.
        /// </summary>
        public static string ValidationTournamentId {
            get {
                return ResourceManager.GetString("ValidationTournamentId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на User&apos;s id is wrong.
        /// </summary>
        public static string ValidationUserId {
            get {
                return ResourceManager.GetString("ValidationUserId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Game can not be deleted because it has already ended.
        /// </summary>
        public static string WrongDeletingGame {
            get {
                return ResourceManager.GetString("WrongDeletingGame", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Start of the applying period must be earlier than its end.
        /// </summary>
        public static string WrongRegistrationDatesPeriod {
            get {
                return ResourceManager.GetString("WrongRegistrationDatesPeriod", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Games should start after the end of the applying period.
        /// </summary>
        public static string WrongRegistrationGames {
            get {
                return ResourceManager.GetString("WrongRegistrationGames", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Start of the round should not be earlier than the start of the tournament or later than the end of the tournament.
        /// </summary>
        public static string WrongRoundDate {
            get {
                return ResourceManager.GetString("WrongRoundDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Start of the tournament must be eariler than its end.
        /// </summary>
        public static string WrongStartTournamentDates {
            get {
                return ResourceManager.GetString("WrongStartTournamentDates", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Applying period is less than 3 months.
        /// </summary>
        public static string WrongThreeMonthRule {
            get {
                return ResourceManager.GetString("WrongThreeMonthRule", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Start of the transfer period must be earlier than its end.
        /// </summary>
        public static string WrongTransferPeriod {
            get {
                return ResourceManager.GetString("WrongTransferPeriod", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Start of the transfer window should be later than the games start.
        /// </summary>
        public static string WrongTransferStart {
            get {
                return ResourceManager.GetString("WrongTransferStart", resourceCulture);
            }
        }
    }
}
