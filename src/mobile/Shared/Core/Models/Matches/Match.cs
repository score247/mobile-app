namespace LiveScore.Core.Models.Matches
{
    using System;
    using System.Collections.Generic;
    using LiveScore.Core.Models.Leagues;
    using LiveScore.Core.Models.Teams;

    public interface IMatch : IEntity<string, string>
    {
        DateTime EventDate { get; }

        IEnumerable<ITeam> Teams { get; }

        bool IsLive { get; }

        IMatchResult MatchResult { get; }

        ITimeLine TimeLine { get; }

        IMatchCondition MatchCondition { get; }

        ILeague League { get; }
    }
}