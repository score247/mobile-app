using LiveScore.Core.Controls.CircleProgressBar;
using Xamarin.Forms;
using Xunit;

namespace LiveScore.Core.Tests.Controls.CircleProgressBar
{
    public class CircularProgressTests
    {
        [Fact]
        public void CircularProgress_Init_AssignedCorrectData()
        {
            var progress = new CircularProgress 
            { 
                BackColor = Color.White,
                ForeColor = Color.Silver,
                AnimationDuration = 1000,
                TextSize = 15,
                TextColor = Color.AliceBlue,
                BarHeight = 5,
                Maximum = 100,
                Value = 17,
                Text = "33/35"
            };

            Assert.Equal(Color.White, progress.BackColor);
            Assert.Equal(Color.Silver, progress.ForeColor);
            Assert.Equal(1000, progress.AnimationDuration);
            Assert.Equal(15, progress.TextSize);
            Assert.Equal(Color.AliceBlue, progress.TextColor);
            Assert.Equal(5, progress.BarHeight);
            Assert.Equal(100, progress.Maximum);
            Assert.Equal(17, progress.Value);
            Assert.Equal("33/35", progress.Text);
        }
    }
}
