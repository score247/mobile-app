using System;
using System.Threading.Tasks;
using LiveScore.Common.LangResources;
using LiveScore.Core.Converters;
using LiveScore.Core.Events;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Models.Teams;
using LiveScore.Core.Services;
using LiveScore.Core.Views;
using MvvmHelpers;
using Prism.Commands;
using Prism.Events;
using PropertyChanged;
using Rg.Plugins.Popup.Services;

namespace LiveScore.Core.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MatchViewModel : BaseViewModel
    {
        private bool isSubscribingTimer;
        private readonly IMatchDisplayStatusBuilder matchDisplayStatusBuilder;
        private readonly IMatchMinuteBuilder matchMinuteBuilder;
        private readonly IEventAggregator eventAggregator;
        private readonly IFavoriteService<IMatch> favoriteService;

        public MatchViewModel(
            IMatch match,
            IMatchDisplayStatusBuilder matchDisplayStatusBuilder,
            IMatchMinuteBuilder matchMinuteBuilder,
            IEventAggregator eventAggregator,
            IFavoriteService<IMatch> favoriteService,
            bool isBusy = false)
        {
            this.matchDisplayStatusBuilder = matchDisplayStatusBuilder;
            this.matchMinuteBuilder = matchMinuteBuilder;
            this.eventAggregator = eventAggregator;
            this.favoriteService = favoriteService;

            IsBusy = isBusy;

            BuildMatch(match);

            EnableFavorite = match.EventDate >= DateTime.Today.AddDays(-2);

            if (EnableFavorite)
            {
                IsFavorite = favoriteService.IsFavorite(match);
                FavoriteCommand = new DelegateCommand(OnFavorite);
            }
        }

        public IMatch Match { get; private set; }

        public string DisplayMatchStatus { get; private set; }

        public bool IsFavorite { get; set; }

        public bool EnableFavorite { get; set; }

        public bool DisableFavorite => !EnableFavorite;

        public DelegateCommand FavoriteCommand { get; }

        public void BuildMatch(IMatch match)
        {
            Match = match;
            BuildDisplayMatchStatus();
        }

        public void UpdateMatch(IMatch match)
        {
            Match.UpdateMatch(match);
            BuildDisplayMatchStatus();
        }

        public void OnReceivedTeamStatistic(bool isHome, ITeamStatistic teamStatistic)
        {
            Match.UpdateTeamStatistic(teamStatistic, isHome);
        }

        public void OnReceivedMatchEvent(IMatchEvent matchEvent)
        {
            Match.UpdateLastTimeline(matchEvent.Timeline);
            Match.UpdateResult(matchEvent.MatchResult);

            if (matchEvent.Timeline?.Type.IsPeriodStart == true)
            {
                Match.CurrentPeriodStartTime = matchEvent.Timeline.Time;
            }

            BuildDisplayMatchStatus();
        }

        private void BuildDisplayMatchStatus()
        {
            var matchStatus = matchDisplayStatusBuilder.BuildDisplayStatus(Match);

            if (!string.IsNullOrWhiteSpace(matchStatus))
            {
                DisplayMatchStatus = matchStatus;
                UnsubscribeMatchTimeChangeEvent();
            }
            else
            {
                BuildMatchTime();
                SubscribeMatchTimeChangeEvent();
            }
        }

        private void BuildMatchTime()
        {
            DisplayMatchStatus = matchMinuteBuilder.BuildMatchMinute(Match);
        }

        private void SubscribeMatchTimeChangeEvent()
        {
            if (Match?.EventStatus.IsLive != true || isSubscribingTimer)
            {
                return;
            }

            eventAggregator.GetEvent<OneMinuteTimerCountUpEvent>().Subscribe(BuildMatchTime);
            isSubscribingTimer = true;
        }

        public void UnsubscribeMatchTimeChangeEvent()
        {
            eventAggregator.GetEvent<OneMinuteTimerCountUpEvent>().Unsubscribe(BuildMatchTime);
            isSubscribingTimer = false;
        }

        public void RecheckFavorite()
        {
            IsFavorite = favoriteService.IsFavorite(Match);
        }

        private void OnFavorite()
        {
            IsFavorite = !IsFavorite;

            if (IsFavorite)
            {
                favoriteService.Add(Match);
            }
            else
            {
                favoriteService.Remove(Match);
            }
        }
    }
}