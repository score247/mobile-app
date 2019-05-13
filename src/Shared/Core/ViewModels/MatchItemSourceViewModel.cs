namespace LiveScore.Core.ViewModels
{
    using LiveScore.Core;
    using LiveScore.Core.Converters;
    using LiveScore.Core.Models.Matches;
    using Prism.Navigation;

    public class MatchItemSourceViewModel : ViewModelBase
    {
        private readonly IMatchStatusConverter matchStatusConverter;

        public MatchItemSourceViewModel(IMatch match, INavigationService navigationService, IDependencyResolver depdendencyResolver)
            : base(navigationService, depdendencyResolver)
        {
            matchStatusConverter = DepdendencyResolver.Resolve<IMatchStatusConverter>(SettingsService.CurrentSportType.Value);
            Match = match;
            ChangeMatchData();
        }

        public IMatch Match { get; private set; }

        public string DisplayMatchStatus { get; private set; }

        public void ChangeMatchData()
        {
            DisplayMatchStatus = matchStatusConverter.BuildStatus(Match);
        }
    }
}
