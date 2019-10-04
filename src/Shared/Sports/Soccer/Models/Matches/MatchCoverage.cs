using LiveScore.Core.Models.Matches;
using MessagePack;

namespace LiveScore.Soccer.Models.Matches
{
    [MessagePackObject]
    public class MatchCoverage
    {
        [Key(0)]
        public string MatchId { get; set; }

        [Key(1)]
        public Coverage Coverage { get; set; }
    }
}