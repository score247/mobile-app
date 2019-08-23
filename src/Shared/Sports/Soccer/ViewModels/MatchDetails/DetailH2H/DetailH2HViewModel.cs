namespace LiveScore.Soccer.ViewModels.DetailH2H
{
    using LiveScore.Core;
    using LiveScore.Core.Controls.TabStrip;
    using LiveScore.Core.Enumerations;
    using Prism.Navigation;
    using Xamarin.Forms;

    internal class DetailH2HViewModel : TabItemViewModel
    {
        public DetailH2HViewModel(
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            DataTemplate dataTemplate)
            : base(navigationService, serviceLocator, dataTemplate)
        {
            TabHeaderIcon = TabDetailImage.H2H;
            TabHeaderActiveIcon = TabDetailImage.H2HActive;
        }
    }
}