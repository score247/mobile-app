﻿using System.Linq;
using LiveScore.Core;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Odds;
using LiveScore.Soccer.Enumerations;
using LiveScore.Soccer.Extensions;
using Prism.Navigation;

namespace LiveScore.Soccer.ViewModels.MatchDetails.Odds.OddItems.OneXTwo
{
    public class OneXTwoMovementItemViewModel : BaseMovementItemViewModel
    {
        public OneXTwoMovementItemViewModel(
            IOddsMovement oddsMovement,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver)
            : base(BetType.OneXTwo, oddsMovement, navigationService, dependencyResolver)
        {
            BuildHomeOdds();

            BuildDrawOdds();

            BuildAwayOdds();
        }

        public string HomeOdds { get; private set; }

        public string HomeOddsTrend { get; private set; }

        public string DrawOdds { get; private set; }

        public string DrawOddsTrend { get; private set; }

        public string AwayOdds { get; private set; }

        public string AwayOddsTrend { get; private set; }

        private void BuildAwayOdds()
        {
            var awayOdds = OddsMovement.BetOptions?.FirstOrDefault(x => x.Type.Equals(BetOption.Away.DisplayName));

            if (awayOdds == null)
            {
                return;
            }

            AwayOdds = awayOdds.LiveOdds.ToOddsFormat();
            AwayOddsTrend = awayOdds.OddsTrend == null ? OddsTrend.Neutral.DisplayName : awayOdds.OddsTrend.Value.ToString();
        }

        private void BuildDrawOdds()
        {
            var drawOdds = OddsMovement.BetOptions?.FirstOrDefault(x => x.Type.Equals(BetOption.Draw.DisplayName));

            if (drawOdds == null)
            {
                return;
            }

            DrawOdds = drawOdds.LiveOdds.ToOddsFormat();
            DrawOddsTrend = drawOdds.OddsTrend == null ? OddsTrend.Neutral.DisplayName : drawOdds.OddsTrend.Value.ToString();
        }

        private void BuildHomeOdds()
        {
            var homeOdds = OddsMovement.BetOptions?.FirstOrDefault(x => x.Type.Equals(BetOption.Home.DisplayName));

            if (homeOdds == null)
            {
                return;
            }

            HomeOdds = homeOdds.LiveOdds.ToOddsFormat();
            HomeOddsTrend = homeOdds.OddsTrend == null ? OddsTrend.Neutral.DisplayName : homeOdds.OddsTrend.Value.ToString();
        }
    }
}