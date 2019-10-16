namespace LiveScore.Soccer.ViewModels.DetailSocial
{
    using LiveScore.Core;
    using LiveScore.Core.Controls.TabStrip;
    using Prism.Navigation;
    using Xamarin.Forms;

    internal class DetailSocialViewModel : TabItemViewModel
    {
        public DetailSocialViewModel(
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            DataTemplate dataTemplate)
            : base(navigationService, serviceLocator, dataTemplate, null, 4)
        {
        }
    }
}