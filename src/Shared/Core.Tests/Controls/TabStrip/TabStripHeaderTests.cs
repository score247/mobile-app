namespace LiveScore.Core.Tests.Controls.TabStrip
{
    using System.Collections.Generic;
    using LiveScore.Core.Controls.TabStrip;
    using LiveScore.Core.Tests.Fixtures;
    using Xamarin.Forms;
    using Xunit;

    public class TabStripHeaderTests : IClassFixture<ResourcesFixture>
    {
        private readonly List<TabItemViewModel> tabs;

        public TabStripHeaderTests()
        {
            tabs = new List<TabItemViewModel>
            {
                new TabItemViewModel(null, null, null) { TabHeaderTitle = "Info" },
                new TabItemViewModel(null, null, null)  { TabHeaderTitle = "Tracker" },
            };
        }

        [Fact]
        public void OnItemsSourceChanged_ItemSourceHasData_OldValueIsNull_InitTabHeader()
        {
            // Arrange
            var tabStripHeader = new TabStripHeader();
            tabStripHeader.ItemsSource = tabs;

            // Act
            var tabHeaders = (((ScrollView)tabStripHeader.Content).Children[0] as FlexLayout)?.Children;

            // Assert
            var firstTabLayout = tabHeaders[0] as StackLayout;
            Assert.Equal("INFO", (firstTabLayout?.Children[1] as Label)?.Text);
            Assert.NotNull(firstTabLayout?.Children[2] as ContentView);

            var secondTabLayout = tabHeaders[1] as StackLayout;
            Assert.Equal("TRACKER", (secondTabLayout?.Children[1] as Label)?.Text);
            Assert.NotNull(secondTabLayout?.Children[2] as ContentView);
        }
    }
}