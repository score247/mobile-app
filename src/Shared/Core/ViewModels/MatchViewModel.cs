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
            HubConnection matchHubConnection,
            IMatchStatusConverter matchStatusConverter)
            : base(navigationService, depdendencyResolver, eventAggregator)
        {
            this.matchHubConnection = matchHubConnection;
            this.matchStatusConverter = matchStatusConverter;
            Match = match;

            SubscribeMatchTimeChangeEvent();
        }

        public IMatch Match { get; protected set; }

        public string DisplayMatchStatus => matchStatusConverter.BuildStatus(Match);


        private void SubscribeMatchTimeChangeEvent()
        {
            matchHubConnection.On<byte, string, int>("PushMatchTime", (sportId, matchId, matchTime) =>
            {
                if (sportId == CurrentSportId && Match.Id == matchId)
                {
                    Match.MatchResult.MatchTime = matchTime;

                }
            });
        }
    }
}