namespace LiveScore.Score.Views
{
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
        }
    }
}