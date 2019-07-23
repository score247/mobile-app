namespace LiveScore.Soccer.ViewModels.DetailSocial
{
    using LiveScore.Core;
    using LiveScore.Core.Controls.TabStrip;
    using LiveScore.Core.Enumerations;
    using Prism.Navigation;
    using Xamarin.Forms;

    internal class DetailSocialViewModel : TabItemViewModelBase
    {
        public DetailSocialViewModel(
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            DataTemplate dataTemplate)
            : base(navigationService, serviceLocator, dataTemplate)
        {
            TabHeaderIcon = TabDetailImages.Social;
            TabHeaderActiveIcon = TabDetailImages.SocialActive;
        }
    }
}