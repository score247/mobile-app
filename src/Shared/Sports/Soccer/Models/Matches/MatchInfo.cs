namespace LiveScore.Soccer.Models.Matches
{
    using System.Collections.Generic;
    using LiveScore.Core.Models.Matches;
    using MessagePack;

    [MessagePackObject]
    public class MatchInfo
    {
        public MatchInfo(Match match, IEnumerable<TimelineEvent> timelineEvents, Venue venue, string referee, int attendance)
        {
            Match = match;
            TimelineEvents = timelineEvents;
            Venue = venue;
            Referee = referee;
            Attendance = attendance;
        }

        [Key(0)]
        public Match Match { get; private set; }

        [Key(1)]
        public IEnumerable<TimelineEvent> TimelineEvents { get; private set; }

        [Key(2)]
        public Venue Venue { get; private set; }

        [Key(3)]
        public string Referee { get; private set; }

        [Key(4)]
        public int Attendance { get; private set; }

        public void UpdateTimelineEvents(IEnumerable<TimelineEvent> timelineEvents)
        {
            TimelineEvents = timelineEvents;
        }
    }
}