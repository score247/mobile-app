namespace LiveScore.Soccer.DTOs.Matches
{
    using System;
    using System.Collections.Generic;
    using LiveScore.Core.Models;
    using LiveScore.Soccer.DTOs.Leagues;
    using LiveScore.Soccer.DTOs.Teams;

    public class MatchDto : Entity<string, string>
    {
        public DateTime EventDate { get; set; }

        public long EventDateUnixTime { get; set; }

        public IEnumerable<TeamDto> Teams { get; set; }

        public MatchResultDto MatchResult { get; set; }

        public TimeLineDto TimeLine { get; set; }

        public MatchConditionDto MatchCondition { get; set; }

        public bool IsLive { get; set; }

        public LeagueDto League { get; set; }
    }
}