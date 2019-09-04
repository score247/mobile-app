namespace LiveScore.Features.TVSchedule
{
    using ViewModels;
    using Views;
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