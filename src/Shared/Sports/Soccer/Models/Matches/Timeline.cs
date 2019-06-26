namespace LiveScore.Soccer.Models.Matches
{
    using System.Collections.Generic;
    using LiveScore.Core.Models.Matches;

    public class TimelineComparer : IEqualityComparer<ITimeline>
    {
        public bool Equals(ITimeline x, ITimeline y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(ITimeline obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}