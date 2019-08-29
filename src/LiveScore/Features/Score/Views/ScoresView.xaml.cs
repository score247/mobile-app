namespace LiveScore.Score.Views
{
    using LiveScore.Common.Helpers;
    using MethodTimer;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScoresView : ContentPage
    {
        private const int LastMatchItemIndexShowedOnTheScreen = 9;

        [Time]
        public ScoresView()
        {
            InitializeComponent();
            LeagueTable.ItemAppearing += LeagueTable_ItemAppearing;
        }

        private static void LeagueTable_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
#if DEBUG
            if (e.ItemIndex == LastMatchItemIndexShowedOnTheScreen)
            {
            }
#endif
        }
    }
}