using System;
using LiveScore.Features.Score.ViewModels;
using Xamarin.Forms.Xaml;

namespace LiveScore.Features.Score.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScoresView : IDisposable
    {
        private bool secondLoad;
        private bool disposedValue;

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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Content = null;
                    BindingContext = null;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
    }
}