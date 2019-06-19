namespace LiveScore.Common.Tests.Controls
{
    using LiveScore.Common.Controls;
    using Xamarin.Forms;
    using Xunit;

    public class InfiniteListViewTests
    {
        [Fact]
        public void LoadMoreCommand_Always_GetSetExpectedValue()
        {
            // Arrange
            var command = new Command(() => { });
            var listview = new InfiniteListView
            {
                LoadMoreCommand = command
            };

            // Act
            var actual = listview.LoadMoreCommand;

            // Assert
            Assert.Equal(actual, command);
        }

        [Fact]
        public void LoadMoreCommand_Always_GetSetToProperty()
        {
            // Arrange
            var command = new Command(() => { });
            var listview = new InfiniteListView
            {
                LoadMoreCommand = command
            };

            // Act
            var actual = listview.GetValue(InfiniteListView.LoadMoreCommandProperty);

            // Assert
            Assert.Equal(actual, command);
        }
    }
}