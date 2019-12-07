namespace LiveScore.Features.League.Views
{
    using LiveScore.Core.ViewModels;
    using Xamarin.Forms;

    public partial class LeagueView : ContentPage
    {
        public LeagueView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            (BindingContext as ViewModelBase)?.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            (BindingContext as ViewModelBase)?.OnDisappearing();
        }
    }
}