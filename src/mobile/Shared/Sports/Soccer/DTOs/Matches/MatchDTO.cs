namespace LiveScore.Soccer.DTOs.Matches
{
    using System;
    using System.Collections.Generic;
    using LiveScore.Core.Models;
    using LiveScore.Soccer.DTOs.Leagues;
    using LiveScore.Soccer.DTOs.Teams;

    public class MatchDTO : Entity<string, string>
    {
        public DateTime EventDate { get; set; }

        public IEnumerable<TeamDTO> Teams { get; set; }

        public MatchResultDTO MatchResult { get; set; }

        public TimeLineDTO TimeLine { get; set; }

        public MatchConditionDTO MatchCondition { get; set; }

        public bool IsLive { get; set; }

        public LeagueDTO League { get; set; }
    }
}