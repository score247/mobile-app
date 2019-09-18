namespace LiveScore.Soccer.ViewModels.DetailTracker
{
    using LiveScore.Core;
    using LiveScore.Core.Controls.TabStrip;
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
        }
    }
}