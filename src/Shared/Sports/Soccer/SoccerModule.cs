using LiveScore.Core.Converters;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Services;
using LiveScore.Soccer.Converters;
using LiveScore.Soccer.Converters.TimelineImages;
using LiveScore.Soccer.Services;
using LiveScore.Soccer.ViewModels.MatchDetails;
using LiveScore.Soccer.ViewModels.MatchDetails.DetailOdds;
using LiveScore.Soccer.Views;
using LiveScore.Soccer.Views.Templates;
using LiveScore.Soccer.Views.Templates.MatchDetails.DetailOdds;
using Prism.Ioc;
using Prism.Modularity;
using Xamarin.Forms;

namespace LiveScore.Soccer
{
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

            containerRegistry.RegisterSingleton<IHubService, SoccerHubService>(SportType.Soccer.Value.ToString());
            containerRegistry.RegisterSingleton<IMatchService, MatchService>(SportType.Soccer.Value.ToString());
            containerRegistry.RegisterSingleton<ISoccerMatchService, MatchService>();
            containerRegistry.RegisterSingleton<IOddsService, OddsService>(SportType.Soccer.Value.ToString());
            containerRegistry.RegisterSingleton<DataTemplate, MatchDataTemplate>(SportType.Soccer.Value.ToString());
            containerRegistry.RegisterSingleton<IMatchStatusConverter, MatchStatusConverter>(SportType.Soccer.Value.ToString());
            containerRegistry.RegisterSingleton<IMatchMinuteConverter, MatchMinuteConverter>(SportType.Soccer.Value.ToString());

            containerRegistry.RegisterSingleton<ITimelineEventImageConverter, DefaultEventImageConverter>();
            containerRegistry.RegisterSingleton<ITimelineEventImageConverter, ScoreChangeImageConverter>(
                EventType.ScoreChange.Value.ToString());
            containerRegistry.RegisterSingleton<ITimelineEventImageConverter, PenaltyShootOutImageConverter>(
                EventType.PenaltyShootout.Value.ToString());
        }
    }
}