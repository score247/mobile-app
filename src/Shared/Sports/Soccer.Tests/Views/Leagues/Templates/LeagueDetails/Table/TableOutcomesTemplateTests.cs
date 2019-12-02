using System.Collections.Generic;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Soccer.Enumerations;
using LiveScore.Soccer.Views.Leagues.Templates.LeagueDetails.Table;
using Xamarin.Forms;
using Xunit;

namespace Soccer.Tests.Views.Leagues.Templates.LeagueDetails.Table
{
    public class TableOutcomesTemplateTests : IClassFixture<ResourcesFixture>
    {
        [Fact]
        public void Outcomes_GetSetValueFromNotesProp()
        {
            // Arrange
            var template = new TableOutcomesTemplate();
            var outcomes = new List<TeamOutcome> { TeamOutcome.AFCChampionsLeague };

            // Act
            template.Outcomes = outcomes;

            // Assert
            var actualOutcomes = template.GetValue(TableOutcomesTemplate.OutcomesProperty);
            Assert.Equal(outcomes, actualOutcomes);
        }

        [Fact]
        public void OnOutcomesPropertyChange_NewValueIsNull_NotBuildContent()
        {
            // Arrange
            var template = new TableOutcomesTemplate();

            // Act
            TableOutcomesTemplate.OnOutcomesPropertyChange(template, null, null);

            // Assert
            Assert.Null(template.Content);
        }

        [Fact]
        public void OnOutcomesPropertyChange_NewValueHasData_BuildOutcomesContent()
        {
            // Arrange
            var template = new TableOutcomesTemplate();
            var outcomes = new List<TeamOutcome> {
                TeamOutcome.AFCChampionsLeague,
                TeamOutcome.CAFConfederationCup
            };

            // Act
            TableOutcomesTemplate.OnOutcomesPropertyChange(template, null, outcomes);

            // Assert
            var outcome1Layout = (template.Content as StackLayout).Children[0] as FlexLayout;
            var outcome1Frame = outcome1Layout.Children[0] as Frame;
            var outcome1Label = outcome1Layout.Children[1] as Label;
            Assert.Equal(template.Resources["MatchDetailTableOutComeLayout"], outcome1Layout.Style);
            Assert.Equal(template.Resources["MatchDetailTableOutComeTeamOnLeague"], outcome1Frame.Style);
            Assert.Equal(Color.FromHex("#66FF59"), outcome1Frame.BackgroundColor);
            Assert.Equal(TeamOutcome.AFCChampionsLeague.FriendlyName, outcome1Label.Text);
            Assert.Equal(template.Resources["MatchDetailTableOutComeLabel"], outcome1Label.Style);

            var outcome2Layout = (template.Content as StackLayout).Children[1] as FlexLayout;
            var outcome2Frame = outcome2Layout.Children[0] as Frame;
            var outcome2Label = outcome2Layout.Children[1] as Label;
            Assert.Equal(template.Resources["MatchDetailTableOutComeLayout"], outcome2Layout.Style);
            Assert.Equal(template.Resources["MatchDetailTableOutComeTeamOnLeague"], outcome2Frame.Style);
            Assert.Equal(Color.FromHex("#66FF59"), outcome2Frame.BackgroundColor);
            Assert.Equal(TeamOutcome.CAFConfederationCup.FriendlyName, outcome2Label.Text);
            Assert.Equal(template.Resources["MatchDetailTableOutComeLabel"], outcome2Label.Style);
        }
    }
}