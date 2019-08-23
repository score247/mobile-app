namespace LiveScore.Soccer.ViewModels.DetailTracker
{
    using LiveScore.Core;
    using LiveScore.Core.Controls.TabStrip;
    using LiveScore.Core.Enumerations;
    using Prism.Navigation;
    using Xamarin.Forms;

    internal class DetailTrackerViewModel : TabItemViewModel
    {
        public DetailTrackerViewModel(
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            DataTemplate dataTemplate)
            : base(navigationService, serviceLocator, dataTemplate)
        {
            TabHeaderIcon = TabDetailImages.Tracker;
            TabHeaderActiveIcon = TabDetailImages.TrackerActive;
        }
    }
}