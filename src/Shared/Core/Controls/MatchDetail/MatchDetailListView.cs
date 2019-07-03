namespace LiveScore.Core.Controls.MatchDetail
{
    using Xamarin.Forms;

    public class MatchDetailListView : ListView
    {
        public delegate void OverScrollUpdateEvent(object sender, float offset);

        public MatchDetailListView() : base(ListViewCachingStrategy.RecycleElement)
        {
        }

        public static void OnScrolling(double offset)
        {
            MessagingCenter.Send("MatchDetail", "OnScrolling", offset);
        }

        public static void OnScrollingBack()
        {
            MessagingCenter.Send("MatchDetail", "OnScrollingBack");
        }
    }
}