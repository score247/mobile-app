namespace LiveScore.Core.Models.Matches
{
    using System.Collections.Generic;

    public interface IMatchInfo
    {
        IMatch Match { get; }

        int Attendance { get; }

        Venue Venue { get; }

        string Referee { get; }

        IEnumerable<ITimelineEvent> TimelineEvents { get; }

        void UpdateTimelineEvents(IEnumerable<ITimelineEvent> timelineEvents);
    }
}