namespace LiveScore.Core.Models.Matches
{
    using System;
    using System.Collections.Generic;
    using LiveScore.Core.Models.Leagues;
    using LiveScore.Core.Models.Teams;

    public interface IMatch : IEntity<string, string>
    {
        DateTime EventDate { get; }

        long EventDateUnixTime { get; }

        IEnumerable<ITeam> Teams { get; }

        bool IsLive { get; }

        IMatchResult MatchResult { get; }

        ITimeLine TimeLine { get; }

        IMatchCondition MatchCondition { get; }

        ILeague League { get; }
    }

    public class Match : Entity<string, string>, IMatch
    {
        public DateTime EventDate { get; set; }

        public long EventDateUnixTime { get; set; }

        public IEnumerable<ITeam> Teams { get; set; }

        public IMatchResult MatchResult { get; set; }

        public ITimeLine TimeLine { get; set; }

        public IMatchCondition MatchCondition { get; set; }

        public bool IsLive { get; set; }

        public ILeague League { get; set; }
    }
}