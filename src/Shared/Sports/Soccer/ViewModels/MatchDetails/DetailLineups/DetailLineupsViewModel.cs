namespace LiveScore.Soccer.ViewModels.DetailLineups
{
    using LiveScore.Core;
    using LiveScore.Core.Controls.TabStrip;
    using LiveScore.Core.Enumerations;
    using Prism.Navigation;
    using Xamarin.Forms;

    internal class DetailLineupsViewModel : TabItemViewModelBase
    {
        public DetailLineupsViewModel(
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            DataTemplate dataTemplate)
            : base(navigationService, serviceLocator, dataTemplate)
        {
            TabHeaderIcon = TabDetailImages.Lineups;
            TabHeaderActiveIcon = TabDetailImages.LineupsActive;
        }
    }
}