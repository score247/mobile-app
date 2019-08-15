namespace LiveScore.Soccer.Views.Templates
{
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.Models.Teams;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RedCardTemplate : ContentView
    {
        public RedCardTemplate()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty StatisticProperty = BindableProperty.Create(
          nameof(Statistic),
          typeof(TeamStatistic),
          typeof(RedCardTemplate),
          propertyChanged: OnRedCardCountChanged);

        public TeamStatistic Statistic
        {
            get { return (TeamStatistic)GetValue(StatisticProperty); }
            set { SetValue(StatisticProperty, value); }
        }

        private static void OnRedCardCountChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RedCardTemplate)bindable;

            if (control == null || newValue == null)
            {
                return;
            }

            var statistics = (TeamStatistic)newValue;
            var stackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };

            for (int i = 0; i < statistics.RedCards + statistics.YellowRedCards; i++)
            {
                stackLayout.Children.Add(BuildRedCardImage());
            }

            control.Content = stackLayout;
        }

        private static Image BuildRedCardImage()
        {
            return new Image
            {
                Source = Images.RedCard.Value,
                Margin = new Thickness(4, 0, 0, 0)
            };
        }
    }
}