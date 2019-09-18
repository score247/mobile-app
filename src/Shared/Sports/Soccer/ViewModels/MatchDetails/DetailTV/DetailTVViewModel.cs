namespace LiveScore.Soccer.ViewModels.DetailTV
{
    using LiveScore.Core;
    using LiveScore.Core.Controls.TabStrip;
    using Prism.Navigation;
    using Xamarin.Forms;

    internal class DetailTVViewModel : TabItemViewModel
    {
        public DetailTVViewModel(
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            DataTemplate dataTemplate)
            : base(navigationService, serviceLocator, dataTemplate)
        {           
        }
    }
}