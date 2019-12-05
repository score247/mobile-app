using LiveScore.Core.Enumerations;
using MessagePack;

namespace LiveScore.Core.Models.Matches
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class MatchPeriod
    {
        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

        public PeriodType PeriodType { get; set; }

        public int Number { get; set; }
    }
}