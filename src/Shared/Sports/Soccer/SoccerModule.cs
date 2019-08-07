﻿namespace LiveScore.Soccer
{
    using LiveScore.Core.Converters;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Converters;
    using LiveScore.Soccer.Services;
    using LiveScore.Soccer.ViewModels;
    using LiveScore.Soccer.ViewModels.DetailOdds.OddItems;
    using LiveScore.Soccer.Views;
    using LiveScore.Soccer.Views.Templates;
    using LiveScore.Soccer.Views.Templates.DetailOdds.OddsItems;
    using Prism.Ioc;
    using Prism.Modularity;
    using Xamarin.Forms;

    public class SoccerModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            // OnInitialized
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MatchDetailView, MatchDetailViewModel>(
                nameof(MatchDetailView) + SportTypes.Soccer.Value);

            containerRegistry.RegisterForNavigation<OddsMovementView, OddsMovementViewModel>(
                nameof(OddsMovementView) + SportTypes.Soccer.Value);

            containerRegistry.Register<IMatchService, MatchService>(SportTypes.Soccer.Value.ToString());
            containerRegistry.Register<IOddsService, OddsService>(SportTypes.Soccer.Value.ToString());
            containerRegistry.Register<DataTemplate, MatchDataTemplate>(SportTypes.Soccer.Value.ToString());
            containerRegistry.Register<IMatchStatusConverter, MatchStatusConverter>(SportTypes.Soccer.Value.ToString());
            containerRegistry.Register<IHubService, HubService>(SportTypes.Soccer.Value.ToString());
        }
    }
}