namespace LiveScore.DomainModels
{
    public interface ILeagueRound
    {
        string Type { get; }

        string Name { get; }

        string Number { get; }

        int CupRoundMatches { get; }

        int CupRoundMatchNumber { get; }
    }

    public class LeagueRound : ILeagueRound
    {
        public string Type { get; set; }

        public string Name { get; set; }

        public string Number { get; set; }

        public int CupRoundMatches { get; set; }

        public int CupRoundMatchNumber { get; set; }
    }
}