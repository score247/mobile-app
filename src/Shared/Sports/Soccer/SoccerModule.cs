namespace LiveScore.Soccer
{
    using LiveScore.Common.Controls.TabStrip;
    using LiveScore.Core.Converters;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Converters;
    using LiveScore.Soccer.Services;
    using LiveScore.Soccer.ViewModels;
    using LiveScore.Soccer.ViewModels.MatchDetailInfo;
    using LiveScore.Soccer.Views;
    using LiveScore.Soccer.Views.Templates;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Mvvm;
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

            containerRegistry.Register<IMatchService, MatchService>(SportTypes.Soccer.Value);
            containerRegistry.Register<DataTemplate, MatchDataTemplate>(SportTypes.Soccer.Value);
            containerRegistry.Register<IMatchStatusConverter, MatchStatusConverter>(SportTypes.Soccer.Value);
        }
    }
}