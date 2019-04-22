namespace LiveScore.Core.Models.Leagues
{
    using System.Collections.Generic;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Models;
    using LiveScore.Core.Models.Teams;
    using Newtonsoft.Json;

    public interface ILeague : IEntity<string, string>
    {
        int Order { get; }

        string Flag { get; }

        ILeagueCategory Category { get; }

        IEnumerable<ILeagueRound> Rounds { get; }

        IEnumerable<ITeam> Teams { get; }
    }

    public class League : Entity<string, string>, ILeague
    {
        public int Order { get; set; }

        public string Flag { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<LeagueCategory>))]
        public ILeagueCategory Category { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<IEnumerable<LeagueRound>>))]
        public IEnumerable<ILeagueRound> Rounds { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<IEnumerable<Team>>))]
        public IEnumerable<ITeam> Teams { get; set; }
    }
}