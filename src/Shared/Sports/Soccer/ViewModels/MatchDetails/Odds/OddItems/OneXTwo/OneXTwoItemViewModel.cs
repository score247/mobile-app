﻿using System.Linq;
using LiveScore.Core;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Odds;
using LiveScore.Soccer.Enumerations;
using LiveScore.Soccer.Extensions;
using Prism.Navigation;

namespace LiveScore.Soccer.ViewModels.MatchDetails.Odds.OddItems.OneXTwo
{
    public class OneXTwoItemViewModel : BaseItemViewModel
    {
        public OneXTwoItemViewModel(
            IBetTypeOdds betTypeOdds,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver)
             : base(BetType.OneXTwo, betTypeOdds, navigationService, dependencyResolver)
        {
            BuildHomeOdds();

            BuildDrawOdds();

            BuildAwayOdds();
        }

        public string HomeLiveOdds { get; private set; }

        public string HomeOpeningOdds { get; private set; }

        public string HomeOddsTrend { get; private set; }

        public string AwayLiveOdds { get; private set; }

        public string AwayOpeningOdds { get; private set; }

        public string AwayOddsTrend { get; private set; }

        public string DrawLiveOdds { get; private set; }

        public string DrawOpeningOdds { get; private set; }

        public string DrawOddsTrend { get; private set; }

        public override void UpdateOdds(IBetTypeOdds betTypeOdds)
        {
            base.UpdateOdds(betTypeOdds);

            BuildHomeOdds();

            BuildDrawOdds();

            BuildAwayOdds();
        }

        private void BuildAwayOdds()
        {
            var awayOdds = BetTypeOdds.BetOptions.FirstOrDefault(x => x.Type.Equals(BetOption.Away.DisplayName));

            if (awayOdds == null)
            {
                return;
            }

            AwayOpeningOdds = awayOdds.OpeningOdds.ToOddsFormat();
            AwayLiveOdds = awayOdds.LiveOdds.ToOddsFormat();
            AwayOddsTrend = awayOdds.OddsTrend == null ? OddsTrend.Neutral.DisplayName : awayOdds.OddsTrend.Value.ToString();
        }

        private void BuildDrawOdds()
        {
            var drawOdds = BetTypeOdds.BetOptions?.FirstOrDefault(x => x.Type.Equals(BetOption.Draw.DisplayName));

            if (drawOdds == null)
            {
                return;
            }

            DrawOpeningOdds = drawOdds.OpeningOdds.ToOddsFormat();
            DrawLiveOdds = drawOdds.LiveOdds.ToOddsFormat();
            DrawOddsTrend = drawOdds.OddsTrend == null ? OddsTrend.Neutral.DisplayName : drawOdds.OddsTrend.Value.ToString();
        }

        private void BuildHomeOdds()
        {
            var homeOdds = BetTypeOdds.BetOptions?.FirstOrDefault(x => x.Type.Equals(BetOption.Home.DisplayName));

            if (homeOdds == null)
            {
                return;
            }

            HomeLiveOdds = homeOdds.LiveOdds.ToOddsFormat();
            HomeOpeningOdds = homeOdds.OpeningOdds.ToOddsFormat();
            HomeOddsTrend = homeOdds.OddsTrend == null ? OddsTrend.Neutral.DisplayName : homeOdds.OddsTrend.Value.ToString();
        }
    }
}