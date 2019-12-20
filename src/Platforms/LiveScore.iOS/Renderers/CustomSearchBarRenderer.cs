using System;
using System.ComponentModel;
using LiveScore.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SearchBar), typeof(CustomSearchBarRenderer))]

namespace LiveScore.iOS.Renderers
{
    public class CustomSearchBarRenderer : SearchBarRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            Control.ShowsCancelButton = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            var txSearchField = (UITextField)Control.ValueForKey(new Foundation.NSString("searchField"));
            var glassIcon = txSearchField.LeftView as UIImageView;

            var backgroundColor = (Color)App.Current.Resources["SearchBarTextBoxBackgroundColor"];

            txSearchField.BackgroundColor = UIColor.FromRGB(
                (nfloat)backgroundColor.R,
                (nfloat)backgroundColor.G,
                (nfloat)backgroundColor.B);

            var placeHolderColor = (Color)App.Current.Resources["SearchBarPlaceholderColor"];
            glassIcon.TintColor = UIColor.FromRGB(
                (nfloat)placeHolderColor.R,
                (nfloat)placeHolderColor.G,
                (nfloat)placeHolderColor.B);

            txSearchField.ClipsToBounds = true;
        }
    }
}