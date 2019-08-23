namespace LiveScore.Score.Views
{
    using LiveScore.Common.Helpers;
    using MethodTimer;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScoresView : ContentPage
    {
        [Time]
        public ScoresView()
        {
            InitializeComponent();
            LeagueTable.ItemAppearing += LeagueTable_ItemAppearing;
        }

        private void LeagueTable_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
#if DEBUG
            if (e.ItemIndex == 9)
            {
                
                Profiler.Stop("ScoresViewModel.LoadMatches.Home");
                Profiler.Stop("ScoresViewModel.LoadMatches.PullDownToRefresh");
                Profiler.Stop("ScoresViewModel.LoadMatches.SelectDate");
                Profiler.Stop("ScoresViewModel.OnNavigatedTo");
                Profiler.Stop("ScoresViewModel.OnResume");
            }
#endif
        }
    }
}