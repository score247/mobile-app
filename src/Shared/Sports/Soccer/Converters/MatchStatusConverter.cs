namespace LiveScore.Soccer.Converters
{
    using System.Collections.Generic;
    using LiveScore.Common.Extensions;
    using LiveScore.Common.LangResources;
    using LiveScore.Core.Converters;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;

    public class MatchStatusConverter : IMatchStatusConverter
    {
        private static readonly IDictionary<string, string> StatusMapper = new Dictionary<string, string>
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

        public string BuildStatus(IMatch match)
        {
            if (match == null)
            {
                return AppResources.FT;
            }

            if (match.MatchResult.EventStatus.IsNotStarted)
            {
                return match.EventDate.ToTimeWithoutSecond();
            }

            if (match.MatchResult.EventStatus.IsLive)
            {
                return BuildStatusForLive(match);
            }

            if (match.MatchResult.EventStatus.IsClosed)
            {
                var status = BuildMatchStatus(match);

                return string.IsNullOrEmpty(status) ? AppResources.FT : status;
            }

            return BuildEventStatus(match);
        }

        private static string BuildStatusForLive(IMatch match)
        {
            var status = BuildMatchStatus(match);

            if (!string.IsNullOrEmpty(status))
            {
                return status;
            }

            return status;
        }

        private static string BuildMatchStatus(IMatch match)
        {
            var matchStatus = match.MatchResult?.MatchStatus;

            if (matchStatus?.Value != null && StatusMapper.ContainsKey(matchStatus.Value))
            {
                return StatusMapper[matchStatus.Value];
            }

            return string.Empty;
        }

        private static string BuildEventStatus(IMatch match)
        {
            var eventStatus = match.MatchResult?.EventStatus;

            if (eventStatus?.Value != null && StatusMapper.ContainsKey(eventStatus.Value))
            {
                return StatusMapper[eventStatus.Value];
            }

            return string.Empty;
        }
    }
}
