namespace LiveScore.Core.ViewModels
{
    using LiveScore.Core.Converters;
    using LiveScore.Core.Models.Matches;
    using Microsoft.AspNetCore.SignalR.Client;

    public class MatchViewModel 
    {
        private readonly IMatchStatusConverter matchStatusConverter;
        private readonly HubConnection matchHubConnection;
        private readonly byte currentSportId;

        public MatchViewModel(
            IMatch match,
            HubConnection matchHubConnection,
            IMatchStatusConverter matchStatusConverter,
            byte currentSportId)
        {
            this.matchHubConnection = matchHubConnection;
            this.matchStatusConverter = matchStatusConverter;
            this.currentSportId = currentSportId;

            Match = match;            
            SubscribeMatchTimeChangeEvent();
        }

        public IMatch Match { get; protected set; }

        public string DisplayMatchStatus => matchStatusConverter.BuildStatus(Match);

        private void SubscribeMatchTimeChangeEvent()
        {
            matchHubConnection.On<byte, string, int>("PushMatchTime", 
                (sportId, matchId, matchTime) =>
                {
                    if (currentSportId == sportId && Match.Id == matchId)
                    {
                        Match.MatchResult.MatchTime = matchTime;
                    }
                });
        }
    }
}