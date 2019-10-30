﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Soccer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MatchDetailView : ContentPage
    {
        public MatchDetailView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var tabStripOffset = TabStrip.Y;
            AbsoluteLayout.SetLayoutBounds(MatchDetailLayout,
                new Rectangle(0, 0, 1, MatchDetailLayout.Height + tabStripOffset));
            AbsoluteLayout.SetLayoutFlags(MatchDetailLayout, AbsoluteLayoutFlags.WidthProportional);

            TabStrip.OnItemScrolling((scrollOffset) =>
            {
                var newOffset = scrollOffset <= tabStripOffset ? scrollOffset : tabStripOffset;

                MatchDetailLayout.TranslationY = -newOffset;
            });
        }

        protected override void OnDisappearing()
        {
            TabStrip.OnDisappearing();
        }
    }
}