namespace Core.ViewModels
{
    using Common.ViewModels;
    using Prism.Navigation;

    public class MatchViewModelBase : ViewModelBase
    {
        private string matchId;

        public string MatchId
        {
            get { return matchId; }
            set { SetProperty(ref matchId, value); }
        }

        public MatchViewModelBase(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}
