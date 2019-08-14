namespace LiveScore.Soccer.Models.Leagues
{
    using System.Collections.Generic;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Models;
    using LiveScore.Core.Models.Leagues;
    using LiveScore.Core.Models.Teams;
    using LiveScore.Soccer.Models.Teams;
    using Newtonsoft.Json;

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