namespace LiveScore.Score.Views
{
    using System;
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
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
            }

#if DEBUG
            this.Appearing += ScoresView_Appearing;
#endif
        }

        private void ScoresView_Appearing(object sender, EventArgs e)
        {
            Profiler.Stop("IOS Application");
        }
    }
}