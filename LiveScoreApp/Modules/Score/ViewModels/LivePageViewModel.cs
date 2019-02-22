using LiveScoreApp.Core.Settings;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Score.Models;
using Score.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Score.ViewModels
{
	public class LivePageViewModel :  ViewModelBase
    {
        private ObservableCollection<IGrouping<string, Match>> groupMatches;
        private bool isRefreshingMatchList;
        private readonly IScoreService scoreService;

        public ObservableCollection<IGrouping<string, Match>> GroupMatches
        {
            get { return groupMatches; }
            set { SetProperty(ref groupMatches, value); }
        }
        
        public bool IsRefreshingMatchList
        {
            get { return isRefreshingMatchList; }
            set { SetProperty(ref isRefreshingMatchList, value); }
        }

        public DelegateCommand RefreshCommand
            => new DelegateCommand(() =>
            {
                IsRefreshingMatchList = true;
                GetMatches();
                IsRefreshingMatchList = false;
            });

        public LivePageViewModel(INavigationService navigationService, IScoreService scoreService)
            : base(navigationService)
        {
            this.scoreService = scoreService;
        }

        public override void OnAppearing()
        {
            GetMatches();
        }

        private void GetMatches()
        {
            var matches = scoreService.GetAll(Settings.CurrentSportId).ToList();
            GroupMatches = new ObservableCollection<IGrouping<string, Match>>(matches.GroupBy(x => x.GroupName));
        }
    }
}
