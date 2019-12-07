using LiveScore.Core.ViewModels;
using Xamarin.Forms;

namespace LiveScore.Features.TVSchedule.Views
{
    public partial class EmptyTVScheduleView : ContentPage
    {
        public EmptyTVScheduleView()
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