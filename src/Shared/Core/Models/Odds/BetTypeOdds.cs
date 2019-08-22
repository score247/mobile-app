namespace LiveScore.Core.Models.Odds
{
    using System.Collections.Generic;

    public interface IBetTypeOdds : IEntity<int, string>
    {
        Bookmaker Bookmaker { get; }

        IEnumerable<BetOptionOdds> BetOptions { get; set; }
    }

    public class BetTypeOdds : Entity<int, string>, IBetTypeOdds
    {
        public Bookmaker Bookmaker { get; set; }

        public IEnumerable<BetOptionOdds> BetOptions { get; set; }
    }
}