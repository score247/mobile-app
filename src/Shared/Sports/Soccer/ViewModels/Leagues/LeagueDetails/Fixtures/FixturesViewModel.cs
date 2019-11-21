using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using Prism.Navigation;

namespace LiveScore.Soccer.ViewModels.Leagues.LeagueDetails.Fixtures
{
    public class FixturesViewModel : TabItemViewModel
    {
        public FixturesViewModel(
             INavigationService navigationService,
             IDependencyResolver serviceLocator)
             : base(navigationService, serviceLocator, null, null, AppResources.Fixtures)
        {
        }
    }
}