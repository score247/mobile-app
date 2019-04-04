namespace LiveScore.Domain.DomainModels
{
    using System;
    using System.Collections.Generic;

    public interface IEvent
    {
        string Id { get; }

        DateTime EventDate { get; }

        ILeague League { get; }

        ILeagueRound LeagueRound { get; }

        IList<ITeam> Teams { get; }

        IVenue Venue { get; }

        string ShortEventDate { get; }
    }

    public class Event : IEvent
    {
        public string Id { get; set; }

        public DateTime EventDate { get; set; }

        public ILeague League { get; set; }

        public ILeagueRound LeagueRound { get; set; }

        public IList<ITeam> Teams { get; set; }

        public IVenue Venue { get; set; }

        public string ShortEventDate { get; set; }
    }
}