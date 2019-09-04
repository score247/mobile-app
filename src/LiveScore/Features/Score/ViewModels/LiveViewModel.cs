namespace LiveScore.Features.Score.ViewModels
{
    using Core.ViewModels;
    using Common.Helpers;
    using Core;
    using Prism.Navigation;

    public class LiveViewModel : ViewModelBase
    {
        public LiveViewModel(INavigationService navigationService, IDependencyResolver serviceLocator)
            : base(navigationService, serviceLocator)
        {
            Profiler.Start(GetType().Name + ".Init");
        }

        public override void OnAppearing()
        {
            Profiler.Stop(GetType().Name + ".Init");
        }
    }
}