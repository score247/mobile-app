using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LiveScore.Common;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.Converters;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Services;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

[assembly: InternalsVisibleTo("Soccer.Tests")]

namespace LiveScore.Soccer.ViewModels.MatchDetails.HeadToHead
{
    public class H2HViewModel : TabItemViewModel
    {
        private const string HomeIdentifier = "home";

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

            OnTeamResultTapped = new DelegateAsyncCommand<string>(LoadTeamResult);
            OnHeadToHeadTapped = new DelegateAsyncCommand(() => LoadDataAsync(LoadHeadToHeadAsync));
            RefreshCommand = new DelegateAsyncCommand(RefreshAsync);
        }

        public bool IsRefreshing { get; set; }

        public bool VisibleHomeResults { get; private set; }

        public bool VisibleAwayResults { get; private set; }

        public bool VisibleHeadToHead { get; private set; }

        public bool VisibleStats { get; private set; }

        public string HomeTeamName { get; private set; }

        public string AwayTeamName { get; private set; }

        public string SelectedTeamId { get; private set; }

        public H2HStatisticViewModel Stats { get; private set; }

        public DelegateAsyncCommand<string> OnTeamResultTapped { get; }

        public DelegateAsyncCommand OnHeadToHeadTapped { get; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public ObservableCollection<H2HMatchGroupViewModel> Matches { get; set; }

        private void Initialize()
        {
            HomeTeamName = match.HomeTeamName;
            AwayTeamName = match.AwayTeamName;

            VisibleHeadToHead = true;
            HasData = true;
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            if (IsFirstLoad)
            {
                await LoadDataAsync(LoadHeadToHeadAsync);
            }
        }

        public override async void OnResumeWhenNetworkOK()
        {
            base.OnResumeWhenNetworkOK();

            await LoadDataAsync(LoadHeadToHeadAsync);
        }

        public override Task OnNetworkReconnectedAsync() => LoadDataAsync(LoadHeadToHeadAsync);

        internal async Task RefreshAsync()
        {
            IsRefreshing = true;

            await LoadDataAsync(LoadHeadToHeadAsync, false);

            IsRefreshing = false;
        }

        internal async Task LoadTeamResult(string teamIdentifier)
        {
            VisibleHomeResults = teamIdentifier == HomeIdentifier;
            VisibleAwayResults = !VisibleHomeResults;
            VisibleHeadToHead = false;
            VisibleStats = false;

            var homeTeamId = "sr:competitor:7424";
            var awayTeamId = "sr:competitor:7423";

            var teamResults = Enumerable.Empty<IMatch>();

            if (VisibleHomeResults)
            {
                SelectedTeamId = homeTeamId;
                teamResults = await teamService.GetTeamResultsAsync(homeTeamId, awayTeamId, CurrentLanguage.DisplayName);
            }
            else
            {
                SelectedTeamId = awayTeamId;
                teamResults = await teamService.GetTeamResultsAsync(awayTeamId, homeTeamId, CurrentLanguage.DisplayName);
            }

            Matches = new ObservableCollection<H2HMatchGroupViewModel>(BuildMatchGroups(teamResults));

            
            HasData = true;
        }

        internal async Task LoadHeadToHeadAsync()
        {
            VisibleHeadToHead = true;
            VisibleHomeResults = !VisibleHeadToHead;
            VisibleAwayResults = !VisibleHeadToHead;

            try
            {
                var headToHeads = (await teamService.GetHeadToHeadsAsync(
                        match.HomeTeamId,
                        match.AwayTeamId,
                        CurrentLanguage.DisplayName)
                    .ConfigureAwait(false))
                    ?.Except(new List<IMatch> { match }).ToList();

                if (headToHeads?.Any() == true)
                {
                    Stats = GenerateStatsViewModel(headToHeads.Where(match => match.EventStatus.IsClosed));
                    VisibleStats = Stats?.Total > 0;

                    Matches = new ObservableCollection<H2HMatchGroupViewModel>(BuildMatchGroups(headToHeads));
                }

                HasData = Matches?.Any() == true;
            }
            catch (Exception ex)
            {
                await LoggingService.LogExceptionAsync(ex);

                HasData = false;
            }
        }

        private IEnumerable<H2HMatchGroupViewModel> BuildMatchGroups(IEnumerable<IMatch> headToHeads)
        {
            var matchGroups
                = headToHeads
                    .OrderByDescending(match => match.EventDate)
                    .Select(match => new SummaryMatchViewModel(match, matchStatusBuilder, VisibleHeadToHead, SelectedTeamId))
                    .GroupBy(item => new H2HMatchGrouping(item.Match));

            return matchGroups.Select(group => new H2HMatchGroupViewModel(group.ToList(), buildFlagUrlFunc));
        }

        private H2HStatisticViewModel GenerateStatsViewModel(IEnumerable<IMatch> closedMatches)
        => closedMatches?.Any() == true
                ? new H2HStatisticViewModel(
                    closedMatches.Count(x => x.WinnerId == match.HomeTeamId),
                    closedMatches.Count(x => x.WinnerId == match.AwayTeamId),
                    closedMatches.Count())
                : null;
    }
}