using System.Collections.ObjectModel;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using PanCardView.EventArgs;
using Prism.Commands;

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
            ScoreItemAppearingCommand = new DelegateCommand<ItemAppearingEventArgs>(OnItemAppearing);
        }

        public byte RangeOfDays { get; } = 2;

        public IReadOnlyList<ScoreItemViewModel> ScoreItemSources { get; private set; }

        public ScoreItemViewModel SelectedScoreItem { get; set; }

        public DelegateCommand<ItemAppearingEventArgs> ScoreItemAppearingCommand { get; private set; }

        public int SelectedScoreItemIndex { get; set; }

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

        private static void OnItemAppearing(ItemAppearingEventArgs args)
        {
            (args?.Item as ScoreItemViewModel)?.OnAppearing();
        }

        private void InitScoreItemSources()
        {
            var itemViewModels = new List<ScoreItemViewModel>();

            for (var i = -RangeOfDays; i <= RangeOfDays; i++)
            {
                itemViewModels.Add(
                    new ScoreItemViewModel(DateTime.Today.AddDays(i), NavigationService, DependencyResolver, EventAggregator));
            }

            ScoreItemSources = itemViewModels;
        }
    }
}