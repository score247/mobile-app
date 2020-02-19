using System.Collections.Generic;
using LiveScore.Core.Models.Matches;
using MessagePack;

namespace LiveScore.Soccer.Models.Matches
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class MatchInfo
    {
        public MatchInfo()
        {
        }

        [SerializationConstructor]
        public MatchInfo(
            SoccerMatch match,
            IEnumerable<TimelineEvent> timelineEvents,
            Venue venue,
            string referee,
            int attendance)
        {
            Match = match;
            TimelineEvents = timelineEvents;
            Venue = venue;
            Referee = referee;
            Attendance = attendance;
        }

        public SoccerMatch Match { get; private set; }

        public IEnumerable<TimelineEvent> TimelineEvents { get; private set; }

        public Venue Venue { get; private set; }

        public string Referee { get; private set; }

        public int Attendance { get; private set; }

        public void UpdateTimelineEvents(IEnumerable<TimelineEvent> timelineEvents)
        {
            TimelineEvents = timelineEvents;
        }
    }
}