using LiveScore.Features.Score.ViewModels;
using Xamarin.Forms.Xaml;

namespace LiveScore.Features.Score.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScoresView
    {
        private bool secondLoad;

        public ScoresView()
        {
            InitializeComponent();

            (BindingContext as ScoresViewModel)?.OnAppearing();
        }

        protected override void OnAppearing()
        {
            if (secondLoad && BindingContext is ScoresViewModel viewModel)
            {
                viewModel.IsActive = true;
                viewModel.OnAppearing();
            }

            secondLoad = true;
        }

        protected override void OnDisappearing()
        {
            if (BindingContext is ScoresViewModel viewModel)
            {
                viewModel.IsActive = false;
                viewModel.OnDisappearing();
            }

            Triggers.Clear();
        }
    }
}