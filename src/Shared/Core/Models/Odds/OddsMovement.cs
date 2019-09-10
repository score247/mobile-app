namespace LiveScore.Core.Models.Odds
{
    using System;
    using System.Collections.Generic;
    using MessagePack;

    public interface IOddsMovement
    {
        IEnumerable<BetOptionOdds> BetOptions { get; }

        string MatchTime { get; }

        int HomeScore { get; }

        int AwayScore { get; }

        DateTimeOffset UpdateTime { get; }

        bool IsMatchStarted { get; }
    }

    [MessagePackObject(keyAsPropertyName: true)]
    public class OddsMovement : IOddsMovement
    {
        public IEnumerable<BetOptionOdds> BetOptions { get; set; }

        public string MatchTime { get; set; }

        public DateTimeOffset UpdateTime { get; set; }

        public bool IsMatchStarted { get; set; }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }
    }
}