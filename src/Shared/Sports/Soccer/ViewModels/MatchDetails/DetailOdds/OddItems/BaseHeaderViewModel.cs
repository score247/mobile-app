﻿using LiveScore.Soccer.Views.Templates.MatchDetails.Odds.OddsItems;
using LiveScore.Soccer.Views.Templates.MatchDetails.Odds.OddsItems.AsianHdp;
using LiveScore.Soccer.Views.Templates.MatchDetails.Odds.OddsItems.OneXTwo;
using LiveScore.Soccer.Views.Templates.MatchDetails.Odds.OddsItems.OverUnder;

namespace LiveScore.Soccer.ViewModels.MatchDetails.DetailOdds.OddItems
{
    using System.Collections.Generic;
    using Core;
    using Enumerations;
    using LiveScore.Core.ViewModels;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class BaseHeaderViewModel : ViewModelBase
    {
        private static readonly IDictionary<BetType, DataTemplate> TemplateMapper = new Dictionary<BetType, DataTemplate>
        {
            { BetType.OneXTwo, new OneXTwoHeaderTemplate() },
            { BetType.AsianHDP, new AsianHdpHeaderTemplate() },
            { BetType.OverUnder, new OverUnderHeaderTemplate() },
        };

        private readonly bool hasData;

        public BaseHeaderViewModel(
            BetType betType,
            bool hasData,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver)
            : base(navigationService, dependencyResolver, false)
        {
            BetType = betType;
            this.hasData = hasData;
        }

        public BetType BetType { get; }

        public DataTemplate CreateTemplate()
        {
            if (hasData && TemplateMapper.ContainsKey(BetType))
            {
                return TemplateMapper[BetType];
            }

            return new NoDataTemplate();
        }
    }
}