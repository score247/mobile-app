namespace LiveScore.Core.Models.Leagues
{
    using LiveScore.Core.Enumerations;

    public interface ILeagueRound
    {
        LeagueRoundTypes Type { get; }

        string Name { get; }

        int Number { get; }
    }

    public class LeagueRound : ILeagueRound
    {
        public LeagueRoundTypes Type { get; set; }

        public string Name { get; set; }

        public int Number { get; set; }
    }
}