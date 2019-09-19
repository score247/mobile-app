using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using LiveScore.Core;
using LiveScore.Core.Controls.DateBar.EventArgs;
using LiveScore.Common.Extensions;
using LiveScore.Core.ViewModels;
using PanCardView.EventArgs;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;

namespace LiveScore.Features.Score.ViewModels
{
    public class ScoresViewModel : ViewModelBase
    {
        private const byte TodayIndex = 3;

        public ScoresViewModel(INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator) : base(navigationService, dependencyResolver, eventAggregator)

        {
            InitScoreItemSources();
            ScoreItemAppearedCommand = new DelegateCommand<ItemAppearedEventArgs>(OnScoreItemAppeared);
            DateBarItemTapCommand = new DelegateCommand<DateBarItemTappedEventArgs>(OnDateBarItemTapped);
            ClickSearchCommand = new DelegateAsyncCommand(OnClickSearch);
        }

        public byte RangeOfDays { get; } = 2;

        public ObservableCollection<ScoreItemViewModel> ScoreItemSources { get; private set; }

        public ScoreItemViewModel SelectedScoreItem { get; set; }

        public int SelectedScoreItemIndex { get; set; }

        public DelegateCommand<ItemAppearedEventArgs> ScoreItemAppearedCommand { get; private set; }

        public DelegateCommand<DateBarItemTappedEventArgs> DateBarItemTapCommand { get; private set; }

        public DelegateAsyncCommand ClickSearchCommand { get; private set; }

        public override async void OnResume()
        {
            var todayItem = ScoreItemSources[TodayIndex];

            if (todayItem?.SelectedDate != DateTime.Today)
            {
                await NavigateToHome();
            }
            else
            {
                SelectedScoreItem?.OnResume();
            }
        }

        public override void OnSleep()
        {
            SelectedScoreItem?.OnSleep();
        }

        //public override void OnAppearing()
        //{
        //    SelectedScoreItem?.OnAppearing();
        //}

        public override void OnDisappearing()
        {
            SelectedScoreItem?.OnDisappearing();
        }

        private void OnScoreItemAppeared(ItemAppearedEventArgs args)
        {
            SelectedScoreItem?.OnAppearing();
        }

        private void OnDateBarItemTapped(DateBarItemTappedEventArgs args)
        {
            SelectedScoreItemIndex = args.Index;
        }

        private void InitScoreItemSources()
        {
            ScoreItemSources = new ObservableCollection<ScoreItemViewModel>
            {
                new ScoreItemViewModel(DateTime.Today, NavigationService, DependencyResolver, EventAggregator, isLive: true)
            };

            for (var i = -RangeOfDays; i <= RangeOfDays; i++)
            {
                ScoreItemSources.Add(
                    new ScoreItemViewModel(DateTime.Today.AddDays(i), NavigationService, DependencyResolver, EventAggregator));
            }

            ScoreItemSources.Add(
                new ScoreItemViewModel(DateTime.Today, NavigationService, DependencyResolver, EventAggregator, isCalendar: true));

            SelectedScoreItemIndex = TodayIndex;
        }

        private async Task OnClickSearch()
        {
            var navigated = await NavigationService
                .NavigateAsync("SearchNavigationPage/SearchView", useModalNavigation: true)
                .ConfigureAwait(false);

            if (!navigated.Success)
            {
                await LoggingService.LogErrorAsync(navigated.Exception).ConfigureAwait(false);
            }
        }
    }
}
