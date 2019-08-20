namespace LiveScore.Score.Views
{
    using System;
    using System.Diagnostics;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScoresView : ContentPage
    {
        public ScoresView()
        {
            InitializeComponent();

            this.Appearing += ScoresView_Appearing;

            this.LayoutChanged += ScoresView_LayoutChanged;

            this.MeasureInvalidated += ScoresView_MeasureInvalidated;

            this.SizeChanged += ScoresView_SizeChanged;

            //this.DescendantRemoved += ScoresView_DescendantRemoved;

            this.Disappearing += ScoresView_Disappearing;

            //this.DescendantAdded += ScoresView_DescendantAdded;

            //this.PropertyChanged += ScoresView_PropertyChanged;

            //this.PropertyChanging += ScoresView_PropertyChanging;            
        }

        private void ScoresView_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            Debug.WriteLine($"ScoresView_PropertyChanging: {DateTime.Now.ToString("HH:mm:ss:ffff")}");
        }

        private void ScoresView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Debug.WriteLine($"ScoresView_PropertyChanged: {DateTime.Now.ToString("HH:mm:ss:ffff")}");
        }

        private void ScoresView_DescendantAdded(object sender, ElementEventArgs e)
        {
            Debug.WriteLine($"ScoresView_DescendantAdded: {DateTime.Now.ToString("HH:mm:ss:ffff")}");
        }

        private void ScoresView_Disappearing(object sender, EventArgs e)
        {
            Debug.WriteLine($"ScoresView_Disappearing: {DateTime.Now.ToString("HH:mm:ss:ffff")}");
        }

        private void ScoresView_DescendantRemoved(object sender, ElementEventArgs e)
        {
            Debug.WriteLine($"ScoresView_DescendantRemoved: {DateTime.Now.ToString("HH:mm:ss:ffff")}");
        }

        private void ScoresView_SizeChanged(object sender, EventArgs e)
        {
            Debug.WriteLine($"ScoresView_SizeChanged: {DateTime.Now.ToString("HH:mm:ss:ffff")}");
        }

        private void ScoresView_MeasureInvalidated(object sender, EventArgs e)
        {
            Debug.WriteLine($"ScoresView_MeasureInvalidated: {DateTime.Now.ToString("HH:mm:ss:ffff")}");
        }

        private void ScoresView_LayoutChanged(object sender, EventArgs e)
        {
            Debug.WriteLine($"ScoresView_LayoutChanged: {DateTime.Now.ToString("HH:mm:ss:ffff")}");
        }

        private void ScoresView_Appearing(object sender, EventArgs e)
        {
            Debug.WriteLine($"ScoresView_Appearing: {DateTime.Now.ToString("HH:mm:ss:ffff")}");
        }
    }
}