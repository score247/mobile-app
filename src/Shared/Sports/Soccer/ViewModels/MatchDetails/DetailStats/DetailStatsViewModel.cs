namespace LiveScore.Soccer.ViewModels.DetailStats
{
    using LiveScore.Core;
    using LiveScore.Core.Controls.TabStrip;
    using LiveScore.Core.Enumerations;
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
            TabHeaderIcon = TabDetailImages.Stats;
            TabHeaderActiveIcon = TabDetailImages.StatsActive;
        }
    }
}