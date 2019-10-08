using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Common.Services;
using LiveScore.Core;
using LiveScore.Core.ViewModels;
using PanCardView.EventArgs;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;

namespace LiveScore.Features.Score.ViewModels
{
    public class ScoresViewModel : ViewModelBase
    {
        private const byte TodayDateBarItemIndex = 3;
        private bool secondLoad;

        public ScoresViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator) : base(navigationService, dependencyResolver, eventAggregator)

        {
            ScoreItemAppearedCommand = new DelegateCommand<ItemAppearedEventArgs>(OnScoreItemAppeared);
            ScoreItemDisappearingCommand = new DelegateCommand<ItemDisappearingEventArgs>(OnScoreItemDisappearing);
            ClickSearchCommand = new DelegateAsyncCommand(OnClickSearchAsync);

            InitScoreItemSources();
        }

        public byte RangeOfDays { get; } = 2;

        public ObservableCollection<ScoreItemViewModel> ScoreItemSources { get; private set; }

        public ScoreItemViewModel SelectedScoreItem { get; set; }

        public int SelectedScoreItemIndex { get; set; }

        public DelegateCommand<ItemAppearedEventArgs> ScoreItemAppearedCommand { get; }

        public DelegateCommand<ItemDisappearingEventArgs> ScoreItemDisappearingCommand { get; }

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
                SelectedScoreItem?.OnResumeWhenNetworkOK();
            }
        }

        public override Task OnNetworkReconnected()
        {
            return SelectedScoreItem?.OnNetworkReconnected();
        }

        public override void OnSleep()
        {
            SelectedScoreItem?.OnSleep();
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            EventAggregator?
                .GetEvent<ConnectionChangePubSubEvent>()
                .Subscribe(OnConnectionChangedBase);

            SelectedScoreItem?.OnAppearing();
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();

            SelectedScoreItem?.OnDisappearing();

            EventAggregator?
                .GetEvent<ConnectionChangePubSubEvent>()
                .Unsubscribe(OnConnectionChangedBase);
        }

        private void OnScoreItemAppeared(ItemAppearedEventArgs args)
        {
            SelectedScoreItem.IsActive = true;

            if (secondLoad)
            {
                SelectedScoreItem.OnAppearing();
            }

            secondLoad = true;
        }

        private void OnScoreItemDisappearing(ItemDisappearingEventArgs args)
        {
            if (args.Index >= 0)
            {
                var oldItemSource = ScoreItemSources[args.Index];

                oldItemSource.IsActive = false;
                oldItemSource.OnDisappearing();
            }
        }

        private void InitScoreItemSources()
        {
            ScoreItemSources = new ObservableCollection<ScoreItemViewModel>
            {
                new LiveItemViewModel(NavigationService, DependencyResolver, EventAggregator)
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

        private void OnConnectionChangedBase(bool isConnected)
        {
            if (isConnected)
            {
                OnNetworkReconnected();
            }
        }
    }
}