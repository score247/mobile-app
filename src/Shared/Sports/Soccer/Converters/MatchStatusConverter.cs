using System.Collections.Generic;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Core.Converters;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;
using LiveScore.Soccer.Models.Matches;

namespace LiveScore.Soccer.Converters
{
    public class MatchStatusConverter : IMatchStatusConverter
    {
        private static readonly IDictionary<MatchStatus, string> StatusResourceMapper = new Dictionary<MatchStatus, string>
        {
            { MatchStatus.Postponed, AppResources.Postp },
            { MatchStatus.StartDelayed, AppResources.Delayed },
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
            { MatchStatus.AwaitingExtraTime, AppResources.AwaitET },
            { MatchStatus.ExtraTimeHalfTime, AppResources.ETHT }
        };

        public string BuildStatus(IMatch match)
        {
            if (match == null)
            {
                return string.Empty;
            }

            var soccerMatch = match as Match;

            if (soccerMatch?.EventStatus == null)
            {
                return AppResources.FT;
            }

            if (soccerMatch.EventStatus.IsNotStarted)
            {
                return match.EventDate.ToLocalTimeWithoutSecond();
            }

            if (soccerMatch.EventStatus.IsLive)
            {
                return BuildMatchStatus(soccerMatch);
            }

            if (soccerMatch.EventStatus.IsClosed)
            {
                var status = BuildMatchStatus(soccerMatch);

                return string.IsNullOrEmpty(status) ? AppResources.FT : status;
            }

            return BuildEventStatus(soccerMatch);
        }

        private static string BuildMatchStatus(Match match)
        {
            var matchStatus = match.MatchStatus;

            if (matchStatus?.Value != null && StatusResourceMapper.ContainsKey(matchStatus))
            {
                return StatusResourceMapper[matchStatus];
            }

            return string.Empty;
        }

        private static string BuildEventStatus(Match match)

        {
            var eventStatus = match.EventStatus;

            if (eventStatus?.Value != null && StatusResourceMapper.ContainsKey(eventStatus))
            {
                return StatusResourceMapper[eventStatus];
            }

            return string.Empty;
        }
    }
}