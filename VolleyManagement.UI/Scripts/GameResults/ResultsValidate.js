﻿function getResults() {
    return {
        "SetsScore": {
            "Home": $("#SetsScore_Home").val(),
            "Away": $("#SetsScore_Away").val()
        },
        "SetScores":
            [
               {
                   "Home": $("#SetScores_0__Home").val(),
                   "Away": $("#SetScores_0__Away").val()
               },
                {
                    "Home": $("#SetScores_1__Home").val(),
                    "Away": $("#SetScores_1__Away").val()
                },
                {
                    "Home": $("#SetScores_2__Home").val(),
                    "Away": $("#SetScores_2__Away").val()
                },
                {
                    "Home": $("#SetScores_3__Home").val(),
                    "Away": $("#SetScores_3__Away").val()
                },
                {
                    "Home": $("#SetScores_4__Home").val(),
                    "Away": $("#SetScores_4__Away").val()
                }
            ],
        "Teams": {
            "Home": $("#HomeTeamId").val(),
            "Away": $("#AwayTeamId").val()
        },
        "IsTechnicalDefeat": $("#IsTechnicalDefeat")[0].checked
    };
}


// Methods provides units of rules.

function AreTheSameTeams(HomeId, AwayId) {
    return HomeId == AwayId;
}

function AreSetScoresMatched(setsScore, setScores) {
    var score = {
        "Home": 0,
        "Away": 0
    }

    for (i = 0; i < setScores.length; i++) {
        if (setScores[i].Home > setScores[i].Away) {
            score.Home++;
        }
        else if (setScores[i].Home < setScores[i].Away) {
            score.Away++;
        }
    }

    return score.Home == setsScore.Home && score.Away == setsScore.Away;
}

function IsSetsScoreValid(setsScore, isTechnicalDefeat) {
    return isTechnicalDefeat ? IsTechnicalDefeatSetsScoreValid(setsScore) : IsOrdinarySetsScoreValid(setsScore);
}

function IsRequiredSetScoreValid(setScore, isTechnicalDefeat) {
    return isTechnicalDefeat ? IsTechnicalDefeatRequiredSetScoreValid(setScore) : IsOrdinaryRequiredSetScoreValid(setScore);
}

function IsOptionalSetScoreValid(setScore, isTechnicalDefeat) {
    return isTechnicalDefeat ? IsTechnicalDefeatOptionalSetScoreValid(setScore) : IsOrdinaryOptionalSetScoreValid(setScore);
}

function IsTechnicalDefeatSetsScoreValid(setsScore) {
    return (setsScore.Home == gameResultConstants.TECHNICAL_DEFEAT_SETS_WINNER_SCORE
        && setsScore.Away == gameResultConstants.TECHNICAL_DEFEAT_SETS_LOSER_SCORE)
        || (setsScore.Home == gameResultConstants.TECHNICAL_DEFEAT_SETS_LOSER_SCORE
        && setsScore.Away == gameResultConstants.TECHNICAL_DEFEAT_SETS_WINNER_SCORE);
}

function IsOrdinarySetsScoreValid(setsScore) {
    return (setsScore.Home == gameResultConstants.SETS_COUNT_TO_WIN
        && setsScore.Away < gameResultConstants.SETS_COUNT_TO_WIN)
        || (setsScore.Home < gameResultConstants.SETS_COUNT_TO_WIN
        && setsScore.Away == gameResultConstants.SETS_COUNT_TO_WIN);
}

function IsTechnicalDefeatRequiredSetScoreValid(setScore) {
    return (setScore.Home == gameResultConstants.TECHNICAL_DEFEAT_SET_WINNER_SCORE
        && setScore.Away == gameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE)
        || (setScore.Home == gameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE
        && setScore.Away == gameResultConstants.TECHNICAL_DEFEAT_SET_WINNER_SCORE);
}

function IsTechnicalDefeatOptionalSetScoreValid(setScore) {
    return setScore.Home == gameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE
        && setScore.Away == gameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE;
}

function IsOrdinaryRequiredSetScoreValid(setScore) {
    var isValid = false;

    if (IsSetScoreGreaterThanMin(setScore)) {
        isValid = Math.abs(setScore.Home - setScore.Away) == gameResultConstants.SET_POINTS_MIN_DELTA_TO_WIN;
    }
    else if (IsSetScoreEqualToMin(setScore)) {
        isValid = Math.abs(setScore.Home - setScore.Away) >= gameResultConstants.SET_POINTS_MIN_DELTA_TO_WIN;
    }

    return isValid;
}

function IsOrdinaryOptionalSetScoreValid(setScore) {
    return IsOrdinaryRequiredSetScoreValid(setScore) || IsSetUnplayed(setScore);
}

function IsSetScoreEqualToMin(setScore) {
    return setScore.Home == gameResultConstants.SET_POINTS_MIN_VALUE_TO_WIN
        || setScore.Away == gameResultConstants.SET_POINTS_MIN_VALUE_TO_WIN;
}

function IsSetScoreGreaterThanMin(setScore) {
    return setScore.Home > gameResultConstants.SET_POINTS_MIN_VALUE_TO_WIN
        || setScore.Away > gameResultConstants.SET_POINTS_MIN_VALUE_TO_WIN;
}

function IsSetUnplayed(setScore) {
    return setScore.Home == gameResultConstants.UNPLAYED_SET_HOME_SCORE
        && setScore.Away == gameResultConstants.UNPLAYED_SET_AWAY_SCORE;
}

// Methods to validate.

function ValidateGameResult(gameResult) {
    ValidateTeams(gameResult.Teams.Home, gameResult.Teams.Away);
    ValidateSetsScore(gameResult.SetsScore, gameResult.IsTechnicalDefeat);
    ValidateSetScores(gameResult.SetScores, gameResult.IsTechnicalDefeat);
    ValidateSetsScoreMatchesSetScores(gameResult.SetsScore, gameResult.SetScores);
}

function ValidateTeams(homeTeamId, awayTeamId) {
    if (AreTheSameTeams(homeTeamId, awayTeamId)) {
        throw resourceMessages.GameResultSameTeam;
    }
}

function ValidateSetsScore(setsScore, isTechnicalDefeat) {
    if (!IsSetsScoreValid(setsScore, isTechnicalDefeat)) {
        var template = jQuery.validator.format(resourceMessages.GameResultSetsScoreInvalid);
        throw template(
            gameResultConstants.VALID_SETS_SCORES,
            gameResultConstants.TECHNICAL_DEFEAT_SETS_WINNER_SCORE,
            gameResultConstants.TECHNICAL_DEFEAT_SETS_LOSER_SCORE);
    }
}

function ValidateSetScores(setScores, isTechnicalDefeat) {
    var isPreviousOptionalSetUnplayed = false;

    for (i = 0; i < setScores.length; i++) {
        if (i < gameResultConstants.SETS_COUNT_TO_WIN) {
            if (!IsRequiredSetScoreValid(setScores[i], isTechnicalDefeat)) {
                var template = jQuery.validator.format(resourceMessages.GameResultRequiredSetScores);
                throw template(
                    gameResultConstants.SET_POINTS_MIN_VALUE_TO_WIN,
                    gameResultConstants.SET_POINTS_MIN_DELTA_TO_WIN,
                    gameResultConstants.TECHNICAL_DEFEAT_SET_WINNER_SCORE,
                    gameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE);
            }
        }
        else {
            if (!IsOptionalSetScoreValid(setScores[i], isTechnicalDefeat)) {
                var template = jQuery.validator.format(resourceMessages.GameResultOptionalSetScores);
                throw template(
                    gameResultConstants.SET_POINTS_MIN_VALUE_TO_WIN,
                    gameResultConstants.SET_POINTS_MIN_DELTA_TO_WIN,
                    gameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE,
                    gameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE);
            }

            if (isPreviousOptionalSetUnplayed) {
                if (!IsSetUnplayed(setScores[i])) {
                    var template = jQuery.validator.format(resourceMessages.GameResultPreviousOptionalSetUnplayed);
                    throw resourceMessages.GameResultPreviousOptionalSetUnplayed;
                }
            }

            isPreviousOptionalSetUnplayed = IsSetUnplayed(setScores[i]);
        }
    }
}

function ValidateSetsScoreMatchesSetScores(setsScore, setScores) {
    if (!AreSetScoresMatched(setsScore, setScores)) {
        throw resourceMessages.GameResultSetsScoreNoMatchSetScores;
    }
}

$("#createForm").submit(function (e) {
   e.preventDefault();
}).validate({
    submitHandler: function (form) {
        try {
            var gameResult = getResults();
            ValidateGameResult(gameResult);
            form.submit();
        }
        catch (message) {
            errors = { ValidationMessage: message };
            this.showErrors(errors);
            return false;
        }
    }
});
