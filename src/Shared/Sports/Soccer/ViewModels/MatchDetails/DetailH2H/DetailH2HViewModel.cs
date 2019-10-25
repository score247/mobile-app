using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.Converters;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Services;
using LiveScore.Soccer.ViewModels.MatchDetails.DetailH2H;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

namespace LiveScore.Soccer.ViewModels.DetailH2H
{
    public class DetailH2HViewModel : TabItemViewModel
    {
        private readonly IMatch match;

        private readonly ITeamService teamService;

        protected readonly IMatchDisplayStatusBuilder matchStatusBuilder;
        protected readonly IMatchMinuteBuilder matchMinuteBuilder;
        protected readonly Func<string, string> buildFlagUrlFunc;

        public DetailH2HViewModel(
            IMatch match,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator,
            DataTemplate dataTemplate)
            : base(navigationService, dependencyResolver, dataTemplate, eventAggregator, AppResources.H2H)
        {
            this.match = match;
            HomeTeamName = match.HomeTeamName;
            AwayTeamName = match.AwayTeamName;

            VisibleHeadToHead = true;
            HasData = false;

            matchStatusBuilder = DependencyResolver.Resolve<IMatchDisplayStatusBuilder>(CurrentSportId.ToString());
            matchMinuteBuilder = DependencyResolver.Resolve<IMatchMinuteBuilder>(CurrentSportId.ToString());
            buildFlagUrlFunc = DependencyResolver.Resolve<Func<string, string>>(FuncNameConstants.BuildFlagUrlFuncName);

            teamService = DependencyResolver.Resolve<ITeamService>(CurrentSportId.ToString());

            OnTeamResultTapped = new DelegateCommand<string>(LoadTeamResult);
            OnHeadToHeadTapped = new DelegateAsyncCommand(() => LoadHeadToHeadAsync(forceFetchLatestData: true));
            RefreshCommand = new DelegateAsyncCommand(
                async () => await LoadDataAsync(() => LoadHeadToHeadAsync(forceFetchLatestData: true), false));
        }

        public bool VisibleHomeResults { get; private set; }

        public bool VisibleAwayResults { get; private set; }

        public bool VisibleHeadToHead { get; private set; }

        public bool HasStats { get; private set; }

        public string HomeTeamName { get; private set; }

        public string AwayTeamName { get; private set; }

        public H2HStatisticViewModel Stats { get; private set; }

        public DelegateCommand<string> OnTeamResultTapped { get; }

        public DelegateAsyncCommand OnHeadToHeadTapped { get; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public ObservableCollection<IGrouping<GroupHeaderMatchViewModel, SummaryMatchViewModel>> Matches { get; set; }

        public async override void OnAppearing()
        {
            base.OnAppearing();

            await LoadHeadToHeadAsync(true);
        }

        public async override void OnResumeWhenNetworkOK()
        {
            base.OnResumeWhenNetworkOK();

            await LoadHeadToHeadAsync(true);
        }

        private void LoadTeamResult(string teamIdentifier)
        {
            if (teamIdentifier == "home")
            {
                VisibleHomeResults = true;
                VisibleAwayResults = false;
            }
            else
            {
                VisibleHomeResults = false;
                VisibleAwayResults = true;
            }

            VisibleHeadToHead = false;
        }

        private async Task LoadHeadToHeadAsync(bool forceFetchLatestData = false)
        {
            var headToHeads = await teamService.GetHeadToHeadsAsync(match.HomeTeamId, match.AwayTeamId, CurrentLanguage.DisplayName, forceFetchLatestData);

            if (headToHeads == null)
            {
                return;
            }

            HasData = headToHeads != null && headToHeads.Any();

            if (HasData)
            {
                Stats = GenerateStatsViewModel(headToHeads.Where(match => match.MatchStatus.IsClosed));

                Matches = new ObservableCollection<IGrouping<GroupHeaderMatchViewModel, SummaryMatchViewModel>>(GroupMatches(headToHeads));
            }

            VisibleHeadToHead = true;

            VisibleHomeResults = false;
            VisibleAwayResults = false;
        }

        private IEnumerable<IGrouping<GroupHeaderMatchViewModel, SummaryMatchViewModel>> GroupMatches(IEnumerable<IMatch> headToHeads)
        {
            var matchItemViewModels = headToHeads
                                .OrderByDescending(match => match.EventDate)
                                .Select(match => new SummaryMatchViewModel(
                                    match,
                                    matchStatusBuilder,
                                    matchMinuteBuilder
                                ));

            return matchItemViewModels
                .GroupBy(item => new GroupHeaderMatchViewModel(item.Match, buildFlagUrlFunc));
        }

        private H2HStatisticViewModel GenerateStatsViewModel(IEnumerable<IMatch> matches)
        {
            var closedMatches = matches.Where(match => match.MatchStatus.IsClosed);

            HasStats = closedMatches != null && closedMatches.Any();

            return HasStats
                ? GenerateStatsViewModel(closedMatches)
                : null;
        }

    }
}