using System.ComponentModel;
using LiveScore.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using System.Linq;
using System;

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
            var primaryColor = (Color)App.Current.Resources["PrimaryColor"];

            txSearchField.BackgroundColor = UIColor.FromRGB((nfloat)primaryColor.R, (nfloat)primaryColor.G, (nfloat)primaryColor.B);
            txSearchField.ClipsToBounds = true;
        }
    }
}
