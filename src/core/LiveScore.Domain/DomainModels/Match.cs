namespace LiveScore.Domain.DomainModels
{
    public interface IMatch
    {
        IEvent Event { get; }

        IMatchResult Status { get; }

        ITimeLine TimeLine { get; }

        IEventCondition EventCondition { get; }
    }

    public class Match : IMatch
    {
        public IEvent Event { get; set; }

        public IMatchResult Status { get; set; }

        public ITimeLine TimeLine { get; set; }

        public IEventCondition EventCondition { get; set; }
    }
}