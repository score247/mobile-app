using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Core.Converters;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;
using LiveScore.Soccer.Models.Matches;

namespace LiveScore.Soccer.Converters
{
    public class MatchStatusConverter : IMatchDisplayStatusBuilder
    {
        private static readonly ReadOnlyDictionary<MatchStatus, string> StatusResourceMapper =
            new ReadOnlyDictionary<MatchStatus, string>(new Dictionary<MatchStatus, string>
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
            });

        public string BuildDisplayStatus(IMatch match)
        {
            if (!(match is SoccerMatch) || match == null)
            {
                return string.Empty;
            }

            var soccerMatch = match as SoccerMatch;

            if (soccerMatch.EventStatus == null)
            {
                return AppResources.FT;
            }

            if (soccerMatch.EventStatus.IsNotStarted)
            {
                return match.EventDate.ToLocalTimeWithoutSecond();
            }

            if (soccerMatch.EventStatus.IsLive)
            {
                return BuildStatus(() => soccerMatch.MatchStatus);
            }

            if (soccerMatch.EventStatus.IsClosed)
            {
                var status = BuildStatus(() => soccerMatch.MatchStatus);

                return string.IsNullOrEmpty(status) ? AppResources.FT : status;
            }

            return BuildStatus(() => soccerMatch.EventStatus);
        }

        private static string BuildStatus(Func<MatchStatus> statusGetter)
        {
            var status = statusGetter();

            if (status?.Value != null && StatusResourceMapper.ContainsKey(status))
            {
                return StatusResourceMapper[status];
            }

            return string.Empty;
        }
    }
}