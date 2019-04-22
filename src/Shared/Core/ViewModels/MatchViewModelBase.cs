namespace LiveScore.Core.ViewModels
{
    using LiveScore.Core.Factories;
    using Prism.Navigation;

    public class MatchViewModelBase : ViewModelBase
    {
        public MatchViewModelBase(INavigationService navigationService, IDepdendencyResolver serviceLocator)
             : base(navigationService, serviceLocator)
        {
        }

        public string MatchId { get; set; }
    }
}