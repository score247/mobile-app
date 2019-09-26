﻿using LiveScore.Features.League.ViewModels;
using LiveScore.Features.League.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace LiveScore.Features.League
{
    public class LeagueModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Initialize module
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<LeagueView, LeagueViewModel>();
            containerRegistry.RegisterForNavigation<LeagueDetailView, LeagueDetailViewModel>();
        }
    }
}