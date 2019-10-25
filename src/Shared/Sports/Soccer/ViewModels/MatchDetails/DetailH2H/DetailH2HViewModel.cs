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

            //TODO apply first load

            OnTeamResultTapped = new DelegateCommand<string>(LoadTeamResult);

            OnHeadToHeadTapped = new DelegateAsyncCommand(() => LoadHeadToHeadAsync(forceFetchLatestData: true));

            RefreshCommand = new DelegateAsyncCommand(
                async () => await LoadDataAsync(() => LoadHeadToHeadAsync(forceFetchLatestData: true), false));
        }

        public bool VisibleHomeResults { get; private set; }

        public bool VisibleAwayResults { get; private set; }

        public bool VisibleHeadToHead { get; private set; }

        public string HomeTeamName { get; private set; }

        public string AwayTeamName { get; private set; }

        public H2HStatistic Stats { get; private set; }

        public DelegateCommand<string> OnTeamResultTapped { get; }

        public DelegateAsyncCommand OnHeadToHeadTapped { get; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public ObservableCollection<IGrouping<GroupHeaderMatchViewModel, SummaryMatchViewModel>> Matches { get; set; }

        public async override void OnAppearing()
        {
            base.OnAppearing();

            await LoadHeadToHeadAsync(true);
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        public override void OnResumeWhenNetworkOK()
        {
            base.OnResumeWhenNetworkOK();
        }

        public override void OnSleep()
        {
            base.OnSleep();
        }

        public override void Destroy()
        {
            base.Destroy();
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
            //var headToHeads = await teamService.GetHeadToHeadsAsync(match.HomeTeamId, match.AwayTeamId, CurrentLanguage.DisplayName, forceFetchLatestData);
            var headToHeads = await teamService.GetHeadToHeadsAsync("sr:competitor:22474", "sr:competitor:22595", CurrentLanguage.DisplayName, forceFetchLatestData);

            if (headToHeads == null)
            {
                return;
            }

            HasData = headToHeads != null && headToHeads.Any();

            if (HasData)
            {
                Stats = CalculateH2HStats(headToHeads.Where(match => match.MatchStatus.IsClosed));

                var matchItemViewModels = headToHeads
                    .OrderByDescending(match => match.EventDate)
                    .Select(match => new SummaryMatchViewModel(
                        match,
                        matchStatusBuilder,
                        matchMinuteBuilder
                    ));

                var matchItems
                    = matchItemViewModels.GroupBy(item => new GroupHeaderMatchViewModel(item.Match, buildFlagUrlFunc));

                Matches = new ObservableCollection<IGrouping<GroupHeaderMatchViewModel, SummaryMatchViewModel>>(matchItems);
            }

            VisibleHeadToHead = true;

            VisibleHomeResults = false;
            VisibleAwayResults = false;
        }

        private H2HStatistic CalculateH2HStats(IEnumerable<IMatch> matches)
        {
            return new H2HStatistic(
                matches.Count(x=>x.WinnerId == match.HomeTeamId),
                matches.Count(x => x.WinnerId == match.AwayTeamId),
                matches.Count());
        }
    }

    public class H2HStatistic
    {
        public H2HStatistic(int homeWin, int awayWin, int total)
        {
            HomeWin = homeWin;
            AwayWin = awayWin;
            Draw = total - homeWin - awayWin;
            Total = total;
        }

        public int HomeWin { get; }

        public int AwayWin { get; }

        public int Draw { get; }

        public int Total { get; }

        public string DisplayHomeWin => $"{HomeWin}/{Total}";

        public string DisplayDraw => $"{Draw}/{Total}";

        public string DisplayAwayWin => $"{AwayWin}/{Total}";
    }
}