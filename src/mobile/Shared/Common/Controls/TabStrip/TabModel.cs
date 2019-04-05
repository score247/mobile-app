namespace LiveScore.Common.Controls.TabStrip
{
    using Xamarin.Forms;

    public class TabModel
    {
        public string Name { get; set; }

        public View Template { get; set; }

        public object ViewModel { get; set; }
    }
}
