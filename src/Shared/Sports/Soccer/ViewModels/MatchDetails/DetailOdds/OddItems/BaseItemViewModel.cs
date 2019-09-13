namespace LiveScore.Soccer.ViewModels.MatchDetails.DetailOdds.OddItems
{
    using System;
    using System.Collections.Generic;
    using Core;
    using Enumerations;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.ViewModels;
    using LiveScore.Soccer.Views.Templates.MatchDetails.DetailOdds.OddsItems.AsianHdp;
    using LiveScore.Soccer.Views.Templates.MatchDetails.DetailOdds.OddsItems.OneXTwo;
    using LiveScore.Soccer.Views.Templates.MatchDetails.DetailOdds.OddsItems.OverUnder;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class BaseItemViewModel : ViewModelBase
    {
        private static readonly IDictionary<BetType, Type> ViewModelMapper = new Dictionary<BetType, Type>
        {
            { BetType.OneXTwo, typeof(OneXTwoItemViewModel) },
            { BetType.AsianHDP, typeof(AsianHdpItemViewModel) },
            { BetType.OverUnder, typeof(OverUnderItemViewModel) },
        };

        private static readonly IDictionary<BetType, DataTemplate> TemplateMapper = new Dictionary<BetType, DataTemplate>
        {
            { BetType.OneXTwo, new OneXTwoItemTemplate() },
            { BetType.AsianHDP, new AsianHdpItemTemplate() },
            { BetType.OverUnder, new OverUnderItemTemplate() },
        };

        public BaseItemViewModel(
            BetType betType,
            IBetTypeOdds betTypeOdds,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver)
            : base(navigationService, dependencyResolver)
        {
            BetTypeOdds = betTypeOdds;
            BetType = betType;

            BookmakerName = betTypeOdds.Bookmaker?.Name;
        }

        public string BookmakerName { get; }

        public IBetTypeOdds BetTypeOdds { get; protected set; }

        public BetType BetType { get; }

        public string MatchId { get; }

        public DataTemplate CreateTemplate()
        {
            if (TemplateMapper.ContainsKey(BetType))
            {
                return TemplateMapper[BetType];
            }

            throw new NotImplementedException($"Not implemented {BetType} yet");
        }

        public BaseItemViewModel CreateInstance()
        {
            if (ViewModelMapper.ContainsKey(BetType))
            {
                return Activator.CreateInstance(
                    ViewModelMapper[BetType],
                    BetTypeOdds, NavigationService, DependencyResolver) as BaseItemViewModel;
            }

            throw new NotImplementedException($"Not implemented {BetType} yet");
        }

        public virtual void UpdateOdds(IBetTypeOdds betTypeOdds)
        {
            BetTypeOdds = betTypeOdds;
        }
    }
}