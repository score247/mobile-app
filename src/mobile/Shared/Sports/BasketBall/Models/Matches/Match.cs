namespace LiveScore.BasketBall.Models.Matches
{
    using System;
    using System.Collections.Generic;
    using LiveScore.Core.Models;
    using LiveScore.Core.Models.Leagues;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Teams;

    public class Match : Entity<string, string>, IMatch
    {
        public DateTime EventDate { get; set; }

        public IEnumerable<ITeam> Teams { get; set; }

        public IMatchResult MatchResult { get; set; }

        public ITimeLine TimeLine { get; set; }

        public IMatchCondition MatchCondition { get; set; }

        public bool IsLive { get; set; }

        public ILeague League { get; set; }
    }
}