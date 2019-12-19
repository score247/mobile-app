namespace LiveScore.Features.Favorites.Views
{
    using LiveScore.Features.Favorites.ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoriteView : ContentPage
    {
        public FavoriteView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            if (BindingContext is FavoriteViewModel viewModel)
            {
                viewModel.IsActive = true;
                viewModel.OnAppearing();
            }
        }

        protected override void OnDisappearing()
        {
            if (BindingContext is FavoriteViewModel viewModel)
            {
                viewModel.IsActive = false;
                viewModel.OnDisappearing();
            }
        }
    }
}