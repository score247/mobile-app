using System;
using LiveScore.Core.Controls.CustomSearchBar;
using LiveScore.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomSearchBar), typeof(CustomSearchBarRenderer))]

namespace LiveScore.iOS.Renderers
{
    public class CustomSearchBarRenderer : SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            if (Control == null || !(Control.ValueForKey(new Foundation.NSString("searchField")) is UITextField txSearchField))
            {
                return;
            }

            var backgroundColor = (Color)App.Current.Resources["SearchBarTextBoxBackgroundColor"];

            txSearchField.BackgroundColor = UIColor.FromRGB(
                (nfloat)backgroundColor.R,
                (nfloat)backgroundColor.G,
                (nfloat)backgroundColor.B);
            txSearchField.ClipsToBounds = true;

            if (txSearchField.LeftView is UIImageView glassIcon)
            {
                var placeHolderColor = (Color)App.Current.Resources["SearchBarPlaceholderColor"];
                glassIcon.TintColor = UIColor.FromRGB(
                    (nfloat)placeHolderColor.R,
                    (nfloat)placeHolderColor.G,
                    (nfloat)placeHolderColor.B);
            }

            var tintColor = (Color)App.Current.Resources["FourthTextColor"];
            Control.TintColor = UIColor.FromRGB(
                (nfloat)tintColor.R,
                (nfloat)tintColor.G,
                (nfloat)tintColor.B);

            if (Element is CustomSearchBar searchBar && searchBar.ShowCancelButton)
            {
                Control.TextChanged += (sender, args) => Control.SetShowsCancelButton(true, false);
                Control.OnEditingStarted += (sender, args) => Control.SetShowsCancelButton(true, true);
                Control.OnEditingStopped += (sender, args) => Control.SetShowsCancelButton(false, true);
                Control.CancelButtonClicked += Control_CancelButtonClicked;
            }
        }

        private void Control_CancelButtonClicked(object sender, EventArgs e)
        {
            if (Element is CustomSearchBar searchBar)
            {
                searchBar.OnCancelled();
            }
        }
    }
}