using LiveScore.Core.Controls.DateBar.Events;
using LiveScore.Core.Controls.DateBar.Models;
using LiveScore.Core.Controls.DateBar.ViewModels;
using LiveScore.Core.Tests.Fixtures;
using NSubstitute;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xunit;

namespace LiveScore.Core.Tests.Controls.DateBar.ViewModels
{
    public class DateBarViewModelTests : IClassFixture<ViewModelBaseFixture>
    {        
        private readonly DateBarViewModel viewModel;

        public DateBarViewModelTests(ViewModelBaseFixture viewModelBaseFixture)
        {
            viewModel = new DateBarViewModel();
            viewModel.EventAggregator = viewModelBaseFixture.EventAggregator;
            viewModel.SettingsService = viewModelBaseFixture.AppSettingsFixture.SettingsService;
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
    }
}
