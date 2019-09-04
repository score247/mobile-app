namespace LiveScore.Soccer.Models.Matches
{
    using System.Collections.Generic;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Models.Matches;
    using Newtonsoft.Json;

    public class MatchInfo : IMatchInfo
    {
        public MatchInfo(Match match, IEnumerable<TimelineEvent> timelineEvents, Venue venue, string referee, int attendance)
        {
            Match = match;
            TimelineEvents = timelineEvents;
            Venue = venue;
            Referee = referee;
            Attendance = attendance;
        }

        [JsonConverter(typeof(JsonConcreteTypeConverter<Match>))]
        public IMatch Match { get; private set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<IEnumerable<TimelineEvent>>))]
        public IEnumerable<ITimelineEvent> TimelineEvents { get; private set; }

        public int Attendance { get; private set; }

        public Venue Venue { get; private set; }

        public string Referee { get; private set; }

        public void UpdateTimelineEvents(IEnumerable<ITimelineEvent> timelineEvents)
        {
            TimelineEvents = timelineEvents;
        }
    }
}