namespace LiveScore.Core.ViewModels
{
    using LiveScore.Core;
    using LiveScore.Core.Converters;
    using LiveScore.Core.Models.Matches;
    using Microsoft.AspNetCore.SignalR.Client;
    using Prism.Events;
    using Prism.Navigation;

    public class MatchViewModel : ViewModelBase
    {
        private readonly IMatchStatusConverter matchStatusConverter;
        private readonly HubConnection matchHubConnection;

        public MatchViewModel(
            IMatch match,
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver,
            IEventAggregator eventAggregator,
            HubConnection matchHubConnection)
            : base(navigationService, depdendencyResolver, eventAggregator)
        {
            this.matchHubConnection = matchHubConnection;
            matchStatusConverter = DependencyResolver.Resolve<IMatchStatusConverter>(SettingsService.CurrentSportType.Value.ToString());
            Match = match;
            BuildMatchStatus();
            SubscribeMatchTimeChangeEvent();
        }

        public IMatch Match { get; protected set; }

        public string DisplayMatchStatus { get; private set; }

        public void BuildMatchStatus()
        {
            DisplayMatchStatus = matchStatusConverter.BuildStatus(Match);
        }

        private void SubscribeMatchTimeChangeEvent()
        {
            matchHubConnection.On<int, string, int>("PushMatchTime", (sportId, matchId, matchTime) =>
            {
                if (sportId == SettingsService.CurrentSportType.Value && Match.Id == matchId)
                {
                    Match.MatchResult.MatchTime = matchTime;
                    BuildMatchStatus();
                }
            });
        }
    }
}