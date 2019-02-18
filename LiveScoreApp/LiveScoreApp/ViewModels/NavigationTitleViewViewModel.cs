using LiveScoreApp.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScoreApp.ViewModels
{
    public class NavigationTitleViewViewModel : ViewModelBase
    {
        private string _title;

        public string SubTitle
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public DelegateCommand SelectSportCommand { get; set; }

        public NavigationTitleViewViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            SubTitle = "Test;";
            SelectSportCommand = new DelegateCommand(NavigateSelectSportPage);
        }

        private async void NavigateSelectSportPage()
        {
            await NavigationService.NavigateAsync("NavigationPage/SelectSportPage");
        }
    }
}