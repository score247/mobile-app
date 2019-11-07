using System;
using System.Linq;
using LiveScore.Core.Enumerations;
using LiveScore.Soccer.Models.Matches;

namespace LiveScore.Soccer.Extensions
{
    public static class CommentaryExtension
    {
        private static readonly EventType[] InvisibleMatchTimeEventTypes =
        {
            EventType.BreakStart,
            EventType.MatchEnded,
            EventType.PenaltyShootout,
            EventType.PeriodStart,
            EventType.MatchStarted
        };

        public static bool IsVisibleMatchTimeEvent(this MatchCommentary matchCommentary)
            => !InvisibleMatchTimeEventTypes.Contains(matchCommentary.TimelineType);
    }
}