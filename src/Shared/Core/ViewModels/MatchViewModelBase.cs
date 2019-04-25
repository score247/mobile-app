namespace LiveScore.Core.ViewModels
{
    using Prism.Navigation;

    public class MatchViewModelBase : ViewModelBase
    {
        public MatchViewModelBase(INavigationService navigationService, IDependencyResolver serviceLocator)
             : base(navigationService, serviceLocator)
        {
        }

        public string MatchId { get; set; }
    }
}