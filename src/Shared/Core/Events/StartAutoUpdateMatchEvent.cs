namespace LiveScore.Core.Events
{
    using System;
    using Prism.Events;
    using LiveScore.Core.Services;

    public class StartAutoUpdateMatchEvent : PubSubEvent<IBackgroundJob>
    {
    }
}
