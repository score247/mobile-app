﻿namespace LiveScore.Soccer.ViewModels.DetailOdds.OddItems
{
    using System;
    using System.Collections.Generic;
    using LiveScore.Core;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.ViewModels;
    using LiveScore.Soccer.Views.Templates.OddsItems;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class BaseItemViewModel : ViewModelBase
    {
        private static readonly IDictionary<string, Type> ViewModelMapper = new Dictionary<string, Type>
        {
            { BetType.OneXTwo.ToString(), typeof(OneXTwoViewModel) },
           
        };

        private static readonly IDictionary<string, DataTemplate> TemplateMapper = new Dictionary<string, DataTemplate>
        {
            { BetType.OneXTwo.ToString(), new OneXTwoItemTemplate() },
        };

        public IBetTypeOdds BetTypeOdds { get; }

        public BetType BetType { get; }

        public string type { get; }

        public BaseItemViewModel(
            BetType betType,
            IBetTypeOdds betTypeOdds,           
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver)
            : base(navigationService, depdendencyResolver)
        {
            BetTypeOdds = betTypeOdds;
            BetType = betType;
            type = betType.ToString();
        }

        public DataTemplate CreateTemplate()
        {
            if (TemplateMapper.ContainsKey(type))
            {
                return TemplateMapper[type];
            }

            return new OneXTwoItemTemplate();
        }

        public BaseItemViewModel CreateInstance()
        {
            if (ViewModelMapper.ContainsKey(type))
            {
                return Activator.CreateInstance(
                    ViewModelMapper[type],
                    BetType, BetTypeOdds, NavigationService, DependencyResolver) as BaseItemViewModel;
            }

            return new BaseItemViewModel(BetType, BetTypeOdds, NavigationService, DependencyResolver);
        }

    }
}
