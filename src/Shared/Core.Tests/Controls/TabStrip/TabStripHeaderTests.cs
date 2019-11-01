using System.Collections.Generic;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Soccer.ViewModels.MatchDetails.Information;
using LiveScore.Soccer.ViewModels.MatchDetails.Odds;
using LiveScore.Soccer.ViewModels.MatchDetails.Statistics;
using LiveScore.Soccer.ViewModels.MatchDetails.TrackerCommentary;
using Xamarin.Forms;
using Xunit;

namespace LiveScore.Core.Tests.Controls.TabStrip
{
    public class TabStripHeaderTests : IClassFixture<ResourcesFixture>, IClassFixture<ViewModelBaseFixture>
    {
        private readonly List<TabItemViewModel> tabs;

        public TabStripHeaderTests(ViewModelBaseFixture baseFixture)
        {
            tabs = new List<TabItemViewModel>
            {
                new OddsViewModel("", MatchStatus.Closed,  baseFixture.NavigationService, baseFixture.DependencyResolver, null, null) ,
                new InformationViewModel(null, baseFixture.NavigationService, baseFixture.DependencyResolver, null, null) ,
                new TrackerCommentaryViewModel(null, baseFixture.NavigationService, baseFixture.DependencyResolver, null, null) ,
                new StatisticsViewModel(null, baseFixture.NavigationService, baseFixture.DependencyResolver, null, null) ,
            };
        }

        [Fact]
        public void OnItemsSourceChanged_ItemSourceHasData_OldValueIsNull_InitTabHeader()
        {
            // Arrange
            var tabStripHeader = new TabStripHeader
            {
                ItemsSource = tabs
            };

            // Act
            var tabHeaders = (((ScrollView)tabStripHeader.Content).Children[0] as FlexLayout)?.Children;

            // Assert
            var oddsTabLayout = tabHeaders[0] as StackLayout;
            Assert.Equal("Odds", (oddsTabLayout?.Children[0] as Label)?.Text);
            Assert.NotNull(oddsTabLayout?.Children[1] as ContentView);

            var infoTabLayout = tabHeaders[1] as StackLayout;
            Assert.Equal("Info", (infoTabLayout?.Children[0] as Label)?.Text);
            Assert.NotNull(infoTabLayout?.Children[1] as ContentView);

            var trackerTabLayout = tabHeaders[2] as StackLayout;
            Assert.Equal("Tracker", (trackerTabLayout?.Children[0] as Label)?.Text);
            Assert.NotNull(trackerTabLayout?.Children[1] as ContentView);

            var statsTabLayout = tabHeaders[3] as StackLayout;
            Assert.Equal("Stats", (statsTabLayout?.Children[0] as Label)?.Text);
            Assert.NotNull(statsTabLayout?.Children[1] as ContentView);
        }
    }
}