namespace LiveScore.Score.ViewModels
{
    using Core.ViewModels;
    using LiveScore.Common.Helpers;
    using LiveScore.Core;
    using Prism.Navigation;

    public class LiveViewModel : ViewModelBase
    {
        public LiveViewModel(INavigationService navigationService, IDependencyResolver serviceLocator)
            : base(navigationService, serviceLocator)
        {
            Profiler.Start(this.GetType().Name + ".Init");
        }

        public override void OnAppearing()
        {
            Profiler.Stop(this.GetType().Name + ".Init");
        }
    }
}