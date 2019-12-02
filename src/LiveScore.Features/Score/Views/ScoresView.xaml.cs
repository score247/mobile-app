using System.Diagnostics;
using LiveScore.Common.Helpers;
using LiveScore.Features.Score.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Features.Score.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScoresView
    {
        private bool secondLoad;

        public ScoresView()
        {
            InitializeComponent();

            (BindingContext as ScoresViewModel)?.OnAppearing();
        }

        protected override void OnAppearing()
        {
            if (secondLoad)
            {
                (BindingContext as ScoresViewModel)?.OnAppearing();
            }

            secondLoad = true;
        }

        protected override void OnDisappearing()
        {
            (BindingContext as ScoresViewModel)?.OnDisappearing();

            // Remove first load event triggers
            Triggers.Clear();
        }

        private static void LeagueTable_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
#if DEBUG
            const int lastMatchItemIndexShowedOnTheScreen = 9;

            if (e.ItemIndex != lastMatchItemIndexShowedOnTheScreen)
            {
                return;
            }

            Profiler.Stop("IOS Application");
            Profiler.Stop("ScoreItemViewModel.LoadMatches.SelectDate");
            Profiler.Stop("ScoreItemViewModel.OnNavigatedTo");
            Profiler.Stop("ScoreItemViewModel.OnResume");
            Profiler.Stop("ScoresView.Render");

            Debug.WriteLine("");
            Debug.WriteLine("=======================================");
            Debug.WriteLine("");
#endif
        }
    }
}