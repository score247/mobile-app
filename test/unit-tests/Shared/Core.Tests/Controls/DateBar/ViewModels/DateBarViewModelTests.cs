using LiveScore.Core.Controls.DateBar.ViewModels;
using LiveScore.Core.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LiveScore.Core.Tests.Controls.DateBar.ViewModels
{
    public class DateBarViewModelTests
    {
        private readonly DateBarViewModel viewModel;

        public DateBarViewModelTests()
        {
            viewModel = new DateBarViewModel();
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
    }
}
