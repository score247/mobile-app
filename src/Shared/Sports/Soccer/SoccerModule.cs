namespace LiveScore.Soccer
{
    using LiveScore.Core.Converters;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Converters;
    using LiveScore.Soccer.Services;
    using LiveScore.Soccer.Views.Templates;
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
            containerRegistry.Register<IMatchService, MatchService>(SportTypes.Soccer.Value);
            containerRegistry.Register<DataTemplate, MatchDataTemplate>(SportTypes.Soccer.Value);
            containerRegistry.Register<IMatchStatusConverter, MatchStatusConverter>(SportTypes.Soccer.Value);
            containerRegistry.Register<IBackgroundJob, MatchAutoUpdateJob>(nameof(MatchAutoUpdateJob) + SportTypes.Soccer.Value);
        }
    }
}