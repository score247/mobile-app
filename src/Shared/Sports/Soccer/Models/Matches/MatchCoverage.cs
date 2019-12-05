using LiveScore.Core.Models.Matches;
using MessagePack;

namespace LiveScore.Soccer.Models.Matches
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class MatchCoverage
    {
        public string MatchId { get; set; }

        public Coverage Coverage { get; set; }
    }
}