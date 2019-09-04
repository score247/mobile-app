namespace LiveScore.Soccer
{
    using Converters;
    using LiveScore.Core.Converters;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Services;
    using Prism.Ioc;
    using Prism.Modularity;
    using Services;
    using ViewModels.MatchDetails;
    using ViewModels.MatchDetails.DetailOdds;
    using Views;
    using Views.Templates;
    using Views.Templates.MatchDetails.DetailOdds;
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
                nameof(MatchDetailView) + SportType.Soccer.Value);

            containerRegistry.RegisterForNavigation<OddsMovementView, OddsMovementViewModel>(
                nameof(OddsMovementView) + SportType.Soccer.Value);

            containerRegistry.Register<IMatchService, MatchService>(SportType.Soccer.Value.ToString());
            containerRegistry.Register<IOddsService, OddsService>(SportType.Soccer.Value.ToString());
            containerRegistry.Register<DataTemplate, MatchDataTemplate>(SportType.Soccer.Value.ToString());
            containerRegistry.Register<IMatchStatusConverter, MatchStatusConverter>(SportType.Soccer.Value.ToString());
            containerRegistry.Register<IMatchMinuteConverter, MatchMinuteConverter>(SportType.Soccer.Value.ToString());
        }
    }
}