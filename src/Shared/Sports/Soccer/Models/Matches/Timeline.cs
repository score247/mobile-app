namespace LiveScore.Soccer.Models.Matches
{
    using System.Collections.Generic;
    using LiveScore.Core.Models.Matches;

    public class TimelineComparer : IEqualityComparer<ITimelineEvent>
    {
        public bool Equals(ITimelineEvent x, ITimelineEvent y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(ITimelineEvent obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}