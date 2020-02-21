using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common;
using LiveScore.Common.Extensions;
using LiveScore.Core;
using LiveScore.Core.Models.Teams;
using LiveScore.Core.Services;
using LiveScore.Core.ViewModels;
using Prism.Navigation;

namespace LiveScore.Soccer.ViewModels.Teams
{
    public class SearchTeamsViewModel : ViewModelBase
    {
        private readonly ITeamService teamService;
        private readonly Func<string, string> buildTeamLogoUrlFunc;

        public SearchTeamsViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver)
            : base(navigationService, dependencyResolver)
        {
            CancelSearchCommand = new DelegateAsyncCommand(OnCancelSearch);
            SearchTeamCommand = new DelegateAsyncCommand<string>(OnSearchTeam);
            buildTeamLogoUrlFunc = DependencyResolver.Resolve<Func<string, string>>(FuncNameConstants.BuildTeamLogoUrlFuncName);
            teamService = DependencyResolver.Resolve<ITeamService>(CurrentSportId.ToString());
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            Task.Run(() => LoadDataAsync(LoadTrendingTeams));
        }

        public bool? ShowTrendingHeader { get; private set; } = true;

        public DelegateAsyncCommand CancelSearchCommand { get; }

        public DelegateAsyncCommand<string> SearchTeamCommand { get; }

        public IReadOnlyCollection<ITeamProfile> TeamsItemSource { get; private set; }

        public async Task OnCancelSearch()
        {
            await NavigationService.GoBackAsync();
        }

        private async Task OnSearchTeam(string text)
        {
            if (text.Length < 2)
            {
                if (text.Length == 0)
                {
                    await LoadTrendingTeams();
                }

                return;
            }
            ShowTrendingHeader = null;

            var teams = (await teamService.SearchTeams(CurrentLanguage.DisplayName, text)).ToList();

            BuildTeamItemSource(teams);
        }

        private async Task LoadTrendingTeams()
        {
            ShowTrendingHeader = true;

            var teams = (await teamService.GetTrendingTeams(CurrentLanguage.DisplayName)).ToList();

            BuildTeamItemSource(teams);
        }

        private void BuildTeamItemSource(List<ITeamProfile> teams)
        {
            teams.ForEach(team => team.LogoUrl = buildTeamLogoUrlFunc(team.Abbreviation));
            TeamsItemSource = teams;
        }
    }
}