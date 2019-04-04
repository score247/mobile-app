namespace LiveScore.Domain.DomainModels
{
    using System;
    using System.Collections.Generic;

    public interface IMatch : IEntity<string, string>
    {
        DateTime EventDate { get; }

        IList<ITeam> Teams { get; }

        IMatchResult MatchResult { get; }

        ITimeLine TimeLine { get; }

        IMatchCondition EventCondition { get; }
    }

    public class Match : Entity<string, string>, IMatch
    {
        public DateTime EventDate { get; set; }

        public IList<ITeam> Teams { get; set; }

        public IMatchResult MatchResult { get; set; }

        public ITimeLine TimeLine { get; set; }

        public IMatchCondition EventCondition { get; set; }
    }
}