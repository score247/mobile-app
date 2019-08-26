﻿namespace LiveScore.Score.Views
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