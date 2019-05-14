
namespace LiveScore.Core.ViewModels
{
    using LiveScore.Core;
    using LiveScore.Core.Converters;
    using LiveScore.Core.Models.Matches;
    using Prism.Navigation;
    using Prism.Events;
    using LiveScore.Core.Events;
    using LiveScore.Core.Services;

    public class MatchItemSourceViewModel : ViewModelBase
    {
        private readonly IMatchStatusConverter matchStatusConverter;

        public MatchItemSourceViewModel(
            IMatch match,
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver,
            IEventAggregator eventAggregator)
            : base(navigationService, depdendencyResolver, eventAggregator)
        {
            matchStatusConverter = DepdendencyResolver.Resolve<IMatchStatusConverter>(SettingsService.CurrentSportType.Value);
            Match = match;
            ChangeMatchData();
            StartAutoUpdateJob();
            SubscribeMatchUpdateEvent();
        }

        private void StartAutoUpdateJob()
        {
            if (Match.MatchResult.EventStatus.IsLive)
            {
                var matchAutoUpdateJob = DepdendencyResolver.Resolve<IBackgroundJob>("MatchAutoUpdateJob" + SettingsService.CurrentSportType.Value);
                matchAutoUpdateJob.Initialize(Match);
                EventAggregator.GetEvent<StartAutoUpdateMatchEvent>().Publish(matchAutoUpdateJob);
            }
        }

        public IMatch Match { get; private set; }

        public string DisplayMatchStatus { get; private set; }

        public void ChangeMatchData()
        {
            DisplayMatchStatus = matchStatusConverter.BuildStatus(Match);
        }

        private void SubscribeMatchUpdateEvent()
        {
            if (Match.MatchResult.EventStatus.IsLive)
            {
                EventAggregator.GetEvent<MatchUpdateEvent>().Subscribe(OnMatchUpdate);
            }
        }

        private void OnMatchUpdate(IMatch match)
        {
            if (Match.Id == match.Id)
            {
                Match = match;
                ChangeMatchData();
            }
        }
    }
}
