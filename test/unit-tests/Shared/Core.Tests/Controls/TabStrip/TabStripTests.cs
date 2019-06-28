namespace LiveScore.Core.Tests.Controls.TabStrip
{
    using KellermanSoftware.CompareNetObjects;
    using LiveScore.Core.Controls.TabStrip;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Core.Tests.Mocks;
    using LiveScore.Core.ViewModels;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xamarin.Forms;
    using Xunit;

    public class TabStripTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {
        private readonly List<TabModel> tabs;

        public TabStripTests()
        {
            tabs = new List<TabModel>
            {
                new TabModel
                {
                    Name = "Info",
                    Template = new ContentView(),
                    ViewModel = Substitute.For<ViewModelBase>()
                },
                 new TabModel
                {
                    Name = "Tracker",
                    Template = new ContentView(),
                    ViewModel = Substitute.For<ViewModelBase>()
                }
            };
        }

        [Fact]
        public void OnItemsSourceChanged_ItemSourceHasData_OldValueIsNull_InitDefaultTab()
        {
            // Arrange
            var firstTab = tabs.FirstOrDefault();
            var tabStrip = new TabStrip();
            tabStrip.ItemsSource = tabs;

            // Act
            var tabContent = (((StackLayout)tabStrip.Content).Children[1] as StackLayout).Children[0] as ContentView;

            // Assert
            Assert.Equal(tabContent.Content, firstTab.Template);
            Assert.Equal(tabContent.BindingContext, firstTab.ViewModel);
            firstTab.ViewModel.Received(1).OnAppearing();
        }

        [Fact]
        public void OnItemsSourceChanged_WhenTabChange_ChangeTab()
        {
            // Arrange
            var selectedIndex = 1;
            var selectedTab = tabs[selectedIndex];
            var tabStrip = new TabStrip();
            tabStrip.ItemsSource = tabs;
            tabStrip.ItemsSource = tabs;

            // Act
            MessagingCenter.Send(nameof(TabStrip), "TabChange", selectedIndex);

            // Assert
            var tabContent = (((StackLayout)tabStrip.Content).Children[1] as StackLayout).Children[0] as ContentView;
            tabs.FirstOrDefault()?.ViewModel.Received(1).OnDisappearing();
            selectedTab.ViewModel.Received(1).OnAppearing();
            Assert.Equal(tabContent.Content, selectedTab.Template);
            Assert.Equal(tabContent.BindingContext, selectedTab.ViewModel);
        }

        [Fact]
        public void OnSwipe_Left_ChangeTab()
        {
            // Arrange
            var tabStrip = new TabStrip();
            tabStrip.ItemsSource = tabs;
            var tabContentLayout = ((StackLayout)tabStrip.Content).Children[1] as StackLayout;

            // Act
            (tabContentLayout.GestureRecognizers[0] as SwipeGestureRecognizer)?.SendSwiped(tabContentLayout, SwipeDirection.Left);
            (tabContentLayout.GestureRecognizers[0] as SwipeGestureRecognizer)?.SendSwiped(tabContentLayout, SwipeDirection.Left);
            (tabContentLayout.GestureRecognizers[0] as SwipeGestureRecognizer)?.SendSwiped(tabContentLayout, SwipeDirection.Left);

            // Assert
            var tabContent = tabContentLayout.Children[0] as ContentView;
            var selectedTab = tabs[1];
            tabs.FirstOrDefault()?.ViewModel.Received().OnDisappearing();
            selectedTab.ViewModel.Received().OnAppearing();
            Assert.Equal(tabContent.Content, selectedTab.Template);
            Assert.Equal(tabContent.BindingContext, selectedTab.ViewModel);
        }

        [Fact]
        public void OnSwipe_Right_ChangeTab()
        {
            // Arrange
            var tabStrip = new TabStrip();
            tabStrip.ItemsSource = tabs;
            var tabContentLayout = ((StackLayout)tabStrip.Content).Children[1] as StackLayout;

            // Act
            (tabContentLayout.GestureRecognizers[1] as SwipeGestureRecognizer)?.SendSwiped(tabContentLayout, SwipeDirection.Right);
            (tabContentLayout.GestureRecognizers[1] as SwipeGestureRecognizer)?.SendSwiped(tabContentLayout, SwipeDirection.Right);
            (tabContentLayout.GestureRecognizers[1] as SwipeGestureRecognizer)?.SendSwiped(tabContentLayout, SwipeDirection.Right);

            // Assert
            var tabContent = tabContentLayout.Children[0] as ContentView;
            var selectedTab = tabs[0];
            selectedTab.ViewModel.Received().OnAppearing();
            Assert.Equal(tabContent.Content, selectedTab.Template);
            Assert.Equal(tabContent.BindingContext, selectedTab.ViewModel);
        }
    }
}