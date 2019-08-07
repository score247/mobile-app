namespace LiveScore.Core.Models.Matches
{
    using System.Collections.Generic;
    using LiveScore.Common.Extensions;
    using Newtonsoft.Json;

    public interface IMatchEvent
    {
        string MatchId { get; }

        IMatchResult MatchResult { get; }

        ITimelineEvent Timeline { get; }
    }
}