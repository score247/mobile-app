namespace LiveScore.Score.ViewModels
{
    using Core.ViewModels;
    using LiveScore.Core;
    using Prism.Navigation;

    public class LiveViewModel : ViewModelBase
    {
        public LiveViewModel(INavigationService navigationService, IDependencyResolver serviceLocator)
            : base(navigationService, serviceLocator)
        {
        }
    }
}