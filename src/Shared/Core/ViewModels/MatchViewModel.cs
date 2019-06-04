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
        private readonly bool showFullStatus = false;

        public MatchViewModel(
            IMatch match,
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver,
            IEventAggregator eventAggregator,
            HubConnection matchHubConnection,
            bool showFullStatus = false)
            : base(navigationService, depdendencyResolver, eventAggregator)
        {
            this.matchHubConnection = matchHubConnection;
            matchStatusConverter = DepdendencyResolver.Resolve<IMatchStatusConverter>(SettingsService.CurrentSportType.Value);
            Match = match;
            this.showFullStatus = showFullStatus;
            BuildMatchStatus();
            SubscribeMatchTimeChangeEvent();
        }

        public IMatch Match { get; protected set; }

        public string DisplayMatchStatus { get; private set; }

        public void BuildMatchStatus()
        {
            DisplayMatchStatus = matchStatusConverter.BuildStatus(Match, showFullStatus);
        }

        private void SubscribeMatchTimeChangeEvent()
        {
            matchHubConnection.On<string, string, int>("PushMatchTime", (sportId, matchId, matchTime) =>
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
