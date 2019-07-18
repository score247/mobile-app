namespace LiveScore.Core.Models.Odds
{
    using System;
    using System.Collections.Generic;

    public class OddsMovement : Entity<int, string>
    {
        public IEnumerable<BetOptionOdds> BetOptions { get; set; }

        public string MatchTime { get; set; }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
