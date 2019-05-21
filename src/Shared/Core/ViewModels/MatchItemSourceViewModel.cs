using System.Threading.Tasks;
namespace LiveScore.Core.ViewModels
{
    using LiveScore.Core;
    using LiveScore.Core.Converters;
    using LiveScore.Core.Models.Matches;
    using Microsoft.AspNetCore.SignalR.Client;
    using Prism.Events;
    using Prism.Navigation;

    public class MatchItemSourceViewModel : ViewModelBase
    {
        private readonly IMatchStatusConverter matchStatusConverter;
        private readonly HubConnection matchHubConnection;

        public MatchItemSourceViewModel(
            IMatch match,
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver,
            IEventAggregator eventAggregator,
            HubConnection matchHubConnection)
            : base(navigationService, depdendencyResolver, eventAggregator)
        {
            this.matchHubConnection = matchHubConnection;
            matchStatusConverter = DepdendencyResolver.Resolve<IMatchStatusConverter>(SettingsService.CurrentSportType.Value);
            Match = match;
            ChangeMatchData();
            SubscribeMatchTimeChangeEvent();
        }

        public IMatch Match { get; private set; }

        public string DisplayMatchStatus { get; private set; }

        public void ChangeMatchData()
        {
            DisplayMatchStatus = matchStatusConverter.BuildStatus(Match);
        }

        private void SubscribeMatchTimeChangeEvent()
        {
            matchHubConnection.On<string, string, int>("PushMatchTime", (sportId, matchId, matchTime) =>
            {
                if (sportId == SettingsService.CurrentSportType.Value && Match.Id == matchId)
                {
                    Match.MatchResult.MatchTime = matchTime;
                    ChangeMatchData();
                }
            });
        }
    }
}
