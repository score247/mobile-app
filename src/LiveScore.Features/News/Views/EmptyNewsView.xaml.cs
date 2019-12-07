using LiveScore.Core.ViewModels;
using Xamarin.Forms;

namespace LiveScore.Features.News.Views
{
    public partial class EmptyNewsView : ContentPage
    {
        public EmptyNewsView()
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