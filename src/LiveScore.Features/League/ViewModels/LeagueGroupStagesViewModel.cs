﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common;
using LiveScore.Common.Extensions;
using LiveScore.Core;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.Services;
using LiveScore.Core.ViewModels;
using LiveScore.Features.League.ViewModels.LeagueItemViewModels;
using Prism.Navigation;

namespace LiveScore.Features.League.ViewModels
{
    public class LeagueGroupStagesViewModel : ViewModelBase
    {
        private readonly Func<string, string> buildFlagFunction;
        private readonly ILeagueService leagueService;
        private bool firstLoad = true;

        public LeagueGroupStagesViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver)
            : base(navigationService, dependencyResolver, false)
        {
            leagueService = DependencyResolver.Resolve<ILeagueService>(CurrentSportId.ToString());
            buildFlagFunction = DependencyResolver.Resolve<Func<string, string>>(FuncNameConstants.BuildFlagUrlFuncName);
            RefreshCommand = new DelegateAsyncCommand(OnRefreshAsync);
        }

        public IEnumerable<LeagueGroupStageViewModel> LeagueGroupStages { get; private set; }

        public string LeagueId { get; private set; }

        public string LeagueSeasonId { get; private set; }

        public string LeagueName { get; private set; }

        public string LeagueFlag { get; private set; }

        public DelegateAsyncCommand RefreshCommand { get; protected set; }

        public bool IsRefreshing { get; set; }

        public override async Task OnNetworkReconnectedAsync()
        {
            await base.OnNetworkReconnectedAsync();
            await LoadDataAsync(UpdateLeagueGroupStagesAsync);
        }

        public override async void OnResumeWhenNetworkOK()
        {
            base.OnResumeWhenNetworkOK();
            await LoadDataAsync(UpdateLeagueGroupStagesAsync);
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            if (!IsActive || !firstLoad)
            {
                return;
            }

            await LoadDataAsync(UpdateLeagueGroupStagesAsync);
            firstLoad = false;
        }

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters?["LeagueId"] is string leagueId)
            {
                LeagueId = leagueId;
            }

            if (parameters?["LeagueSeasonId"] is string leagueSeasonId)
            {
                LeagueSeasonId = leagueSeasonId;
            }

            if (parameters?["LeagueName"] is string leagueName)
            {
                LeagueName = leagueName;
            }

            if (parameters?["LeagueFlag"] is string leagueFlag)
            {
                LeagueFlag = leagueFlag;
            }

            if (parameters?["LeagueGroupStages"] is IEnumerable<ILeagueGroupStage> leagueGroupStages)
            {
                BuildGroupStages(leagueGroupStages);
                firstLoad = true;
            }

            IsActive = true;
        }

        protected virtual async Task OnRefreshAsync()
        {
            await LoadDataAsync(UpdateLeagueGroupStagesAsync, false);

            IsRefreshing = false;
        }

        protected virtual async Task UpdateLeagueGroupStagesAsync()
        {
            var leagueGroupStages = await leagueService.GetLeagueGroupStages(LeagueId, LeagueSeasonId, CurrentLanguage);
            if (leagueGroupStages != null)
            {
                BuildGroupStages(leagueGroupStages);
                HasData = true;
            }

            IsRefreshing = false;
        }

        private void BuildGroupStages(IEnumerable<ILeagueGroupStage> leagueGroupStages)
        {
            LeagueGroupStages = leagueGroupStages?
                 .OrderBy(leagueGroupStage => leagueGroupStage.GroupStageName)
                 .Select(leagueGroupStage => new LeagueGroupStageViewModel(
                     NavigationService,
                     DependencyResolver,
                     buildFlagFunction,
                     LeagueId,
                     LeagueSeasonId,
                     LeagueFlag,
                     leagueGroupStage.LeagueRound.Group,
                     leagueGroupStage.GroupStageName));
        }
    }
}