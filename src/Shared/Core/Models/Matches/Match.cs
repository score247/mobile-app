namespace LiveScore.Core.Models.Matches
{
    using System;
    using System.Collections.Generic;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Models.Leagues;
    using LiveScore.Core.Models.Teams;
    using Newtonsoft.Json;
    using PropertyChanged;

    public interface IMatch : IEntity<string, string>
    {
        DateTime EventDate { get; set; }

        IEnumerable<ITeam> Teams { get; set; }

        IMatchResult MatchResult { get; set; }

        IEnumerable<ITimeline> TimeLines { get; set; }

        IMatchCondition MatchCondition { get; set; }

        ILeague League { get; set; }

        int Attendance { get; }

        IVenue Venue { get; }

        string Referee { get; }
    }

    [AddINotifyPropertyChangedInterface]
    public class Match : Entity<string, string>, IMatch
    {
        public DateTime EventDate { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<IEnumerable<Team>>))]
        public IEnumerable<ITeam> Teams { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<MatchResult>))]
        public IMatchResult MatchResult { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<IEnumerable<Timeline>>))]
        public IEnumerable<ITimeline> TimeLines { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<MatchCondition>))]
        public IMatchCondition MatchCondition { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<League>))]
        public ILeague League { get; set; }

        public int Attendance { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<Venue>))]
        public IVenue Venue { get; set; }

        public string Referee { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<Timeline>))]
        public ITimeline LatestTimeline { get; set; }

    }
}