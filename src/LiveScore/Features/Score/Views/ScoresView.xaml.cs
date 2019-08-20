namespace LiveScore.Score.Views
{
    using System;
    using System.Diagnostics;
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

            # if DEBUG
            this.Appearing += ScoresView_Appearing;

            this.Disappearing += ScoresView_Disappearing;
            #endif
        }

        private void ScoresView_Disappearing(object sender, EventArgs e)
        {
            Debug.WriteLine($"ScoresView_Disappearing: {DateTime.Now.ToString("HH:mm:ss:ffff")}");
        }

        private void ScoresView_Appearing(object sender, EventArgs e)
        {
            Debug.WriteLine($"ScoresView_Appearing: {DateTime.Now.ToString("HH:mm:ss:ffff")}");
        }
    }
}