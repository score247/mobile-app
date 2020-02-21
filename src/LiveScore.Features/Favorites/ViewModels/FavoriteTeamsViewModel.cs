using System;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using Prism.Events;
using Prism.Navigation;

namespace LiveScore.Features.Favorites.ViewModels
{
    public class FavoriteTeamsViewModel : TabItemViewModel
    {
        public FavoriteTeamsViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(navigationService, dependencyResolver, null, eventAggregator, AppResources.Teams)
        {
            AddFavoriteTeamCommand = new DelegateAsyncCommand(OnAddFavoriteTeam);
        }

        public DelegateAsyncCommand AddFavoriteTeamCommand { get; }

        private async Task OnAddFavoriteTeam()
        {
            await NavigationService.NavigateAsync("NavigationPage/SearchTeamsView" + CurrentSportId, useModalNavigation: true);
        }
    }
}