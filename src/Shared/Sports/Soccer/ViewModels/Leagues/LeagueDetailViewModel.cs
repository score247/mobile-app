using System.Collections.Generic;
using LiveScore.Core;
using LiveScore.Core.ViewModels;
using LiveScore.Soccer.ViewModels.Leagues.LeagueDetails.Fixtures;
using LiveScore.Soccer.ViewModels.Leagues.LeagueDetails.Table;
using Prism.Events;
using Prism.Navigation;

namespace LiveScore.Soccer.ViewModels.Leagues
{
    public class LeagueDetailViewModel : ViewModelBase
    {
        public LeagueDetailViewModel(
         INavigationService navigationService,
         IDependencyResolver dependencyResolver,
         IEventAggregator eventAggregator)
         : base(navigationService, dependencyResolver, eventAggregator)
        {
            LeagueDetailItemSources = new List<object> {
                new TableViewModel(navigationService, dependencyResolver),
                new FixturesViewModel(navigationService, dependencyResolver)
            };
        }

        public IReadOnlyList<object> LeagueDetailItemSources { get; }
    }
}