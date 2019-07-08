namespace LiveScore.Core.Tests.Controls.TabStrip
{
    using LiveScore.Core.Tests.Fixtures;
    using Core.Controls.TabStrip;
    using Xunit;
    using System.Collections.Generic;
    using Xamarin.Forms;
    using NSubstitute;
    using PanCardView.EventArgs;
    using PanCardView.Enums;
    using System.Linq;

    public class TabStripTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {
        private readonly ViewModelBaseFixture baseFixture;
        private readonly TabStrip tabStrip;

        public TabStripTests(ViewModelBaseFixture baseFixture)
        {
            this.baseFixture = baseFixture;
            tabStrip = new TabStrip
            {
                SelectedTabIndex = 0,
                ItemsSource = new List<TabItemViewModelBase> {
                Substitute.For<TabItemViewModelBase>(),
                Substitute.For<TabItemViewModelBase>()
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

        [Fact]
        public void OnItemsSourceChanged_ItemBeforeAppearing_ChangeSelectedTab()
        {
            // Arrange
            var tabCarouselView = ((tabStrip.Content as StackLayout)?.Children[1] as PanCardView.CarouselView);

            // Act
            TabStrip.TabContent_ItemBeforeAppearing(tabCarouselView, new ItemBeforeAppearingEventArgs(InteractionType.User, true, 1, tabStrip.ItemsSource.ToList()[1]));

            // Assert
            Assert.Equal(1, tabStrip.SelectedTabIndex);
        }
    }
}