namespace LiveScore.Core.Models.Odds
{
    using System.Collections.Generic;
    using MessagePack;

    public interface IBetTypeOdds : IEntity<byte, string>
    {
        Bookmaker Bookmaker { get; }

        IEnumerable<BetOptionOdds> BetOptions { get; set; }
    }

    [MessagePackObject(keyAsPropertyName: true)]
    public class BetTypeOdds : IBetTypeOdds
    {
        public byte Id { get; set; }

        public string Name { get; set; }

        public Bookmaker Bookmaker { get; set; }

        public IEnumerable<BetOptionOdds> BetOptions { get; set; }
    }
}