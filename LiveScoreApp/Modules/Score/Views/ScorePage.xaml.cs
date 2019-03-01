using Xamarin.Forms;

namespace Score.Views
{
    public partial class ScorePage : ContentPage
    {
        public ScorePage()
        {
#if DEBUG
            LiveReload.Init();
#endif
            InitializeComponent();
        }
    }
}