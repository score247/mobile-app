using Xamarin.Forms;

namespace LiveScore.Core.Controls.Calendar
{
    public class CalendarListView : ListView
    {
        public CalendarListView() : base(ListViewCachingStrategy.RecycleElement)
        {
        }
    }
}