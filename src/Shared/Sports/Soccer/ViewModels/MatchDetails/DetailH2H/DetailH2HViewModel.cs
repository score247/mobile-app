using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private readonly IMatchDisplayStatusBuilder matchStatusBuilder;
        private readonly IMatchMinuteBuilder matchMinuteBuilder;
        private readonly Func<string, string> buildFlagUrlFunc;

        public DetailH2HViewModel(
            IMatch match,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator,
            DataTemplate dataTemplate)
            : base(navigationService, dependencyResolver, dataTemplate, eventAggregator, AppResources.H2H)
        {
            this.match = match;
            InitData();

            matchStatusBuilder = DependencyResolver.Resolve<IMatchDisplayStatusBuilder>(CurrentSportId.ToString());
            matchMinuteBuilder = DependencyResolver.Resolve<IMatchMinuteBuilder>(CurrentSportId.ToString());
            buildFlagUrlFunc = DependencyResolver.Resolve<Func<string, string>>(FuncNameConstants.BuildFlagUrlFuncName);

            teamService = DependencyResolver.Resolve<ITeamService>(CurrentSportId.ToString());

            OnTeamResultTapped = new DelegateCommand<string>(LoadTeamResult);
            OnHeadToHeadTapped = new DelegateAsyncCommand(() => LoadHeadToHeadAsync(forceFetchLatestData: true));
            RefreshCommand = new DelegateAsyncCommand(RefreshAsync);
        }

        public bool IsRefreshing { get; set; }

        public bool VisibleHomeResults { get; private set; }

        public bool VisibleAwayResults { get; private set; }

        public bool VisibleHeadToHead { get; private set; }

        public bool VisibleStats { get; private set; }

        public string HomeTeamName { get; private set; }

        public string AwayTeamName { get; private set; }

        public H2HStatisticViewModel Stats { get; private set; }

        public DelegateCommand<string> OnTeamResultTapped { get; }

        public DelegateAsyncCommand OnHeadToHeadTapped { get; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public ObservableCollection<H2HMatchGroupViewModel> Matches { get; set; }

        private void InitData()
        {
            HomeTeamName = match.HomeTeamName;
            AwayTeamName = match.AwayTeamName;

            VisibleHeadToHead = true;
            HasData = true;
            Stats = new H2HStatisticViewModel();
        }

        public async override void OnAppearing()
        {
            base.OnAppearing();

            await LoadHeadToHeadAsync(true);
        }

        public async override void OnResumeWhenNetworkOK()
        {
            base.OnResumeWhenNetworkOK();

            await RefreshCommand.ExecuteAsync();
        }

        private async Task RefreshAsync()
        {
            IsRefreshing = true;

            await LoadHeadToHeadAsync(forceFetchLatestData: IsRefreshing);

            IsRefreshing = false;
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
            try
            {
                var headToHeads = await teamService.GetHeadToHeadsAsync(match.HomeTeamId, match.AwayTeamId, CurrentLanguage.DisplayName, forceFetchLatestData);

                if (headToHeads != null)
                {
                    HasData = headToHeads != null && headToHeads.Any();

                    if (HasData)
                    {
                        Debug.WriteLine($"H2H HasData {HasData}");
                        Stats = GenerateStatsViewModel(headToHeads.Where(match => match.EventStatus.IsClosed));

                        Matches = new ObservableCollection<H2HMatchGroupViewModel>(BuildMatchGroups(headToHeads));
                    }
                }
                else
                {
                    HasData = false;
                }
                
                VisibleHeadToHead = true;

                VisibleHomeResults = false;
                VisibleAwayResults = false;
            }
            catch (Exception ex)
            {
                await LoggingService.LogExceptionAsync(ex);
            }
        }

        private IEnumerable<H2HMatchGroupViewModel> BuildMatchGroups(IEnumerable<IMatch> headToHeads)
        {
            var matchGroups = headToHeads
                .OrderByDescending(match => match.EventDate)
                .Select(match => new SummaryMatchViewModel(
                    match,
                    matchStatusBuilder,
                    matchMinuteBuilder
                ))
                .GroupBy(item => new GroupHeaderMatchViewModel(item.Match));

            return
                matchGroups.Select(g => new H2HMatchGroupViewModel(g.ToList(), buildFlagUrlFunc));
        }

        private H2HStatisticViewModel GenerateStatsViewModel(IEnumerable<IMatch> closedMatches)
        {
            VisibleStats = closedMatches != null && closedMatches.Any();

            return VisibleStats
                ? new H2HStatisticViewModel(
                    closedMatches.Count(x => x.WinnerId == match.HomeTeamId),
                    closedMatches.Count(x => x.WinnerId == match.AwayTeamId),
                    closedMatches.Count())
                : null;
        }
    }
}