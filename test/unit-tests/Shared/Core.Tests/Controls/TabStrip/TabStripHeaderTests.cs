namespace LiveScore.Core.Tests.Controls.TabStrip
{
    using System.Collections.Generic;
    using LiveScore.Core.Controls.TabStrip;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Core.ViewModels;
    using NSubstitute;
    using Xamarin.Forms;
    using Xunit;

    public class TabStripHeaderTests : IClassFixture<ResourcesFixture>
    {
        //private readonly List<TabModel> tabs;

        public TabStripHeaderTests()
        {
            //tabs = new List<TabModel>
            //{
            //    new TabModel
            //    {
            //        Name = "Info",
            //        Template = new DataTemplate(),
            //        ViewModel = Substitute.For<ViewModelBase>()
            //    },
            //     new TabModel
            //    {
            //        Name = "Tracker",
            //        Template = new DataTemplate(),
            //        ViewModel = Substitute.For<ViewModelBase>()
            //    }
            //};
        }

        //[Fact]
        //public void OnItemsSourceChanged_ItemSourceHasData_OldValueIsNull_InitTabHeader()
        //{
        //    // Arrange
        //    var tabStripHeader = new TabStripHeader();
        //    tabStripHeader.ItemsSource = tabs;

        //    // Act
        //    var tabHeaders = (((ScrollView)tabStripHeader.Content).Children[0] as FlexLayout)?.Children;

        //    // Assert
        //    var firstTabLayout = tabHeaders[0] as StackLayout;
        //    Assert.Equal("INFO", (firstTabLayout?.Children[0] as Label)?.Text);
        //    Assert.NotNull(firstTabLayout?.Children[1] as ContentView);

        //    var secondTabLayout = tabHeaders[1] as StackLayout;
        //    Assert.Equal("TRACKER", (secondTabLayout?.Children[0] as Label)?.Text);
        //    Assert.NotNull(secondTabLayout?.Children[1] as ContentView);
        //}
    }
}