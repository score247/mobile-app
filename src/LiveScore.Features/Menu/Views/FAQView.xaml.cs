namespace LiveScore.Features.Menu.Views
{
    using LiveScore.Core.Controls.ExpandableView;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FAQView : ContentPage
    {
        public FAQView()
        {
            InitializeComponent();
        }

        private static async void ExpandableView_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            var rotation = 180;
            var headerStyle = "Question";
            switch (e.Status)
            {
                case ExpandStatus.Collapsing:
                    break;

                case ExpandStatus.Expanding:
                    rotation = 0;
                    headerStyle = "ExpandedQuestion";
                    break;

                default:
                    return;
            }
            if ((sender as ExpandableView)?.PrimaryView is StackLayout headerView)
            {
                if (headerView.Children?.Count > 0)
                {
                    var headerLabel = headerView.Children[0];
                    headerLabel.SetDynamicResource(StyleProperty, headerStyle);
                }
                if (headerView.Children?.Count > 1)
                {
                    var arrowLabel = headerView.Children[1];
                    await arrowLabel.RotateTo(rotation, 200, Easing.CubicInOut);
                }
            }
        }
    }
}