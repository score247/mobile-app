using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using LiveScore.Core.Converters;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Models.Teams;
using LiveScore.Core.ViewModels;
using Prism.Events;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace LiveScore.Features.Score.Extensions
{
    public static class MatchItemSourceExtension
    {
        public static void UpdateMatchItems(
            this ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> matchItems,
            IEnumerable<IMatch> matches,
            IMatchStatusConverter matchStatusConverter,
            IMatchMinuteConverter matchMinuteConverter,
            IEventAggregator eventAggregator,
            Func<string, string> buildFlagUrlFunc)
        {
            var matchViewModels = matchItems?.SelectMany(g => g).ToList();

            foreach (var match in matches)
            {
                var matchViewModel = matchViewModels?.FirstOrDefault(viewModel => viewModel.Match.Id == match.Id);

                if (matchViewModel == null)
                {
                    matchItems.AddNewMatch(match, matchStatusConverter, matchMinuteConverter, eventAggregator, buildFlagUrlFunc);

                    continue;
                }

                if (match.ModifiedTime > matchViewModel.Match.ModifiedTime)
                {
                    Device.BeginInvokeOnMainThread(() => matchViewModel?.UpdateMatch(match));
                }
            }
        }

        public static void AddNewMatch(
                this ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> matchItems,
                IMatch newMatch,
                IMatchStatusConverter matchStatusConverter,
                IMatchMinuteConverter matchMinuteConverter,
                IEventAggregator eventAggregator,
                Func<string, string> buildFlagUrlFunc)
        {
            var currentGroupIndex = matchItems.IndexOf(g => g.Key.LeagueId == newMatch.LeagueId);
            List<MatchViewModel> currentMatchViewModels;

            if (currentGroupIndex >= 0)
            {
                currentMatchViewModels = matchItems[currentGroupIndex].ToList();

                currentMatchViewModels
                    .Add(new MatchViewModel(newMatch, matchStatusConverter, matchMinuteConverter, eventAggregator));

                var group = currentMatchViewModels
                    .OrderBy(m => m.Match.EventDate)
                    .GroupBy(item => new GroupMatchViewModel(item.Match, buildFlagUrlFunc))
                    .FirstOrDefault();

                Device.BeginInvokeOnMainThread(() => matchItems[currentGroupIndex] = group);
            }
            else
            {
                currentMatchViewModels = new List<MatchViewModel>
                {
                    new MatchViewModel(newMatch, matchStatusConverter, matchMinuteConverter, eventAggregator)
                };

                var group = currentMatchViewModels
                    .OrderBy(m => m.Match.EventDate)
                    .GroupBy(item => new GroupMatchViewModel(item.Match, buildFlagUrlFunc))
                    .FirstOrDefault();

                // TODO: Should fix: This code does not move favorite/major leagues to top
                Device.BeginInvokeOnMainThread(() => matchItems.Add(group));
            }
        }

        public static void UpdateMatchItemEvent(
            this ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> matchItems,
            IMatchEvent matchEvent)
        {
            var matchItem = matchItems?
                .SelectMany(group => group)
                .FirstOrDefault(m => m.Match.Id == matchEvent.MatchId);

            if (matchItem?.Match != null)
            {
                matchItem.OnReceivedMatchEvent(matchEvent);
            }
        }

        public static void UpdateMatchItemStatistics(
            this ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> matchItems,
            string matchId,
            bool isHome,
            ITeamStatistic statistic)
        {
            var matchItem = matchItems?
                .SelectMany(group => group)
                .FirstOrDefault(m => m.Match.Id == matchId);

            matchItem?.OnReceivedTeamStatistic(isHome, statistic);
        }
    }
}