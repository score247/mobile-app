﻿namespace LiveScore.Features.Menu
{
    using Prism.Ioc;
    using Prism.Modularity;
    using ViewModels;
    using Views;

    public class MenuModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            // OnInitialized
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<RefreshView, RefreshViewModel>();
            containerRegistry.RegisterForNavigation<DefaultSportView, DefaultSportViewModel>();
            containerRegistry.RegisterForNavigation<DefaultLanguageView, DefaultLanguageViewModel>();
            containerRegistry.RegisterForNavigation<InfoAlertView, InfoAlertViewModel>();
            containerRegistry.RegisterForNavigation<FAQView, FaqPageViewModel>();
        }
    }
}