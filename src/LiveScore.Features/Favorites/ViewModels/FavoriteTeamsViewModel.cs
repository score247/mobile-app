using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.Models.Teams;
using LiveScore.Core.Services;
using LiveScore.Features.Favorites.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;

namespace LiveScore.Features.Favorites.ViewModels
{
    public class FavoriteTeamsViewModel : TabItemViewModel
    {
        private readonly IFavoriteService<ITeamProfile> favoriteService;

        public FavoriteTeamsViewModel(

            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(navigationService, dependencyResolver, null, eventAggregator, AppResources.Teams)
        {
            AddFavoriteTeamCommand = new DelegateAsyncCommand(OnAddFavoriteTeam);
            UnFavoriteCommand = new DelegateCommand<ITeamProfile>(OnUnFavoriteTeam);
            TapTeamCommand = new DelegateAsyncCommand<ITeamProfile>(OnTapTeam);
            favoriteService = DependencyResolver.Resolve<IFavoriteService<ITeamProfile>>(CurrentSportId.ToString());
        }

        public DelegateAsyncCommand AddFavoriteTeamCommand { get; }

        public DelegateAsyncCommand<ITeamProfile> TapTeamCommand { get; }

        public DelegateCommand<ITeamProfile> UnFavoriteCommand { get; }

        public ObservableCollection<ITeamProfile> FavoriteTeams { get; private set; }

        public bool? HasHeader { get; private set; } = true;

        public override void OnAppearing()
        {
            base.OnAppearing();

            FavoriteTeams = new ObservableCollection<ITeamProfile>(favoriteService.GetAll());
            HasHeader = FavoriteTeams.Any() ? null : (bool?)true;
        }

        private async Task OnAddFavoriteTeam()
        {
            await NavigationService.NavigateAsync("NavigationPage/SearchTeamsView", useModalNavigation: true, animated: false);
        }

        private void OnUnFavoriteTeam(ITeamProfile team)
        {
            favoriteService.Remove(team);
            FavoriteTeams.Remove(team);

            HasHeader = FavoriteTeams.Any() ? null : (bool?)true;
        }

        private async Task OnTapTeam(ITeamProfile team)
        {
            await NavigationService.NavigateAsync(
                nameof(FavoriteTeamMatchesView),
                new NavigationParameters
                {
                    {"Team", team}
                });
        }
    }
}