namespace LiveScore.Core.Models.Matches
{
    using LiveScore.Core.Models.Odds;
    using System.Collections.Generic;

    public interface IMatchOdds
    {
        string MatchId { get; }

        IEnumerable<IBetTypeOdds> BetTypeOddsList { get; }
    }

    public class MatchOdds : IMatchOdds
    {
        public string MatchId { get; set; }

        public IEnumerable<IBetTypeOdds> BetTypeOddsList { get; set; }
    }
}
