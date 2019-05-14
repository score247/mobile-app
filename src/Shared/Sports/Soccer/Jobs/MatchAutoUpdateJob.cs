
namespace LiveScore.Soccer.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using LiveScore.Core.Events;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Services;
    using Prism.Events;
    using Xamarin.Forms;

    public class MatchAutoUpdateJob : IBackgroundJob
    {
        private readonly IEventAggregator eventAggregator;
        private IMatch match;

        public MatchAutoUpdateJob(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public void Initialize(object data)
        {
            match = (IMatch)data;
        }

        public async Task Start(CancellationToken cancellationToken)
        {
            if (match.MatchResult.EventStatus.IsLive)
            {
                for (var matchMinute = match.MatchResult.MatchTimeMinute; matchMinute <= 120; matchMinute++)
                    await Task.Run(async () =>
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        await Task.Delay(1000 * 60);

                        if (!string.IsNullOrEmpty(match.MatchResult.MatchTime))
                        {
                            match.MatchResult.MatchTime = $"{matchMinute}:00";

                            Device.BeginInvokeOnMainThread(() =>
                            {
                                eventAggregator.GetEvent<MatchUpdateEvent>().Publish(match);
                            });
                        }
                    }, cancellationToken);
            }
        }
    }
}
