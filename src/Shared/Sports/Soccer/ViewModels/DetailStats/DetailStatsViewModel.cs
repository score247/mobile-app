

namespace LiveScore.Soccer.ViewModels.DetailStats
{
    using LiveScore.Core;
    using LiveScore.Core.ViewModels;
    using Prism.Navigation;

    internal class DetailStatsViewModel : ViewModelBase
    {
        public DetailStatsViewModel(
            INavigationService navigationService,
            IDependencyResolver serviceLocator)
            : base(navigationService, serviceLocator)
        {
           
        }
    }
}
