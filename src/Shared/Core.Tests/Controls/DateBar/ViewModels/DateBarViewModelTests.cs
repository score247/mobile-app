namespace LiveScore.Core.Tests.Controls.DateBar.ViewModels
{
    using System;
    using System.Collections.Generic;
    using KellermanSoftware.CompareNetObjects;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Controls.DateBar.Events;
    using LiveScore.Core.Controls.DateBar.Models;
    using LiveScore.Core.Controls.DateBar.ViewModels;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Core.ViewModels;
    using Prism.Events;
    using Prism.Navigation;
    using Xunit;

    public class DateBarViewModelTests : IClassFixture<ViewModelBaseFixture>, IDisposable
    {
        private readonly DateBarViewModel viewModel;
        private readonly MockViewModelBase mockViewModelBase;
        private readonly CompareLogic comparer;
        private DateRange currentDateRange;

        public DateBarViewModelTests(ViewModelBaseFixture viewModelBaseFixture)
        {
            comparer = viewModelBaseFixture.CommonFixture.Comparer;
            viewModel = new DateBarViewModel
            {
                EventAggregator = viewModelBaseFixture.EventAggregator,
                SettingsService = viewModelBaseFixture.AppSettingsFixture.SettingsService,
                NumberOfDisplayDays = 3
            };

            viewModel.EventAggregator.GetEvent<DateBarItemSelectedEvent>().Subscribe(OnSelectDateBarItem);
            viewModel.RenderCalendarItems();
            mockViewModelBase = new MockViewModelBase(
                viewModelBaseFixture.NavigationService,
                viewModelBaseFixture.DependencyResolver,
                new EventAggregator());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            viewModel.EventAggregator.GetEvent<DateBarItemSelectedEvent>()?.Unsubscribe(OnSelectDateBarItem);
            //currentDateRange = null;
        }

        private void OnSelectDateBarItem(DateRange dateRange)
        {
            currentDateRange = dateRange;
        }

        [Fact]
        public void RenderCalendarItems_ShouldReturnListBaseOnNumberDisplayDays()
        {
            // Arrange
            viewModel.NumberOfDisplayDays = 2;

            // Act
            viewModel.RenderCalendarItems();

            // Assert
            var expectedCalendarItems = new List<DateBarItem>(new List<DateBarItem> {
                new DateBarItem (DateTime.Today.AddDays(-2) ),
                new DateBarItem(DateTime.Today.AddDays(-1) ),
                new DateBarItem(DateTime.Today),
                new DateBarItem(DateTime.Today.AddDays(1) ),
                new DateBarItem(DateTime.Today.AddDays(2) )
            });

            Assert.True(comparer.Compare(expectedCalendarItems, viewModel.CalendarItems).AreEqual);
        }

        [Fact]
        public void OnSelectHome_NotSelectingHome_FireDateBarItemSelectedEvent()
        {
            // Arrange
            viewModel.HomeIsSelected = false;

            // Act
            viewModel.SelectHomeCommand.Execute();

            // Assert
            Assert.Equal(DateRange.FromYesterdayUntilNow().From, currentDateRange.From);
            Assert.Equal(DateRange.FromYesterdayUntilNow().To, currentDateRange.To);
        }

        [Fact]
        public void OnSelectHome_Always_NoItemIsSelected()
        {
            // Arrange
            viewModel.HomeIsSelected = false;

            // Act
            viewModel.SelectHomeCommand.Execute();

            // Assert
            Assert.DoesNotContain(viewModel.CalendarItems, item => item.IsSelected);
        }

        [Fact]
        public void SelectDateCommand_DifferentCurrentDate_FireDateBarItemSelectedEvent()
        {
            // Act
            viewModel.SelectDateCommand.Execute(new DateBarItem(DateTime.Today));

            // Assert
            Assert.Equal(DateTime.Today, currentDateRange.From);
            Assert.Equal(DateTime.Today.EndOfDay(), currentDateRange.To);
        }

        [Fact]
        public void SelectDateCommand_DifferentCurrentDate_HasExpectedSelectedItem()
        {
            // Act
            viewModel.SelectDateCommand.Execute(new DateBarItem(DateTime.Today));

            // Assert
            Assert.Contains(viewModel.CalendarItems, item => item.IsSelected && item.Date.Equals(DateTime.Today));
        }

        [Fact]
        public void InitializeBindingContext_ShouldRenderExpectedCalendarItems()
        {
            // Arrange
            viewModel.NumberOfDisplayDays = 2;

            // Act
            viewModel.InitializeBindingContext(mockViewModelBase);

            // Assert
            Assert.NotEmpty(viewModel.CalendarItems);
        }

        [Fact]
        public void InitializeBindingContext_Always_ExecuteSelectHomeCommand()
        {
            // Act
            viewModel.InitializeBindingContext(mockViewModelBase);

            // Assert
            Assert.True(viewModel.HomeIsSelected);
        }
    }

    public class MockViewModelBase : ViewModelBase
    {
        public MockViewModelBase(
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            IEventAggregator eventAggregator) : base(navigationService, serviceLocator, eventAggregator)
        {
        }
    }
}