﻿// <auto-generated>
// Odds functions are disabled
// </auto-generated>
using System;
using System.Collections.Generic;
using LiveScore.Common.Extensions;
using LiveScore.Core;
using LiveScore.Core.Models.Odds;
using LiveScore.Core.ViewModels;
using LiveScore.Soccer.Enumerations;
using LiveScore.Soccer.ViewModels.MatchDetails.Odds.OddItems.AsianHdp;
using LiveScore.Soccer.ViewModels.MatchDetails.Odds.OddItems.OneXTwo;
using LiveScore.Soccer.ViewModels.MatchDetails.Odds.OddItems.OverUnder;
using LiveScore.Soccer.Views.Matches.Templates.MatchDetails.Odds.OddsItems.AsianHdp;
using LiveScore.Soccer.Views.Matches.Templates.MatchDetails.Odds.OddsItems.OneXTwo;
using LiveScore.Soccer.Views.Matches.Templates.MatchDetails.Odds.OddsItems.OverUnder;
using Prism.Navigation;
using Xamarin.Forms;

namespace LiveScore.Soccer.ViewModels.MatchDetails.Odds.OddItems
{
    public class BaseMovementItemViewModel : ViewModelBase
    {
        private static readonly IDictionary<BetType, Type> ViewModelMapper = new Dictionary<BetType, Type>
        {
            { BetType.OneXTwo, typeof(OneXTwoMovementItemViewModel) },
            { BetType.AsianHDP, typeof(AsianHdpMovementItemViewModel) },
            { BetType.OverUnder, typeof(OverUnderMovementItemViewModel) },
        };

        private static readonly IDictionary<BetType, DataTemplate> TemplateMapper = new Dictionary<BetType, DataTemplate>
        {
            { BetType.OneXTwo, new OneXTwoMovementItemTemplate() },
            { BetType.AsianHDP, new AsianHdpMovementItemTemplate() },
            { BetType.OverUnder, new OverUnderMovementItemTemplate() },
        };

        public BaseMovementItemViewModel(
            BetType betType,
            IOddsMovement oddsMovement,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver)
            : base(navigationService, dependencyResolver, false)
        {
            OddsMovement = oddsMovement;
            BetType = betType;

            UpdateTime = oddsMovement.UpdateTime.ToLocalDateAndTime();
            FullUpdateTime = oddsMovement.UpdateTime.ToFullLocalDateAndTime();

            MatchScore = oddsMovement.IsMatchStarted
               ? $"{oddsMovement.HomeScore} - {oddsMovement.AwayScore}"
               : string.Empty;

            MatchTime = oddsMovement.MatchTime;
        }

        public IOddsMovement OddsMovement { get; }

        public BetType BetType { get; }

        public string UpdateTime { get; }

        public string FullUpdateTime { get; }

        public string MatchTime { get; }

        public string MatchScore { get; }

        public DataTemplate CreateTemplate()
        {
            if (TemplateMapper.ContainsKey(BetType))
            {
                return TemplateMapper[BetType];
            }

            return new OneXTwoMovementItemTemplate();
        }

        public BaseMovementItemViewModel CreateInstance()
        {
            if (ViewModelMapper.ContainsKey(BetType))
            {
                return Activator.CreateInstance(
                    ViewModelMapper[BetType],
                    OddsMovement, NavigationService, DependencyResolver) as BaseMovementItemViewModel;
            }

            return new BaseMovementItemViewModel(BetType, OddsMovement, NavigationService, DependencyResolver);
        }
    }
}