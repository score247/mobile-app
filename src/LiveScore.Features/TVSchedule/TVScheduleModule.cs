namespace LiveScore.Features.TVSchedule
{
    using Prism.Ioc;
    using Prism.Modularity;
    using ViewModels;
    using Views;

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