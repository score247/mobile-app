namespace LiveScore.Core.Events
{
    using LiveScore.Core.Services;
    using Prism.Events;

    public class StartAutoUpdateMatchEvent : PubSubEvent<IBackgroundJob>
    {
    }
}