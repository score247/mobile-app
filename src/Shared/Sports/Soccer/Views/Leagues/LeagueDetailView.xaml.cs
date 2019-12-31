using System;
using System.Threading.Tasks;
using LiveScore.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Soccer.Views.Leagues
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeagueDetailView : ContentPage
    {
        private int navigationStackCount;

        public LeagueDetailView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            navigationStackCount = Navigation.NavigationStack.Count;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            Task.Delay(1000).ContinueWith(_ =>
            {
                if (Navigation.NavigationStack.Count < navigationStackCount)
                {
                    (BindingContext as ViewModelBase)?.Destroy();
                    BindingContext = null;
                    Content = null;
                    GC.Collect();
                }
            });
        }
    }
}