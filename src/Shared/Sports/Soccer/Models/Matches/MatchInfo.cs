using System.Collections.Generic;
using System.Linq;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;
using MessagePack;

namespace LiveScore.Soccer.Models.Matches
{
    [MessagePackObject]
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

        [Key(0)]
        public SoccerMatch Match { get; private set; }

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

        public IEnumerable<TimelineEvent> FilterPenaltyEvents()
        {
            if (this?.Match == null || TimelineEvents == null)
            {
                return Enumerable.Empty<TimelineEvent>();
            }

            var timelineEvents = TimelineEvents.ToList();

            if (Match.EventStatus.IsClosed)
            {
                timelineEvents.RemoveAll(t => t.Type == EventType.PenaltyShootout && t.IsFirstShoot);

                return timelineEvents;
            }

            if (Match.EventStatus.IsLive && Match.MatchStatus.IsInPenalties)
            {
                var lastEvent = timelineEvents.LastOrDefault();
                timelineEvents.RemoveAll(t => t.IsFirstShoot);

                if (lastEvent?.IsFirstShoot == true)
                {
                    timelineEvents.Add(lastEvent);
                }

                return timelineEvents;
            }

            return timelineEvents;
        }
    }
}