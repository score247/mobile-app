namespace LiveScore.Soccer.Models.Matches
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models;
    using LiveScore.Core.Models.Leagues;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Teams;
    using LiveScore.Soccer.Extensions;
    using Newtonsoft.Json;
    using PropertyChanged;

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

        public string HomePenaltyImage
            => MatchResult.EventStatus.IsClosed
                    && MatchResult.GetPenaltyResult() != null
                    && Teams.FirstOrDefault()?.Id == MatchResult.WinnerId ?
                Images.PenaltyWinner.Value : string.Empty;

        public string AwayPenaltyImage
             => MatchResult.EventStatus.IsClosed
                    && MatchResult.GetPenaltyResult() != null
                    && Teams.LastOrDefault()?.Id == MatchResult.WinnerId ?
                Images.PenaltyWinner.Value : string.Empty;

        public string HomeSecondLegImage
              => MatchResult.EventStatus.IsClosed
                    && (!string.IsNullOrEmpty(MatchResult.AggregateWinnerId)
                    && Teams.FirstOrDefault()?.Id == MatchResult.WinnerId) ?
                Images.SecondLeg.Value : string.Empty;

        public string AwaySecondLegImage
               => MatchResult.EventStatus.IsClosed
                    && (!string.IsNullOrEmpty(MatchResult.AggregateWinnerId)
                    && Teams.LastOrDefault()?.Id == MatchResult.WinnerId) ?
                Images.SecondLeg.Value : string.Empty;
    }
}