﻿namespace LiveScore.Features.News.ViewModels
{
    using Core;
    using Core.ViewModels;
    using Prism.Navigation;

    public class NewsViewModel : ViewModelBase
    {
        public NewsViewModel(INavigationService navigationService, IDependencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
            Title = "News";
        }
    }
}