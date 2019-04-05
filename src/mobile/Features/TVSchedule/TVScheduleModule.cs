namespace LiveScore.TVSchedule
{
    using Prism.Ioc;
    using Prism.Modularity;
    using LiveScore.TVSchedule.ViewModels;
    using LiveScore.TVSchedule.Views;

    public class TVScheduleModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            // OnInitialized
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<TVScheduleView, TVScheduleViewModel>();
        }
    }
}
