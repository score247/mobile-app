namespace LiveScore.Domain.DomainModels
{
    using System;
    using System.Collections.Generic;

    public interface IMatch : IEntity<string, string>
    {
        DateTime EventDate { get; }

        ILeague League { get; }

        ILeagueRound LeagueRound { get; }

        IList<ITeam> Teams { get; }

        IVenue Venue { get; }

        string ShortEventDate { get; }

        IMatchResult MatchResult { get; }

        ITimeLine TimeLine { get; }

        IMatchCondition EventCondition { get; }
    }

    public class Match : Entity<string, string>, IMatch
    {
        public DateTime EventDate { get; set; }

        public ILeague League { get; set; }

        public ILeagueRound LeagueRound { get; set; }

        public IList<ITeam> Teams { get; set; }

        public IVenue Venue { get; set; }

        public string ShortEventDate { get; set; }

        public IMatchResult MatchResult { get; set; }

        public ITimeLine TimeLine { get; set; }

        public IMatchCondition EventCondition { get; set; }
    }
}