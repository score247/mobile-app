using Android.Content;
using Android.Widget;
using LiveScore.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using G = Android.Graphics;

[assembly: ExportRenderer(typeof(SearchBar), typeof(CustomSearchBarRenderer))]

namespace LiveScore.Droid.Renderers
{
    public class CustomSearchBarRenderer : SearchBarRenderer
    {
        public CustomSearchBarRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.Background = Context.GetDrawable(Resource.Drawable.custom_search_view);
            }

            var metrics = Resources.DisplayMetrics;
            var backGroundColor = ((Color)App.Current.Resources["SearchBarTextBoxBackgroundColor"]).ToAndroid();
            var textColor = ((Color)App.Current.Resources["PrimaryTextColor"]).ToAndroid();

            SearchView searchView = base.Control;
            searchView.SetMaxWidth(metrics.WidthPixels);

            var textViewId = searchView.Context.Resources.GetIdentifier("android:id/search_src_text", null, null);
            EditText textView = (searchView.FindViewById(textViewId) as EditText);
            textView.SetBackgroundColor(backGroundColor);
            textView.SetTextColor(textColor);

            var searchIconId = searchView.Resources.GetIdentifier("android:id/search_mag_icon", null, null);
            var searchPlateIcon = searchView.FindViewById(searchIconId);
            (searchPlateIcon as ImageView).SetColorFilter(textColor, G.PorterDuff.Mode.SrcIn);
        }
    }
}