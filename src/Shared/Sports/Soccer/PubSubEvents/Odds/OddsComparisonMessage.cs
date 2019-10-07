﻿using System.Collections.Generic;
using LiveScore.Core.Models.Odds;

namespace LiveScore.Soccer.PubSubEvents.Odds
{
    public class OddsComparisonMessage
    {
        public OddsComparisonMessage(
            byte sportId,
            string matchId,
            IEnumerable<BetTypeOdds> betTypeOddsList)
        {
            SportId = sportId;
            MatchId = matchId;
            BetTypeOddsList = betTypeOddsList;
        }

        public byte SportId { get; private set; }

        public string MatchId { get; private set; }

        public IEnumerable<BetTypeOdds> BetTypeOddsList { get; private set; }
    }
}