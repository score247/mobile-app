namespace TVSchedule
{
    using Prism.Ioc;
    using Prism.Modularity;
    using TVSchedule.ViewModels;
    using TVSchedule.Views;

    public class TVScheduleModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<TVScheduleView, TVScheduleViewModel>();
        }
    }
}
