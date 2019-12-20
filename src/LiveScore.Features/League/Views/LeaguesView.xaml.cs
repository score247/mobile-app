using LiveScore.Features.League.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Features.League.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeaguesView : ContentPage
    {
        public LeaguesView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            if (BindingContext is LeaguesViewModel viewModel)
            {
                viewModel.IsActive = true;
                viewModel.OnAppearing();
            }
        }

        protected override void OnDisappearing()
        {
            if (BindingContext is LeaguesViewModel viewModel)
            {
                viewModel.IsActive = false;
                viewModel.OnDisappearing();
            }
        }
    }
}