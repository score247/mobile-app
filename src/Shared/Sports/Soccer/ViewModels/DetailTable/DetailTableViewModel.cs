namespace LiveScore.Soccer.ViewModels.DetailTable
{
    using LiveScore.Core;
    using LiveScore.Core.Controls.TabStrip;
    using LiveScore.Core.Enumerations;
    using Prism.Navigation;
    using Xamarin.Forms;

    internal class DetailTableViewModel : TabItemViewModelBase
    {
        public DetailTableViewModel(
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            DataTemplate dataTemplate)
            : base(navigationService, serviceLocator, dataTemplate)
        {
            TabHeaderIcon = TabDetailImages.Table;
            TabHeaderActiveIcon = TabDetailImages.TableActive;
        }
    }
}