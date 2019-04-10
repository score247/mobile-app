namespace LiveScore.League.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Core.Constants;
    using Core.Factories;
    using Core.Services;
    using Core.ViewModels;
    using Core.Models.Leagues;
    using Prism.Navigation;
    using Prism.Services;

    public class LeagueViewModel : ViewModelBase
    {
        public LeagueViewModel(
            INavigationService navigationService,
            IGlobalFactoryProvider globalFactory,
            ISettingsService settingsService,
            IPageDialogService pageDialogService)
                : base(navigationService, globalFactory, settingsService)
        {
            Title = "League";

            Leagues = new ObservableCollection<ILeague>();
            IsLoading = true;
            HasData = !IsLoading;
        }

        public bool IsLoading { get; set; }

        public bool HasData { get; set; }

        public ObservableCollection<ILeague> Leagues { get; set; }

        public override void OnAppearing()
        {
            base.OnAppearing();

            IsLoading = false;
            HasData = !IsLoading;
        }
    }
}