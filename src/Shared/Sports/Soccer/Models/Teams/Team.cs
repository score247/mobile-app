namespace LiveScore.Soccer.Models.Teams
{
    using System.Collections.Generic;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Models;
    using LiveScore.Core.Models.Teams;
    using MessagePack;
    using Newtonsoft.Json;
    using PropertyChanged;

    [AddINotifyPropertyChangedInterface, MessagePackObject]
    public class Team : ITeam
    {
        public Team(string id, string name) 
        {
            Id = id;
            Name = name;
        }

        [Key(0)]
        public string Id { get; }

        [Key(1)]
        public string Name { get; }

        //public string Country { get; set; }

        //public string CountryCode { get; set; }

        //public string Flag { get; set; }

        //public bool IsHome { get; set; }

       // [JsonConverter(typeof(JsonConcreteTypeConverter<IEnumerable<Player>>))]
        //public IEnumerable<IPlayer> Players { get; set; }

        //[JsonConverter(typeof(JsonConcreteTypeConverter<TeamStatistic>))]
        //public ITeamStatistic Statistic { get; set; }

        //[JsonConverter(typeof(JsonConcreteTypeConverter<Coach>))]
        //public ICoach Coach { get; set; }

        //public string Formation { get; set; }

        //public string Abbreviation { get; set; }

       

        //[JsonConverter(typeof(JsonConcreteTypeConverter<IEnumerable<Player>>))]
        //public IEnumerable<IPlayer> Substitutions { get; set; }
    }
}