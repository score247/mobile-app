﻿namespace LiveScore.Favorites.ViewModels
{
    using Core.ViewModels;
    using LiveScore.Core.Factories;
    using Prism.Navigation;

    public class FavoriteViewModel : ViewModelBase
    {
        public FavoriteViewModel(INavigationService navigationService, IDepdendencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
        }
    }
}