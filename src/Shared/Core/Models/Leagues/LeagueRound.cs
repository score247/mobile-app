namespace LiveScore.Core.Models.Leagues
{
    using System.Collections.Generic;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using Newtonsoft.Json;

    public interface ILeagueRound
    {
        LeagueRoundTypes Type { get; }

        string Name { get; }

        int Number { get; }

        IEnumerable<IMatch> Matches { get; }
    }

    public class LeagueRound : ILeagueRound
    {
        public LeagueRoundTypes Type { get; set; }

        public string Name { get; set; }

        public int Number { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<Match>))]
        public IEnumerable<IMatch> Matches { get; set; }
    }
}