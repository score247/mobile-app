﻿namespace LiveScore.Features.Favorites.ViewModels
{
    using Core;
    using Core.ViewModels;
    using Prism.Navigation;

    public class FavoriteViewModel : ViewModelBase
    {
        public FavoriteViewModel(INavigationService navigationService, IDependencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
        }
    }
}