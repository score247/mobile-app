using System.Collections.Generic;
using LiveScore.Soccer.Enumerations;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Soccer.Views.Leagues.Templates.LeagueDetails.Table
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TableOutcomesTemplate : ContentView
    {
        public TableOutcomesTemplate()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty OutcomesProperty = BindableProperty.Create(
           nameof(Outcomes),
           typeof(IEnumerable<TeamOutcome>),
           typeof(TableOutcomesTemplate),
           propertyChanged: OnOutcomesPropertyChange);

        public IEnumerable<TeamOutcome> Outcomes
        {
            get => (IEnumerable<TeamOutcome>)GetValue(OutcomesProperty);
            set => SetValue(OutcomesProperty, value);
        }

        internal static void OnOutcomesPropertyChange(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is TableOutcomesTemplate control) || newValue == null || newValue == oldValue)
            {
                return;
            }

            var outcomes = newValue as IEnumerable<TeamOutcome>;
            control.Content = BuildTeamOutcomes(control, outcomes);
        }

        private static StackLayout BuildTeamOutcomes(TableOutcomesTemplate control, IEnumerable<TeamOutcome> outcomes)
        {
            var content = new StackLayout();

            foreach (var outcome in outcomes)
            {
                var colorFrame = new Frame { Style = (Style)control.Resources["MatchDetailTableOutComeTeamOnLeague"] };
                colorFrame.BackgroundColor = (Color)Application.Current.Resources[outcome.ColorResourceKey];

                var outcomeLabel = new Label
                {
                    Text = outcome.FriendlyName,
                    Style = (Style)control.Resources["MatchDetailTableOutComeLabel"]
                };

                var outcomeLayout = new FlexLayout { Style = (Style)control.Resources["MatchDetailTableOutComeLayout"] };
                outcomeLayout.Children.Add(colorFrame);
                outcomeLayout.Children.Add(outcomeLabel);

                content.Children.Add(outcomeLayout);
            }

            return content;
        }
    }
}