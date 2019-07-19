﻿namespace LiveScore.Soccer.ViewModels.DetailTV
{
    using LiveScore.Core;
    using LiveScore.Core.Controls.TabStrip;
    using LiveScore.Core.Enumerations;
    using Prism.Navigation;
    using Xamarin.Forms;

    internal class DetailTVViewModel : TabItemViewModelBase
    {
        public DetailTVViewModel(
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            DataTemplate dataTemplate)
            : base(navigationService, serviceLocator, dataTemplate)
        {
            TabHeaderIcon = TabDetailImages.TV;
            TabHeaderActiveIcon = TabDetailImages.TVActive;
        }
    }
}