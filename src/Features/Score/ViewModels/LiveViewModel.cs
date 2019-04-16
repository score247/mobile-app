﻿namespace LiveScore.Score.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Core.Factories;
    using Core.Services;
    using Core.ViewModels;
    using LiveScore.Core.Models.Matches;
    using Prism.Commands;
    using Prism.Navigation;

    public class LiveViewModel : ViewModelBase
    {
        public LiveViewModel(
            INavigationService navigationService,
            IGlobalFactoryProvider globalFactory,
            ISettingsService settingsService) : base(navigationService, globalFactory, settingsService)
        {
        }

        public ObservableCollection<IGrouping<string, IMatch>> GroupMatches { get; set; }

        public bool IsRefreshingMatchList { get; set; }

        public DelegateCommand RefreshCommand
            => new DelegateCommand(() =>
            {
                IsRefreshingMatchList = true;
                GetMatches();
                IsRefreshingMatchList = false;
            });

        public override void OnAppearing()
        {
            GetMatches();
        }

        private void GetMatches()
        {
            var matches = new List<IMatch>();
            GroupMatches = new ObservableCollection<IGrouping<string, IMatch>>(matches.GroupBy(x => x.League.Id));
        }
    }
}