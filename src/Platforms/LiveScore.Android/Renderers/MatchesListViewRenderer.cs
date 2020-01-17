using Android.Content;
using LiveScore.Core.Controls.MatchesListView;
using LiveScore.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MatchesListView), typeof(MatchesListViewRenderer))]

namespace LiveScore.Droid.Renderers
{
    public class MatchesListViewRenderer : ListViewRenderer
    {
        public MatchesListViewRenderer(Context context) : base(context)
        {
        }
    }
}