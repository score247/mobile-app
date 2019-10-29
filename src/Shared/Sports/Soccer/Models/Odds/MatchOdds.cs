using System.Collections.Generic;
using LiveScore.Core.Models.Odds;
using MessagePack;

namespace LiveScore.Soccer.Models.Odds
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class MatchOdds
    {
        public MatchOdds(string matchId, IEnumerable<BetTypeOdds> betTypeOddsList)
        {
            MatchId = matchId;
            BetTypeOddsList = betTypeOddsList;
        }

        public string MatchId { get; }

        public IEnumerable<BetTypeOdds> BetTypeOddsList { get; }
    }
}