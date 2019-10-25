using LiveScore.Core.Enumerations;
using MessagePack;

namespace LiveScore.Core.Models.Matches
{
    [MessagePackObject]
    public class MatchPeriod
    {
        [Key(0)]
        public int HomeScore { get; set; }

        [Key(1)]
        public int AwayScore { get; set; }

        [Key(2)]
        public PeriodType PeriodType { get; set; }

        [Key(3)]
        public int Number { get; set; }
    }
}