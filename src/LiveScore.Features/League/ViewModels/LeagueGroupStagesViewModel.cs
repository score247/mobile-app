using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common;
using LiveScore.Common.Extensions;
using LiveScore.Core;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.NavigationParams;
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

        public string CountryFlag { get; private set; }

        public string LeagueName { get; private set; }

        public LeagueDetailNavigationParameter LeagueDetail { get; private set; }

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

            if (IsActive)
            {
                return;
            }

            await LoadDataAsync(UpdateLeagueGroupStagesAsync);
            IsActive = true;
        }

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters?["League"] is LeagueDetailNavigationParameter leagueDetail)
            {
                LeagueDetail = leagueDetail;
                LeagueName = leagueDetail.LeagueGroupName.ToUpperInvariant();
            }

            if (parameters?["CountryFlag"] is string countryFlag)
            {
                CountryFlag = countryFlag;
            }
        }

        protected virtual async Task OnRefreshAsync()
        {
            await LoadDataAsync(UpdateLeagueGroupStagesAsync, false);

            IsRefreshing = false;
        }

        protected virtual async Task UpdateLeagueGroupStagesAsync()
        {
            var leagueGroupStages = await leagueService.GetLeagueGroupStages(LeagueDetail.Id, LeagueDetail.SeasonId, CurrentLanguage);

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
                     LeagueDetail,
                     CountryFlag,
                     leagueGroupStage.LeagueRound.Group,
                     leagueGroupStage.GroupStageName,
                     leagueGroupStage.HasStanding));
        }
    }
}