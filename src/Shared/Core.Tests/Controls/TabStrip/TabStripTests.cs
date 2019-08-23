namespace LiveScore.Core.Tests.Controls.TabStrip
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Controls.TabStrip;
    using LiveScore.Core.Tests.Fixtures;
    using NSubstitute;
    using PanCardView.Enums;
    using PanCardView.EventArgs;
    using Xamarin.Forms;
    using Xunit;

    public class TabStripTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {
        private readonly TabStrip tabStrip;

        public TabStripTests(ViewModelBaseFixture baseFixture)
        {
            tabStrip = new TabStrip
            {
                ItemsSource = new List<TabItemViewModel> {
                    Substitute.For<TabItemViewModel>(),
                    Substitute.For<TabItemViewModel>()
                }
            };
        }

        [Fact]
        public void OnItemsSourceChanged_ItemAppearing_CallViewModelAppearing()
        {
            // Arrange
            var tabCarouselView = ((tabStrip.Content as StackLayout)?.Children[1] as PanCardView.CarouselView);

            // Act
            TabStrip.TabContent_ItemAppearing(tabCarouselView, new ItemAppearingEventArgs(InteractionType.User, true, 1, tabStrip.ItemsSource.ToList()[1]));

            // Assert
            Assert.Equal(1, tabStrip.SelectedTabIndex);
            tabStrip.ItemsSource.ToList()[1].Received(1).OnAppearing();
        }

        [Fact]
        public void OnItemsSourceChanged_ItemDisappearing_CallViewModelAppearing()
        {
            // Arrange
            var tabCarouselView = ((tabStrip.Content as StackLayout)?.Children[1] as PanCardView.CarouselView);

            // Act
            TabStrip.TabContent_ItemDisappearing(tabCarouselView, new ItemDisappearingEventArgs(InteractionType.User, true, 1, tabStrip.ItemsSource.ToList()[1]));

            // Assert
            tabStrip.ItemsSource.ToList()[1].Received(1).OnDisappearing();
        }
    }
}