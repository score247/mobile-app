using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public static void AddItems(
            this ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> matchItems,
            ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> newItems)
        {
            foreach (var item in newItems)
            {
                matchItems.Add(item);
            }
        }

        public static void RemoveItems(
            this ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> matchItems,
            ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> newItems)
        {
            foreach (var item in newItems)
            {
                matchItems.Remove(item);
            }
        }

        public static void UpdateMatchItems(
                this ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> matchItems,
                IEnumerable<IMatch> matches,
                IMatchDisplayStatusBuilder matchStatusConverter,
                IMatchMinuteBuilder matchMinuteConverter,
                IEventAggregator eventAggregator,
                Func<string, string> buildFlagUrlFunc)
        {
            var matchViewModels = matchItems?.SelectMany(g => g).ToList();
            var orderedMatches = matches.OrderBy(m => m.LeagueOrder);

            foreach (var match in orderedMatches)
            {
                var matchViewModel = matchViewModels?.FirstOrDefault(viewModel => viewModel.Match.Id == match.Id);

                if (matchViewModel == null)
                {
                    matchItems.AddNewMatch(match, matchStatusConverter, matchMinuteConverter, eventAggregator, buildFlagUrlFunc);

                    continue;
                }

                if (match.ModifiedTime > matchViewModel.Match.ModifiedTime)
                {
                    matchViewModel.UpdateMatch(match);
                }
            }
        }

        public static void AddNewMatch(
                this ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> matchItems,
                IMatch newMatch,
                IMatchDisplayStatusBuilder matchStatusConverter,
                IMatchMinuteBuilder matchMinuteConverter,
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

                matchItems[currentGroupIndex] = group;
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

                var leagueOrders = matchItems.Select(i => i.Key.LeagueOrder).ToList();
                var newLeagueIndex = CalculateLeagueIndexByOrder(group?.Key.LeagueOrder ?? 0, leagueOrders);

                if (newLeagueIndex >= matchItems.Count)
                {
                    matchItems.Add(group);
                }
                else
                {
                    matchItems.Insert(newLeagueIndex, group);
                }
            }
        }

        public static void RemoveMatches(
            this ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> matchItems,
            string[] removedMatchIds,
            Func<string, string> buildFlagUrlFunc)
        {
            foreach (var removedMatchId in removedMatchIds)
            {
                var league = matchItems
                    .FirstOrDefault(l => l.Any(match => match.Match.Id == removedMatchId));

                if (league == null)
                {
                    continue;
                }

                var leagueMatches = league.ToList();
                leagueMatches.RemoveAll(m => m.Match.Id == removedMatchId);

                if (leagueMatches.Count == 0)
                {
                    matchItems.Remove(league);
                    continue;
                }

                var indexOfLeague = matchItems.IndexOf(league);
                matchItems[indexOfLeague] = leagueMatches
                    .GroupBy(item => new GroupMatchViewModel(item.Match, buildFlagUrlFunc))
                    .FirstOrDefault();
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

        private static int CalculateLeagueIndexByOrder(int leagueOrder, IList<int> leagueOrders)
        {
            leagueOrders.Add(leagueOrder);

            return leagueOrders.OrderBy(order => order).IndexOf(leagueOrder);
        }
    }
}