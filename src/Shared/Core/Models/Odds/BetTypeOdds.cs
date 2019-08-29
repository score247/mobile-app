namespace LiveScore.Core.Models.Odds
{
    using System.Collections.Generic;

    public interface IBetTypeOdds : IEntity<byte, string>
    {
        Bookmaker Bookmaker { get; }

        IEnumerable<BetOptionOdds> BetOptions { get; set; }
    }

    public class BetTypeOdds : Entity<byte, string>, IBetTypeOdds
    {
        public Bookmaker Bookmaker { get; set; }

        public IEnumerable<BetOptionOdds> BetOptions { get; set; }
    }
}