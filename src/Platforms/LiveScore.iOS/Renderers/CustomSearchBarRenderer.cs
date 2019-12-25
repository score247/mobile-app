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

            if (Control != null)
            {
                Control.ShowsCancelButton = false;
            }
        }

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
        }
    }
}