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
    public class H2HViewModel : TabItemViewModel
    {
        private readonly IMatch match;
        private readonly ITeamService teamService;
        private readonly IMatchDisplayStatusBuilder matchStatusBuilder;
        private readonly Func<string, string> buildFlagUrlFunc;

        public H2HViewModel(
            IMatch match,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator,
            DataTemplate dataTemplate)
            : base(navigationService, dependencyResolver, dataTemplate, eventAggregator, AppResources.H2H)
        {
            this.match = match;
            Initialize();

            matchStatusBuilder = DependencyResolver.Resolve<IMatchDisplayStatusBuilder>(CurrentSportId.ToString());
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

        private void Initialize()
        {
            HomeTeamName = match.HomeTeamName;
            AwayTeamName = match.AwayTeamName;

            VisibleHeadToHead = true;
            HasData = true;
            Stats = new H2HStatisticViewModel();
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            await LoadHeadToHeadAsync(true);
        }

        public override async void OnResumeWhenNetworkOK()
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
            HasData = true;
        }

        private async Task LoadHeadToHeadAsync(bool forceFetchLatestData = false)
        {
            try
            {
                var headToHeads = await teamService.GetHeadToHeadsAsync(
                    match.HomeTeamId, match.AwayTeamId, CurrentLanguage.DisplayName, forceFetchLatestData);

                if (headToHeads != null && headToHeads.Any() && Matches == null)
                {
                    Stats = GenerateStatsViewModel(headToHeads.Where(match => match.EventStatus.IsClosed));

                    Matches = new ObservableCollection<H2HMatchGroupViewModel>(BuildMatchGroups(headToHeads));
                }

                HasData = Matches.Any();
            }
            catch (Exception ex)
            {
                await LoggingService.LogExceptionAsync(ex);

                HasData = false;
            }

            VisibleHeadToHead = true;

            VisibleHomeResults = false;
            VisibleAwayResults = false;
        }

        private IEnumerable<H2HMatchGroupViewModel> BuildMatchGroups(IEnumerable<IMatch> headToHeads)
        {
            var matchGroups = headToHeads
                .OrderByDescending(m => m.EventDate)
                .Select(m => new SummaryMatchViewModel(
                    match,
                    matchStatusBuilder
                ))
                .GroupBy(item => new H2HMatchGrouping(item.Match));

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