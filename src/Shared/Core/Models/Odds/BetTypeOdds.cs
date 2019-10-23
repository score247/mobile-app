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
        public BetTypeOdds(byte id, string matchId, Bookmaker bookmaker, IEnumerable<BetOptionOdds> betOptions)
        {
            Id = id;
            MatchId = matchId;
            Bookmaker = bookmaker;
            BetOptions = betOptions;
        }

        public byte Id { get; }

        public string MatchId { get; }

        public Bookmaker Bookmaker { get; }

        public IEnumerable<BetOptionOdds> BetOptions { get; }
    }
}