namespace LiveScore.Soccer.ViewModels.DetailH2H
{
    using LiveScore.Core;
    using LiveScore.Core.Controls.TabStrip;
    using LiveScore.Core.Enumerations;
    using Prism.Navigation;
    using Xamarin.Forms;

    internal class DetailH2HViewModel : TabItemViewModelBase
    {
        public DetailH2HViewModel(
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            DataTemplate dataTemplate)
            : base(navigationService, serviceLocator, dataTemplate)
        {
            TabHeaderIcon = TabDetailImages.H2H;
            TabHeaderActiveIcon = TabDetailImages.H2HActive;
        }
    }
}