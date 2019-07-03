namespace LiveScore.Core.Models.Odds
{
    using System.Collections.Generic;
    public interface IBetTypeOdds: IEntity<int, string>
    {
        IBookmaker Bookmaker { get; }

        IEnumerable<IBetOptionOdds> BetOptions { get; set; }
    }

    public class BetTypeOdds : Entity<int, string>, IBetTypeOdds
    {
        public IBookmaker Bookmaker { get; set; }

        public IEnumerable<IBetOptionOdds> BetOptions { get; set; }
    }
}
