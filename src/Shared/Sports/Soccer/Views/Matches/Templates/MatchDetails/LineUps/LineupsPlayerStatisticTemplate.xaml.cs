using System.Collections.Generic;
using FFImageLoading.Svg.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Soccer.Views.Templates.MatchDetails.LineUps
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LineupsPlayerStatisticTemplate : ContentView
    {
        public static readonly BindableProperty PlayerStatisticsProperty = BindableProperty.Create(
             nameof(List<KeyValuePair<string, int>>),
             typeof(List<KeyValuePair<string, int>>),
             typeof(LineupsPlayerStatisticTemplate),
             propertyChanged: OnPlayerStatisticsChanged);

        public List<KeyValuePair<string, int>> PlayerStatistics
        {
            get => (List<KeyValuePair<string, int>>)GetValue(PlayerStatisticsProperty);
            set => SetValue(PlayerStatisticsProperty, value);
        }

        public LineupsPlayerStatisticTemplate()
        {
            InitializeComponent();
        }

        private static void OnPlayerStatisticsChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            var control = (LineupsPlayerStatisticTemplate)bindableObject;

            if (control == null || newValue == null)
            {
                return;
            }

            var stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Style = control.Resources["MatchDetailLineUpsIconArea"] as Style
            };

            var playerStatistic = (List<KeyValuePair<string, int>>)newValue;

            for (var i = 0; i < playerStatistic.Count; i++)
            {
                stackLayout.Children.Add(BuildPlayerStatistic(playerStatistic[i], control));
            }

            control.Content = stackLayout;
        }

        private static AbsoluteLayout BuildPlayerStatistic(KeyValuePair<string, int> statistic, LineupsPlayerStatisticTemplate control)
        {
            var image = new SvgCachedImage
            {
                Style = control.Resources["MatchDetailLineUpsSubstitutionImage"] as Style,
                DownsampleToViewSize = true,
                Source = statistic.Key
            };

            var button = new Button
            {
                IsVisible = statistic.Value > 1,
                Text = statistic.Value.ToString(),
                Style = control.Resources["MatchDetailLineUpsScoreNumber"] as Style
            };

            var absoluteLayout = new AbsoluteLayout
            {
                Style = control.Resources["MatchDetailLineUpsScoreBox"] as Style,
                HorizontalOptions = LayoutOptions.End
            };

            absoluteLayout.Children.Add(image);
            absoluteLayout.Children.Add(button);

            return absoluteLayout;
        }
    }
}