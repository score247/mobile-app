namespace LiveScore.Core.Models.Odds
{
    using System.Collections.Generic;
    using MessagePack;

    public interface IBetTypeOdds 
    {
        string MatchId { get; }

        Bookmaker Bookmaker { get; }

        IEnumerable<BetOptionOdds> BetOptions { get; }
    }

    [MessagePackObject(keyAsPropertyName: true)]
    public class BetTypeOdds : IBetTypeOdds
    {      
        public BetTypeOdds(string matchId, Bookmaker bookmaker, IEnumerable<BetOptionOdds> betOptions)
        {
            MatchId = matchId;
            Bookmaker = bookmaker;
            BetOptions = betOptions;
        }

        public string MatchId { get; }

        public Bookmaker Bookmaker { get; }

        public IEnumerable<BetOptionOdds> BetOptions { get; }
    }
}