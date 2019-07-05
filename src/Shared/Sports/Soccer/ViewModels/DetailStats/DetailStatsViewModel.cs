namespace LiveScore.Soccer.ViewModels.DetailStats
{
    using LiveScore.Core;
    using LiveScore.Core.Controls.TabStrip;
    using Prism.Navigation;
    using Xamarin.Forms;

    internal class DetailStatsViewModel : TabItemViewModelBase
    {
        public DetailStatsViewModel(
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            DataTemplate dataTemplate)
            : base(navigationService, serviceLocator, dataTemplate)
        {
        }
    }
}