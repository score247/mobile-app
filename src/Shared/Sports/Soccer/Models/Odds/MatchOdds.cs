using MessagePack;

namespace LiveScore.Soccer.Models.Odds
{
    using System.Collections.Generic;
    using LiveScore.Core.Models.Odds;
    using MessagePack;

    [MessagePackObject(keyAsPropertyName: true)]
    public class MatchOdds
    {
        public string MatchId { get; set; }

        public IEnumerable<BetTypeOdds> BetTypeOddsList { get; set; }
    }
}