using LiveScore.Features.News.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Features.News.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsView : ContentPage
    {
        public NewsView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is NewsViewModel viewModel)
            {
                viewModel.IsActive = true;
                viewModel.OnAppearing();
            }
        }

        protected override void OnDisappearing()
        {
            if (BindingContext is NewsViewModel viewModel)
            {
                viewModel.IsActive = false;
                viewModel.OnDisappearing();
            }
        }
    }
}