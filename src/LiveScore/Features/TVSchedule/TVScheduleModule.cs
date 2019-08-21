namespace LiveScore.Features.TVSchedule
{
    using LiveScore.Features.TVSchedule.ViewModels;
    using LiveScore.Features.TVSchedule.Views;
    using Prism.Ioc;
    using Prism.Modularity;

    public class TVScheduleModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            // OnInitialized
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<TVScheduleView, TVScheduleViewModel>();
            containerRegistry.RegisterForNavigation<EmptyTVScheduleView, EmptyTVScheduleViewModel>();
        }
    }
}
