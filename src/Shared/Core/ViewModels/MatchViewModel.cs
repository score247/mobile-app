namespace LiveScore.Core.ViewModels
{
    using System.Linq;
    using LiveScore.Core.Converters;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Teams;
    using Microsoft.AspNetCore.SignalR.Client;
    using PropertyChanged;

    [AddINotifyPropertyChangedInterface]
    public class MatchViewModel
    {
        private readonly IMatchStatusConverter matchStatusConverter;
        private readonly HubConnection matchHubConnection;
        private readonly byte currentSportId;

        public MatchViewModel(
            IMatch match,
            //HubConnection matchHubConnection,
            IMatchStatusConverter matchStatusConverter,
            byte currentSportId)
        {
            //this.matchHubConnection = matchHubConnection;
            this.matchStatusConverter = matchStatusConverter;
            this.currentSportId = currentSportId;

            Match = match;
            DisplayMatchStatus = matchStatusConverter.BuildStatus(Match);
            SubscribeMatchTimeChangeEvent();
        }

        public IMatch Match { get; protected set; }

        public string DisplayMatchStatus { get; private set; }

        public void OnReceivedMatchEvent(IMatchEvent matchEvent)
        {
            //Match.EventStatus = matchEvent.MatchResult;
            //Match.LatestTimeline = matchEvent.Timeline;
            //DisplayMatchStatus = matchStatusConverter.BuildStatus(Match);
        }

        public void OnReceivedTeamStatistic(bool isHome, ITeamStatistic teamStatistic)
        {
            //var currentTeam = Match.Teams.FirstOrDefault(t => t.IsHome == isHome);
            //currentTeam.Statistic = teamStatistic;
        }

        private void SubscribeMatchTimeChangeEvent()
        {
            //matchHubConnection.On<byte, string, int>("PushMatchTime",
            //    (sportId, matchId, matchTime) =>
            //    {
            //        if (currentSportId == sportId && Match.Id == matchId)
            //        {
            //            Match.MatchResult.MatchTime = matchTime;
            //        }
            //    });
        }
    }
}