using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.Converters;
using LiveScore.Core.Services;
using LiveScore.Core.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

namespace LiveScore.Soccer.ViewModels.DetailH2H
{
    public class DetailH2HViewModel : TabItemViewModel
    {
        private readonly string matchId;
        private readonly string homeTeamId;
        private readonly string awayTeamId;

        private readonly ITeamService teamService;

        protected readonly IMatchDisplayStatusBuilder matchStatusConverter;
        protected readonly IMatchMinuteBuilder matchMinuteConverter; //TODO remove or pass null
        protected readonly Func<string, string> buildFlagUrlFunc;

        public DetailH2HViewModel(
            string matchId,
            string homeTeamId,
            string awayTeamId,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator,
            DataTemplate dataTemplate)
            : base(navigationService, dependencyResolver, dataTemplate, eventAggregator, AppResources.H2H)
        {
            this.matchId = matchId;
            this.homeTeamId = homeTeamId;
            this.awayTeamId = awayTeamId;
            VisibleHeadToHead = true;

            matchStatusConverter = DependencyResolver.Resolve<IMatchDisplayStatusBuilder>(CurrentSportId.ToString());
            matchMinuteConverter = DependencyResolver.Resolve<IMatchMinuteBuilder>(CurrentSportId.ToString());
            buildFlagUrlFunc = DependencyResolver.Resolve<Func<string, string>>(FuncNameConstants.BuildFlagUrlFuncName);

            teamService = DependencyResolver.Resolve<ITeamService>(CurrentSportId.ToString());

            //TODO apply first load

            OnTeamResultTapped = new DelegateCommand<string>(LoadTeamResult);

            OnHeadToHeadTapped = new DelegateAsyncCommand(() => LoadHeadToHead(forceFetchLatestData: true));
            RefreshCommand = new DelegateAsyncCommand(() => LoadHeadToHead(forceFetchLatestData: true));
        }

        public bool VisibleHomeResults { get; private set; }

        public bool VisibleAwayResults { get; private set; }

        public bool VisibleHeadToHead { get; private set; }

        public DelegateCommand<string> OnTeamResultTapped { get; }

        public DelegateAsyncCommand OnHeadToHeadTapped { get; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> Matches { get; set; }

        public override void OnAppearing()
        {
            base.OnAppearing();
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

        private async Task LoadHeadToHead(bool forceFetchLatestData = false)
        {
            var headToHeads = await teamService.GetHeadToHeadsAsync("sr:competitor:22474", "sr:competitor:22595", CurrentLanguage.DisplayName, forceFetchLatestData);

            if (headToHeads == null)
            {
                return;
            }

            var matchItemViewModels = headToHeads.Select(match => new MatchViewModel(
                match,
                matchStatusConverter,
                matchMinuteConverter, //TODO can pass null?
                EventAggregator));

            //TODO group by league, season
            //TODO mapping season model
            //TODO sort by event date and league order
            var matchItems
                = matchItemViewModels.GroupBy(item => new GroupMatchViewModel(item.Match, buildFlagUrlFunc));

            Matches = new ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>>(matchItems);

            VisibleHeadToHead = true;

            VisibleHomeResults = false;
            VisibleAwayResults = false;
        }
    }
}