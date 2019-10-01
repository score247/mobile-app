using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Core;
using LiveScore.Core.Controls.DateBar.EventArgs;
using LiveScore.Core.PubSubEvents.Matches;
using LiveScore.Core.Services;
using LiveScore.Core.ViewModels;
using PanCardView.EventArgs;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

namespace LiveScore.Features.Score.ViewModels
{
    public class ScoresViewModel : ViewModelBase
    {
        private const byte LiveIndex = 0;
        private const byte TodayDateBarItemIndex = 3;
        private readonly IMatchService matchService;
        private bool secondLoad;

        public ScoresViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator) : base(navigationService, dependencyResolver, eventAggregator)

        {
            matchService = DependencyResolver.Resolve<IMatchService>(CurrentSportId.ToString());
            ScoreItemAppearedCommand = new DelegateCommand<ItemAppearedEventArgs>(OnScoreItemAppeared);
            DateBarItemTapCommand = new DelegateCommand<DateBarItemTappedEventArgs>(OnDateBarItemTapped);
            ClickSearchCommand = new DelegateAsyncCommand(OnClickSearchAsync);

            EventAggregator
                .GetEvent<LiveMatchPubSubEvent>()
                .Subscribe(OnReceivedLiveMatches, true);

            InitScoreItemSources();
            Task.Run(() => GetLiveMatchCount());
        }

        public byte RangeOfDays { get; } = 2;

        public ObservableCollection<ScoreItemViewModel> ScoreItemSources { get; private set; }

        public int LiveMatchCount { get; private set; }

        public ScoreItemViewModel SelectedScoreItem { get; set; }

        public int SelectedScoreItemIndex { get; set; }

        public DelegateCommand<ItemAppearedEventArgs> ScoreItemAppearedCommand { get; }

        public DelegateCommand<DateBarItemTappedEventArgs> DateBarItemTapCommand { get; }

        public DelegateAsyncCommand ClickSearchCommand { get; }

        public override async void OnResumeWhenNetworkOK()
        {
            var todayItem = ScoreItemSources[TodayDateBarItemIndex];

            if (todayItem?.SelectedDate != DateTime.Today)
            {
                await NavigateToHome().ConfigureAwait(false);
            }
            else
            {
                await Task.Run(() => GetLiveMatchCount());
                SelectedScoreItem?.OnResumeWhenNetworkOK();
            }
        }

        public override void OnSleep()
        {
            SelectedScoreItem?.OnSleep();
        }

        public override void OnAppearing()
        {
            if (secondLoad)
            {
                Task.Run(() => GetLiveMatchCount());
                SelectedScoreItem?.OnAppearing();
            }

            secondLoad = true;
        }

        public override void OnDisappearing()
        {
            SelectedScoreItem?.OnDisappearing();
        }

        public override void Destroy()
        {
            EventAggregator
                .GetEvent<LiveMatchPubSubEvent>()
                .Unsubscribe(OnReceivedLiveMatches);
        }

        private void OnScoreItemAppeared(ItemAppearedEventArgs args)
        {
            if (args?.Index == LiveIndex)
            {
                Task.Run(() => GetLiveMatchCount());
            }

            SelectedScoreItem?.OnAppearing();
        }

        private void OnDateBarItemTapped(DateBarItemTappedEventArgs args)
        {
            SelectedScoreItemIndex = args.Index;
        }

        private async Task GetLiveMatchCount(bool getLatestData = true)
        {
            var liveMatchCount = await matchService.GetLiveMatchCount(CurrentLanguage, getLatestData);

            Device.BeginInvokeOnMainThread(() => LiveMatchCount = liveMatchCount);
        }

        private void InitScoreItemSources()
        {
            ScoreItemSources = new ObservableCollection<ScoreItemViewModel>
            {
                new LiveItemViewModel( NavigationService, DependencyResolver, EventAggregator)
            };

            for (var i = -RangeOfDays; i <= RangeOfDays; i++)
            {
                ScoreItemSources.Add(
                    new ScoreItemViewModel(DateTime.Today.AddDays(i), NavigationService, DependencyResolver, EventAggregator));
            }

            ScoreItemSources.Add(
                new CalendarItemViewModel(NavigationService, DependencyResolver, EventAggregator));

            SelectedScoreItemIndex = TodayDateBarItemIndex;
        }

        private async Task OnClickSearchAsync()
        {
            var navigated = await NavigationService
                .NavigateAsync("SearchNavigationPage/SearchView", useModalNavigation: true)
                .ConfigureAwait(false);

            if (!navigated.Success)
            {
                await LoggingService.LogErrorAsync(navigated.Exception).ConfigureAwait(false);
            }
        }

        private void OnReceivedLiveMatches(ILiveMatchMessage message)
        {
            if (message?.SportId != CurrentSportId)
            {
                return;
            }

            LiveMatchCount = message.LiveMatchCount;
        }
    }
}