using System.Collections.Generic;
using MessagePack;

namespace LiveScore.Core.Models.Odds
{
    public interface IBetTypeOdds : IEntity<byte, string>
    {
        Bookmaker Bookmaker { get; }

        IEnumerable<BetOptionOdds> BetOptions { get; }
    }

    [MessagePackObject(keyAsPropertyName: true)]
    public class BetTypeOdds : Entity<byte, string>, IBetTypeOdds
    {
        public BetTypeOdds(
            int id,
            string name,
            Bookmaker bookmaker,
            IEnumerable<BetOptionOdds> betOptions)
        {
            Id = (byte)id;
            Name = name;
            Bookmaker = bookmaker;         
            BetOptions = betOptions;
        }

        public Bookmaker Bookmaker { get; }

        public IEnumerable<BetOptionOdds> BetOptions { get; }
    }
}