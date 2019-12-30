using System;
using System.Diagnostics;
using LiveScore.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Soccer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MatchDetailView : ContentPage
    {
        private int navigationStackCount;

        public MatchDetailView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            navigationStackCount = Navigation.NavigationStack.Count;

            var tabStripOffset = TabStrip.Y;
            TabStrip.OnItemScrolling((scrollOffset) =>
            {
                var newOffset = scrollOffset <= tabStripOffset ? scrollOffset : tabStripOffset;

                MatchDetailLayout.TranslationY = -newOffset;
            });
        }

        protected override void OnDisappearing()
        {
            (BindingContext as ViewModelBase)?.OnDisappearing();
            TabStrip.OnDisappearing();

            if (Navigation.NavigationStack.Count <= navigationStackCount)
            {
                (BindingContext as ViewModelBase)?.Destroy();
                BindingContext = null;
                Content = null;
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            }
        }
    }
}