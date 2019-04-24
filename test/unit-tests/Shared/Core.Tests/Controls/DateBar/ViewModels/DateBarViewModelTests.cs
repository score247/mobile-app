using LiveScore.Core.Controls.DateBar.Events;
using LiveScore.Core.Controls.DateBar.Models;
using LiveScore.Core.Controls.DateBar.ViewModels;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Core.ViewModels;
using NSubstitute;
using Prism.Events;
using Prism.Navigation;
using System;
using Xunit;

namespace LiveScore.Core.Tests.Controls.DateBar.ViewModels
{
    public class DateBarViewModelTests : IClassFixture<ViewModelBaseFixture>
    {
        private readonly DateBarViewModel viewModel;
        private readonly MockViewModelBase mockViewModelBase;

        public DateBarViewModelTests(ViewModelBaseFixture viewModelBaseFixture)
        {
            viewModel = new DateBarViewModel();
            viewModel.EventAggregator = viewModelBaseFixture.EventAggregator;
            viewModel.SettingsService = viewModelBaseFixture.AppSettingsFixture.SettingsService;

            mockViewModelBase = new MockViewModelBase(Substitute.For<INavigationService>(), Substitute.For<IDepdendencyResolver>(), Substitute.For<IEventAggregator>());
        }

        [Fact]
        public void RenderCalendarItems_ShouldReturnListBaseOnNumberDisplayDays()
        {
            // Arrange
            viewModel.NumberOfDisplayDays = 3;
            var expectedCount = viewModel.NumberOfDisplayDays * 2 + 1;

            // Act
            viewModel.RenderCalendarItems();
            var actualCount = viewModel.CalendarItems.Count;

            // Assert
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void OnSelectHome_SelectingHome_NotGetEvent()
        {
            // Arrange
            viewModel.HomeIsSelected = true;

            // Act
            viewModel.SelectHomeCommand.Execute();

            // Assert
            viewModel.EventAggregator.DidNotReceive().GetEvent<DateBarItemSelectedEvent>();
        }

        [Fact]
        public void OnSelectHome_NotSelectingHome_InjectEventAggregator()
        {
            // Arrange
            viewModel.HomeIsSelected = false;
            viewModel.NumberOfDisplayDays = 2;
            viewModel.RenderCalendarItems();
            viewModel.EventAggregator.GetEvent<DateBarItemSelectedEvent>().Returns(new DateBarItemSelectedEvent());

            // Act
            viewModel.SelectHomeCommand.Execute();

            // Assert
            viewModel.EventAggregator.Received(1).GetEvent<DateBarItemSelectedEvent>();
        }

        [Fact]
        public void SelectDateCommand_DifferentCurrentDate_InjectEventAggregator()
        {
            // Arrange     
            viewModel.NumberOfDisplayDays = 2;
            viewModel.RenderCalendarItems();
            viewModel.EventAggregator.GetEvent<DateBarItemSelectedEvent>().Returns(new DateBarItemSelectedEvent());

            // Act
            viewModel.SelectDateCommand.Execute(new DateBarItem { Date = DateTime.Now });

            // Assert
            viewModel.EventAggregator.Received(1).GetEvent<DateBarItemSelectedEvent>();
        }

        [Fact]
        public void InitializeBindingContext_ShouldRenderCalendarItems()
        {
            // Arrange     
            viewModel.NumberOfDisplayDays = 2;
            viewModel.HomeIsSelected = true;

            // Act
            viewModel.InitializeBindingContext(mockViewModelBase);

            // Assert
            Assert.NotEmpty(viewModel.CalendarItems);
        }
    }

    public class MockViewModelBase : ViewModelBase
    {
        public MockViewModelBase(
            INavigationService navigationService, 
            IDepdendencyResolver serviceLocator, 
            IEventAggregator eventAggregator) : base(navigationService, serviceLocator, eventAggregator)
        {

        }
    }
}
