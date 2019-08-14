namespace LiveScore.Core.Models.Leagues
{
    using LiveScore.Core.Enumerations;

    public interface ILeagueRound
    {
        LeagueRoundType Type { get; }

        string Name { get; }

        int Number { get; }
    }

    public class LeagueRound : ILeagueRound
    {
        public LeagueRoundType Type { get; set; }

        public string Name { get; set; }

        public int Number { get; set; }
    }
}