using System.Diagnostics;

namespace LiveScore.Features.Score.Views
{
    using System.Threading.Tasks;
    using Common.Helpers;
    using LiveScore.Features.Score.ViewModels;
    using MethodTimer;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScoresView
    {
        [Time]
        public ScoresView()
        {
            InitializeComponent();
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