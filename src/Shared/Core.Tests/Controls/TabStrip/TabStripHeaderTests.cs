using System.Collections.Generic;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Soccer.ViewModels.DetailStats;
using LiveScore.Soccer.ViewModels.MatchDetailInfo;
using LiveScore.Soccer.ViewModels.MatchDetails.DetailOdds;
using LiveScore.Soccer.ViewModels.MatchDetails.DetailTracker;
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
                new DetailOddsViewModel("", MatchStatus.Closed,  baseFixture.NavigationService, baseFixture.DependencyResolver, null, null) ,
                new DetailInfoViewModel("", baseFixture.NavigationService, baseFixture.DependencyResolver, null, null) ,
                new DetailTrackerViewModel(null, baseFixture.NavigationService, baseFixture.DependencyResolver, null, null) ,
                new DetailStatsViewModel(null, baseFixture.NavigationService, baseFixture.DependencyResolver, null, null) ,
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