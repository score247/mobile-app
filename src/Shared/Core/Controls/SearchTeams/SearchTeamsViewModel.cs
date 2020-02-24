﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common;
using LiveScore.Common.Extensions;
using LiveScore.Core.Models.Teams;
using LiveScore.Core.Services;
using LiveScore.Core.ViewModels;
using Prism.Navigation;

namespace LiveScore.Core.Controls.SearchTeams
{
    public class SearchTeamsViewModel : ViewModelBase
    {
        private readonly ITeamService teamService;
        private readonly IFavoriteService<ITeamProfile> favoriteService;
        private readonly Func<string, string> buildTeamLogoUrlFunc;
        private IList<ITeamProfile> favoriteTeams;
        private IList<ITeamProfile> trendingTeams;

        public SearchTeamsViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver)
            : base(navigationService, dependencyResolver)
        {
            CancelSearchCommand = new DelegateAsyncCommand(OnCancelSearch);
            SearchTeamCommand = new DelegateAsyncCommand<string>(OnSearchTeam);
            FavoriteCommand = new DelegateAsyncCommand<ITeamProfile>(OnFavoriteTeam);
            buildTeamLogoUrlFunc = DependencyResolver.Resolve<Func<string, string>>(FuncNameConstants.BuildTeamLogoUrlFuncName);
            teamService = DependencyResolver.Resolve<ITeamService>(CurrentSportId.ToString());
            favoriteService = DependencyResolver.Resolve<IFavoriteService<ITeamProfile>>(CurrentSportId.ToString());
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            favoriteTeams = favoriteService.GetAll();

            Task.Run(() => LoadDataAsync(LoadTrendingTeams));
        }

        public bool? ShowTrendingHeader { get; private set; } = true;

        public DelegateAsyncCommand CancelSearchCommand { get; }

        public DelegateAsyncCommand<string> SearchTeamCommand { get; }

        public DelegateAsyncCommand<ITeamProfile> FavoriteCommand { get; }

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

            var team = (await teamService.SearchTeams(CurrentLanguage.DisplayName, text)).ToList();

            BuildTeamItemSource(team);
        }

        private async Task LoadTrendingTeams()
        {
            ShowTrendingHeader = true;

            trendingTeams ??= (await teamService.GetTrendingTeams(CurrentLanguage.DisplayName)).ToList();

            BuildTeamItemSource(trendingTeams);
        }

        private Task OnFavoriteTeam(ITeamProfile team)
        {
            favoriteService.Add(team);
            team.IsFavorite = true;
            return Task.CompletedTask;
        }

        private void BuildTeamItemSource(IEnumerable<ITeamProfile> teams)
        {
            var teamList = teams.ToList();

            teamList.ForEach(team =>
            {
                team.LogoUrl = buildTeamLogoUrlFunc(team.Abbreviation);
                team.IsFavorite = favoriteTeams.Any(favoriteTeam => favoriteTeam.Id == team.Id);
            });

            TeamsItemSource = teamList;
        }
    }
}