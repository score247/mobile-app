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
            this ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> groupMatchViewModels,
            IEnumerable<IGrouping<GroupMatchViewModel, MatchViewModel>> newItems)
        {
            foreach (var item in newItems)
            {
                if (!groupMatchViewModels.Contains(item))
                {
                    groupMatchViewModels.Add(item);
                }
            }
        }

        public static void RemoveItems(
            this ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> groupMatchViewModels,
            IEnumerable<IGrouping<GroupMatchViewModel, MatchViewModel>> newItems)
        {
            foreach (var item in newItems)
            {
                groupMatchViewModels.Remove(item);
            }
        }

        public static void UpdateMatchItems(
                this ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> groupMatchViewModels,
                IEnumerable<IMatch> matches,
                IMatchDisplayStatusBuilder matchStatusBuilder,
                IMatchMinuteBuilder matchMinuteBuilder,
                IEventAggregator eventAggregator,
                Func<string, string> buildFlagUrlFunc)
        {
            if (matches == null || groupMatchViewModels == null)
            {
                return;
            }

            var groupMatchViewModel = groupMatchViewModels
                .SelectMany(group => group)
                .ToList();

            if (groupMatchViewModel == null)
            {
                return;
            }

            foreach (var match in matches.OrderBy(match => match.LeagueOrder))
            {
                var matchViewModel = groupMatchViewModel
                    .Find(viewModel => viewModel.Match.Id == match.Id);

                if (matchViewModel == null)
                {
                    groupMatchViewModels
                        .AddNewMatch(match, matchStatusBuilder, matchMinuteBuilder, eventAggregator, buildFlagUrlFunc);
                }
                else
                {
                    if (match.ModifiedTime > matchViewModel.Match.ModifiedTime)
                    {
                        matchViewModel.UpdateMatch(match);
                    }
                }
            }
        }

        public static void AddNewMatch(
                this ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> groupMatchViewModels,
                IMatch newMatch,
                IMatchDisplayStatusBuilder matchStatusBuilder,
                IMatchMinuteBuilder matchMinuteBuilder,
                IEventAggregator eventAggregator,
                Func<string, string> buildFlagUrlFunc)
        {
            if (groupMatchViewModels == null || newMatch == null)
            {
                return;
            }

            var currentGroupIndex
                = groupMatchViewModels.IndexOf(g => g.Key.LeagueId == newMatch.LeagueId);

            List<MatchViewModel> currentMatchViewModels;

            if (currentGroupIndex >= 0)
            {
                currentMatchViewModels
                    = groupMatchViewModels[currentGroupIndex].ToList();

                currentMatchViewModels
                    .Add(new MatchViewModel(
                        newMatch,
                        matchStatusBuilder,
                        matchMinuteBuilder,
                        eventAggregator));

                groupMatchViewModels[currentGroupIndex]
                    = currentMatchViewModels
                        .OrderBy(m => m.Match.EventDate)
                        .GroupBy(item => new GroupMatchViewModel(item.Match, buildFlagUrlFunc))
                        .FirstOrDefault();
            }
            else
            {
                currentMatchViewModels = new List<MatchViewModel>
                {
                    new MatchViewModel(newMatch, matchStatusBuilder, matchMinuteBuilder, eventAggregator)
                };

                var group = currentMatchViewModels
                    .OrderBy(m => m.Match.EventDate)
                    .GroupBy(item => new GroupMatchViewModel(item.Match, buildFlagUrlFunc))
                    .FirstOrDefault();

                var leagueOrders
                    = groupMatchViewModels
                        .Select(i => i.Key.LeagueOrder)
                        .ToList();

                var newLeagueIndex
                    = CalculateLeagueIndexByOrder(group?.Key.LeagueOrder ?? 0, leagueOrders);

                if (newLeagueIndex >= groupMatchViewModels.Count)
                {
                    groupMatchViewModels.Add(group);
                }
                else
                {
                    groupMatchViewModels.Insert(newLeagueIndex, group);
                }
            }
        }

        public static void RemoveMatches(
            this ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> groupMatchViewModels,
            string[] removedMatchIds,
            Func<string, string> buildFlagUrlFunc)
        {
            if (groupMatchViewModels == null)
            {
                return;
            }

            foreach (var removedMatchId in removedMatchIds)
            {
                var league = groupMatchViewModels
                    .FirstOrDefault(l => l.Any(match => match.Match.Id == removedMatchId));

                if (league == null)
                {
                    continue;
                }

                var leagueMatches = league.ToList();
                leagueMatches.RemoveAll(m => m.Match.Id == removedMatchId);

                if (leagueMatches.Count == 0)
                {
                    groupMatchViewModels.Remove(league);
                    continue;
                }

                var indexOfLeague = groupMatchViewModels.IndexOf(league);

                groupMatchViewModels[indexOfLeague]
                    = leagueMatches
                        .GroupBy(item => new GroupMatchViewModel(item.Match, buildFlagUrlFunc))
                        .FirstOrDefault();
            }
        }

        public static void UpdateMatchItemEvent(
            this ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> groupMatchViewModels,
            IMatchEvent matchEvent)
        {
            var matchItem
                = groupMatchViewModels?
                    .SelectMany(group => group)
                    .FirstOrDefault(m => m.Match != null && m.Match.Id == matchEvent.MatchId);

            if (matchItem?.Match != null)
            {
                matchItem.OnReceivedMatchEvent(matchEvent);
            }
        }

        public static void UpdateMatchItemStatistics(
            this ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>> groupMatchViewModels,
            string matchId,
            bool isHome,
            ITeamStatistic statistic)
        {
            var matchItem
                = groupMatchViewModels?
                    .SelectMany(group => group)
                    .FirstOrDefault(m => m.Match != null && m.Match.Id == matchId);

            matchItem?.OnReceivedTeamStatistic(isHome, statistic);
        }

        private static int CalculateLeagueIndexByOrder(int leagueOrder, IList<int> leagueOrders)
        {
            leagueOrders.Add(leagueOrder);

            return leagueOrders
                .OrderBy(order => order)
                .IndexOf(leagueOrder);
        }
    }
}