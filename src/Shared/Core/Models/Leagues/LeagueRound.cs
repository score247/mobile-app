namespace LiveScore.Core.Models.Leagues
{
    using Enumerations;
    using MessagePack;

    public interface ILeagueRound
    {
        LeagueRoundType Type { get; }

        string Name { get; }

        string Group { get; }

        int Number { get; }
    }

    [MessagePackObject(keyAsPropertyName: true)]
    public class LeagueRound : ILeagueRound
    {
        public LeagueRound(
            LeagueRoundType type,
            string name,
            string group,
            int number)
        {
            Type = type;
            Name = name;
            Group = group;
            Number = number;
        }

        public LeagueRoundType Type { get; set; }

        public string Name { get; set; }

        public string Group { get; }

        public int Number { get; set; }
    }
}