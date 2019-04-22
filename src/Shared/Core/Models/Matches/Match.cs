namespace LiveScore.Core.Models.Matches
{
    using System;
    using System.Collections.Generic;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Models.Leagues;
    using LiveScore.Core.Models.Teams;
    using Newtonsoft.Json;

    public interface IMatch : IEntity<string, string>
    {
        DateTime EventDate { get; }

        IEnumerable<ITeam> Teams { get; }

        IMatchResult MatchResult { get; }

        ITimeLine TimeLine { get; }

        IMatchCondition MatchCondition { get; }

        ILeague League { get; }

        string DisplayLocalTime { get; }
    }

    public class Match : Entity<string, string>, IMatch
    {
        public DateTime EventDate { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<IEnumerable<Team>>))]
        public IEnumerable<ITeam> Teams { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<MatchResult>))]
        public IMatchResult MatchResult { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<TimeLine>))]
        public ITimeLine TimeLine { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<MatchCondition>))]
        public IMatchCondition MatchCondition { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<League>))]
        public ILeague League { get; set; }

        public string DisplayLocalTime => EventDate.ToString("HH:mm");
    }
}