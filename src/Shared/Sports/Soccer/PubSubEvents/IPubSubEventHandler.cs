using Newtonsoft.Json;
using Prism.Events;

namespace LiveScore.Soccer.PubSubEvents
{
    public interface IPubSubEventHandler
    {
        string HubMethod { get; }

        void Handle(string data);
    }

    public abstract class BasePubSubEventHandler<T, U> : IPubSubEventHandler
        where T : class
        where U : EventBase, new()
    {
        protected readonly IEventAggregator eventAggregator;

        protected BasePubSubEventHandler(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public abstract string HubMethod { get; }

        public virtual void Handle(string data)
        {
            var message = JsonConvert.DeserializeObject<T>(data);

            if (message != null)
            {
                var pubSubEvent = eventAggregator.GetEvent<U>();
                
                Publish(message, pubSubEvent);
            }
        }

        protected abstract void Publish(T message, U pubSubEvent);
    }
}