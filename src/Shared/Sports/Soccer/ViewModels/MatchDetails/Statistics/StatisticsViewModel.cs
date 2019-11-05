﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.Enumerations;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Services;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

namespace LiveScore.Soccer.ViewModels.MatchDetails.Statistics
{
    public class StatisticsViewModel : TabItemViewModel
    {
        private readonly ISoccerMatchService soccerMatchService;
        private readonly string matchId;

        public StatisticsViewModel(
            string matchId,
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            IEventAggregator eventAggregator,
            DataTemplate dataTemplate)
            : base(navigationService, serviceLocator, dataTemplate, eventAggregator, AppResources.Stats)
        {
            this.matchId = matchId;
            soccerMatchService = DependencyResolver.Resolve<ISoccerMatchService>();
            RefreshCommand = new DelegateAsyncCommand(
                async () => await LoadDataAsync(LoadStatisticsAsync, false));
        }

        public DelegateAsyncCommand RefreshCommand { get; }

        public MatchStatisticItem MainStatistic { get; private set; }

        public IReadOnlyList<MatchStatisticItem> SubStatisticItems { get; private set; }

        public bool IsRefreshing { get; set; }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            await LoadDataAsync(LoadStatisticsAsync);
        }

        public override async void OnResumeWhenNetworkOK()
        {
            await LoadDataAsync(LoadStatisticsAsync);
        }

        public override Task OnNetworkReconnectedAsync() => LoadDataAsync(LoadStatisticsAsync);

        private async Task LoadStatisticsAsync()
        {
            var matchStatistic = await soccerMatchService
                    .GetMatchStatisticAsync(matchId, Language.English)
                    .ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(matchStatistic?.MatchId))
            {
                MainStatistic = matchStatistic.GetMainStatistic();
                SubStatisticItems = matchStatistic.GetSubStatisticItems().ToList();
                HasData = MainStatistic.IsVisibled || SubStatisticItems.Count > 0;
            }
            else
            {
                HasData = false;
            }

            IsRefreshing = false;
        }
    }
}