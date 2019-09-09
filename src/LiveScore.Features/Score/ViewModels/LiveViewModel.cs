namespace LiveScore.Features.Score.ViewModels
{
    using Core.ViewModels;
    using Core;
    using Prism.Navigation;
    using LiveScore.Common.Helpers;

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