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
        private bool isLoading;
        private bool hasData;
        private ObservableCollection<ILeague> leagues;


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

        public bool IsLoading
        {
            get { return isLoading; }
            set { SetProperty(ref isLoading, value); }
        }

        public bool HasData
        {
            get { return hasData; }
            set { SetProperty(ref hasData, value); }
        }

        public ObservableCollection<ILeague> Leagues
        {
            get => leagues;
            set => SetProperty(ref leagues, value);
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            IsLoading = false;
            HasData = !IsLoading;
        }
    }
}