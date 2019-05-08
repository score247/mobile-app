namespace LiveScore.Core.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using LiveScore.Common.Extensions;
    using LiveScore.Common.LangResources;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using Xamarin.Forms;

    public class MatchStatusConverter : IValueConverter
    {
        private static readonly IDictionary<string, string> StatusDisplayNames = new Dictionary<string, string>
        {
            { MatchStatus.Postponed, AppResources.Postp },
            { MatchStatus.StartDelayed, AppResources.StartDelay },
            { MatchStatus.Cancelled, AppResources.Canc },
            { MatchStatus.AwaitingPenalties, AppResources.AwaitPen },
            { MatchStatus.Penalties, AppResources.Pen },
            { MatchStatus.Pause, AppResources.Pause },
            { MatchStatus.Interrupted, AppResources.INT },
            { MatchStatus.Halftime, AppResources.HT },
            { MatchStatus.Delayed, AppResources.Delayed },
            { MatchStatus.Abandoned, AppResources.AB },
            { MatchStatus.FullTime, AppResources.FT },
            { MatchStatus.Ended, AppResources.FT },
            { MatchStatus.Closed, AppResources.FT },
            { MatchStatus.EndedAfterPenalties, AppResources.AP },
            { MatchStatus.EndedExtraTime, AppResources.AET },
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return AppResources.FT;
            }

            var match = (IMatch)value;

            if (match.MatchResult.EventStatus.IsNotStarted)
            {
                return match.EventDate.ToTimeWithoutSecond();
            }

            if (match.MatchResult.EventStatus.IsLive)
            {
                return GenerateStatusForLiveMatch(match);
            }

            if (match.MatchResult.EventStatus.IsClosed)
            {
                return GenerateMatchStatus(match);
            }

            return GenerateEventStatus(match);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (MatchStatus)value;
        }

        private static string GenerateStatusForLiveMatch(IMatch match)
        {
            var status = GenerateMatchStatus(match);

            if (!string.IsNullOrEmpty(status))
            {
                return status;
            }

            var matchTime = DateTime.ParseExact(match.MatchResult.MatchTime, "mm:ss", CultureInfo.InvariantCulture);

            if (match.TimeLine != null && match.TimeLine.Type.IsInjuryTimeShown)
            {
                return $"{matchTime.Minute}+{match.TimeLine.StoppageTime}'";
            }

            return matchTime.Minute + "'";
        }

        private static string GenerateMatchStatus(IMatch match)
        {
            var matchStatus = match.MatchResult?.MatchStatus;

            if (matchStatus != null && StatusDisplayNames.ContainsKey(matchStatus.Value))
            {
                return StatusDisplayNames[matchStatus.Value];
            }

            return string.Empty;
        }

        private static string GenerateEventStatus(IMatch match)
        {
            var eventStatus = match.MatchResult?.EventStatus;

            if (eventStatus != null && StatusDisplayNames.ContainsKey(eventStatus.Value))
            {
                return StatusDisplayNames[eventStatus.Value];
            }

            return string.Empty;
        }
    }
}