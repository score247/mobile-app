namespace LiveScore.Core.Events
{
    using LiveScore.Core.Constants;
    using Prism.Events;

    public class SportChangeEvent : PubSubEvent<SportType>
    {
    }
}
