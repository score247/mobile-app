namespace LiveScore.Domain.Models.Matches
{
    using System;
    using System.Collections.Generic;
    using LiveScore.Domain.Models.Teams;

    public interface IMatch : IEntity<string, string>
    {
        DateTime EventDate { get; }

        string LeagueId { get; }

        string LeagueName { get; }

        string LeagueRoundName { get; }

        string LeagueCategory { get; }

        IEnumerable<ITeam> Teams { get; }

        bool IsLive { get; }

        IMatchResult MatchResult { get; }

        ITimeLine TimeLine { get; }

        IMatchCondition MatchCondition { get; }
    }

    public class Match : Entity<string, string>, IMatch
    {
        public DateTime EventDate { get; set; }

        public IEnumerable<ITeam> Teams { get; set; }

        public IMatchResult MatchResult { get; set; }

        public ITimeLine TimeLine { get; set; }

        public IMatchCondition MatchCondition { get; set; }

        public bool IsLive { get; set; }

        public string LeagueId { get; set; }

        public string LeagueName { get; set; }

        public string LeagueRoundName { get; set; }

        public string LeagueCategory { get; set; }
    }
}