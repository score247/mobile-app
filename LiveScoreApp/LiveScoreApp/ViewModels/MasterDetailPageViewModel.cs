using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveScoreApp.ViewModels
{
    public class MasterDetailPageViewModel : ViewModelBase
    {
        private INavigationService navigationService;

        public DelegateCommand<string> NavigateCommand { get; set; }

        public MasterDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.navigationService = navigationService;
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private async void Navigate(string name)
        {
            var result = await navigationService.NavigateAsync(name);

            if (!result.Success)
            {
            }
        }
    }
}