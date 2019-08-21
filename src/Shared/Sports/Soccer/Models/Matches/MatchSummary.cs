namespace LiveScore.Soccer.Models.Matches
{
    using System;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;

    public class MatchSummary : IMatchSummary
    {
        /// <summary>
        /// Keep private setter for Json Serializer
        /// </summary>
        public string Id { get; private set; }

        public DateTimeOffset EventDate { get; private set; }

        public string LeagueId { get; private set; }

        public string LeagueName { get; private set; }

        public string HomeTeamId { get; private set; }

        public string HomeTeamName { get; private set; }

        public string AwayTeamId { get; private set; }

        public string AwayTeamName { get; private set; }

        public MatchStatus MatchStatus { get; private set; }

        public MatchStatus EventStatus { get; private set; }

        public byte HomeScore { get; private set; }

        public byte AwayScore { get; private set; }

        public string AggregateWinnerId { get; private set; }

        public byte HomeRedCards { get; private set; }

        public byte AwayRedCards { get; private set; }

        public byte MatchTime { get; private set; }

        public string StoppageTime { get; private set; }

        public byte InjuryTimeAnnounced { get; private set; }

        public EventType LastTimelineType { get; private set; }
    }
}