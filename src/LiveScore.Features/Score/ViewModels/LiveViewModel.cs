using LiveScore.Common.Helpers;
using LiveScore.Core;
using LiveScore.Core.ViewModels;
using Prism.Navigation;

namespace LiveScore.Features.Score.ViewModels
{
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