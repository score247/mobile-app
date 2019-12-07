using LiveScore.Core.ViewModels;
using Xamarin.Forms;

namespace LiveScore.Features.Favorites.Views
{
    public partial class EmptyFavoriteView : ContentPage
    {
        public EmptyFavoriteView()
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