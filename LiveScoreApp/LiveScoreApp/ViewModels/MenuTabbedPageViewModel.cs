using Core.LangResources;
using LiveScoreApp.Views;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScoreApp.ViewModels
{
    public class MenuTabbedPageViewModel : ViewModelBase
    {
        public MenuTabbedPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }
    }
}