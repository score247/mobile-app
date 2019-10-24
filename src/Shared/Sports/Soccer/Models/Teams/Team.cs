namespace LiveScore.Soccer.Models.Teams
{
    using System.Collections.Generic;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Models;
    using LiveScore.Core.Models.Teams;
    using Newtonsoft.Json;
    using PropertyChanged;

    [AddINotifyPropertyChangedInterface]
    public class Team : Entity<string, string>, ITeam
    {
        public string Country { get; set; }

        public string CountryCode { get; set; }

        public string Flag { get; set; }

        public bool IsHome { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<IEnumerable<Player>>))]
        public IEnumerable<IPlayer> Players { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<TeamStatistic>))]
        public ITeamStatistic Statistic { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<Coach>))]
        public ICoach Coach { get; set; }

        public string Formation { get; set; }

        public string Abbreviation { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<IEnumerable<Player>>))]
        public IEnumerable<IPlayer> Substitutions { get; set; }
    }
}