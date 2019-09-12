namespace LiveScore.Features.Score.ViewModels
{
    using System;
    using System.Collections.Generic;
    using Core;
    using Core.Controls.DateBar.EventArgs;
    using LiveScore.Core.ViewModels;
    using PanCardView.EventArgs;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Navigation;

    public class ScoresViewModel : ViewModelBase
    {
        private const byte TodayIndex = 3;
        private bool isFirstLoad = true;

        public ScoresViewModel(INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator) : base(navigationService, dependencyResolver, eventAggregator)

        {
            InitScoreItemSources();
            ScoreItemAppearedCommand = new DelegateCommand<ItemAppearedEventArgs>(OnScoreItemAppeared);
            DateBarItemTapCommand = new DelegateCommand<DateBarItemTappedEventArgs>(OnDateBarItemTapped);
        }

        public byte RangeOfDays { get; } = 2;

        public IReadOnlyList<ScoreItemViewModel> ScoreItemSources { get; private set; }

        public ScoreItemViewModel SelectedScoreItem { get; set; }

        public int SelectedScoreItemIndex { get; set; }

        public DelegateCommand<ItemAppearedEventArgs> ScoreItemAppearedCommand { get; private set; }

        public DelegateCommand<DateBarItemTappedEventArgs> DateBarItemTapCommand { get; private set; }

        public override async void OnResume()
        {
            var todayItem = ScoreItemSources[TodayIndex];

            if (todayItem.SelectedDate != DateTime.Today)
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

        public override void OnAppearing()
        {
            if (isFirstLoad)
            {
                SelectedScoreItem?.OnAppearing();
                isFirstLoad = false;
            }
        }

        public override void OnDisappearing()
        {
            SelectedScoreItem?.OnDisappearing();
        }

        private void OnScoreItemAppeared(ItemAppearedEventArgs args)
        {
            if (!isFirstLoad)
            {
                (args?.Item as ScoreItemViewModel)?.OnAppearing();
            }
        }

        private void OnDateBarItemTapped(DateBarItemTappedEventArgs args)
        {
            SelectedScoreItemIndex = args.Index;
        }

        private void InitScoreItemSources()
        {
            var itemViewModels = new List<ScoreItemViewModel>
            {
                new ScoreItemViewModel(DateTime.Today, NavigationService, DependencyResolver, EventAggregator, isLive: true)
            };

            for (var i = -RangeOfDays; i <= RangeOfDays; i++)
            {
                itemViewModels.Add(
                    new ScoreItemViewModel(DateTime.Today.AddDays(i), NavigationService, DependencyResolver, EventAggregator));
            }

            itemViewModels.Add(
                new ScoreItemViewModel(DateTime.Today, NavigationService, DependencyResolver, EventAggregator, isCalendar: true));

            ScoreItemSources = itemViewModels;
            SelectedScoreItemIndex = TodayIndex;
        }
    }
}