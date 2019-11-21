using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using Prism.Navigation;
using Xamarin.Forms;

namespace LiveScore.Soccer.ViewModels.Leagues.LeagueDetails.Table
{
    public class TableViewModel : TabItemViewModel
    {
        public TableViewModel(
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            DataTemplate dataTemplate = null)
            : base(navigationService, serviceLocator, dataTemplate, null, AppResources.Table)
        {
        }
    }
}