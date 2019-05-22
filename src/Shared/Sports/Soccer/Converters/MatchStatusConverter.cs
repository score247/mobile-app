namespace LiveScore.Soccer.Converters
{
    using System.Collections.Generic;
    using LiveScore.Common.Extensions;
    using LiveScore.Common.LangResources;
    using LiveScore.Core.Converters;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using System.Linq;

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

        private static readonly IDictionary<string, int> PeriodEndTimes = new Dictionary<string, int>
        {
            { MatchStatus.FirstHaft, 45 },
            { MatchStatus.SecondHaft, 90 },
            { MatchStatus.FirstHaftExtra, 105 },
            { MatchStatus.SecondHaftExtra, 120 }
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

            var timeline = match.TimeLines?.FirstOrDefault();

            if (timeline != null && timeline.Type == EventTypes.InjuryTimeShown)
            {
                var periodEndTime = PeriodEndTimes[match.MatchResult.MatchStatus.Value];
                var annoucedInjuryTime = timeline.InjuryTimeAnnounced;
                var currentInjuryTime = match.MatchResult.MatchTime - periodEndTime;
                var displayInjuryTime = currentInjuryTime == 0 ? 1 : currentInjuryTime;

                if (currentInjuryTime > annoucedInjuryTime)
                {
                    displayInjuryTime = annoucedInjuryTime;
                }

                return $"{periodEndTime}+{displayInjuryTime}'";
            }

            return match.MatchResult.MatchTime + "'";
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
