namespace LiveScore.Features.Score.ViewModels
{
    using System;
    using System.Collections.Generic;
    using Core;
    using LiveScore.Core.ViewModels;
    using Prism.Events;
    using Prism.Navigation;

    public class ScoresViewModel : ViewModelBase
    {
        public ScoresViewModel(INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator) : base(navigationService, dependencyResolver, eventAggregator)

        {
            InitScoreItemSources();
        }

        public byte RangeOfDays { get; } = 2;

        public IList<ScoreItemViewModel> ScoreItemSources { get; private set; }

        public ScoreItemViewModel SelectedScoreItem { get; set; }

        public override void OnResume()
        {
            SelectedScoreItem.OnResume();
        }

        public override void OnSleep()
        {
            SelectedScoreItem.OnSleep();
        }

        public override void OnAppearing()
        {
            SelectedScoreItem.OnAppearing();
        }

        public override void OnDisappearing()
        {
            SelectedScoreItem.OnDisappearing();
        }

        private void InitScoreItemSources()
        {
            ScoreItemSources = new List<ScoreItemViewModel>();

            for (var i = -RangeOfDays; i <= RangeOfDays; i++)
            {
                ScoreItemSources.Add(
                    new ScoreItemViewModel(DateTime.Today.AddDays(i), NavigationService, DependencyResolver, EventAggregator));
            }

            SelectedScoreItem = ScoreItemSources[RangeOfDays];
        }
    }
}