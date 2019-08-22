namespace LiveScore.Core.ViewModels
{
    using LiveScore.Core.Converters;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Teams;
    using PropertyChanged;

    [AddINotifyPropertyChangedInterface]
    public class MatchViewModel
    {
        private readonly IMatchStatusConverter matchStatusConverter;

        public MatchViewModel(IMatch match, IDependencyResolver dependencyResolver, byte currentSportId)
        {
            Match = match;
            matchStatusConverter = dependencyResolver.Resolve<IMatchStatusConverter>(currentSportId.ToString());
            DisplayMatchStatus = matchStatusConverter.BuildStatus(Match);
        }

        public IMatch Match { get; protected set; }

        public string DisplayMatchStatus { get; private set; }

        public void OnReceivedMatchEvent(IMatchEvent matchEvent)
        {
            Match.UpdateResult(matchEvent.MatchResult);
            Match.UpdateLastTimeline(matchEvent.Timeline);
            DisplayMatchStatus = matchStatusConverter.BuildStatus(Match);
        }

        public void OnReceivedTeamStatistic(bool isHome, ITeamStatistic teamStatistic)
        {
            Match.UpdateTeamStatistic(teamStatistic, isHome);
        }
    }
}