namespace LiveScore.Core.Models.Teams
{
    using System.Collections.Generic;
    using LiveScore.Common.Extensions;
    using Newtonsoft.Json;

    public interface ITeam : IEntity<string, string>
    {
        string Country { get; }

        string CountryCode { get; }

        string Flag { get; }

        bool IsHome { get; }

        //"formation": "4-2-3-1",
        string Formation { get; }

        string Abbreviation { get; }

        ITeamStatistic Statistic { get; }

        ICoach Coach { get; }

        IEnumerable<IPlayer> Players { get; }

        IEnumerable<IPlayer> Substitutions { get; }
    }

    public class Team : Entity<string, string>, ITeam
    {
        public string Country { get; set; }

        public string CountryCode { get; set; }

        public string Flag { get; set; }

        public bool IsHome { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<IEnumerable<Player>>))]
        public IEnumerable<IPlayer> Players { get; set; }

        public ITeamStatistic Statistic { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<Coach>))]
        public ICoach Coach { get; set; }

        public string Formation { get; set; }

        public string Abbreviation { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<IEnumerable<Player>>))]
        public IEnumerable<IPlayer> Substitutions { get; set; }
    }
}