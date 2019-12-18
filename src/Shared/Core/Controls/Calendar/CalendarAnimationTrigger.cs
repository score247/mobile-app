using System.Threading.Tasks;
using Xamarin.Forms;

namespace LiveScore.Core.Controls.Calendar
{
    public class CalendarAnimationTrigger : TriggerAction<Grid>
    {
        private double screenHeight;

        public AnimationAction Action { get; set; }

        public enum AnimationAction
        {
            Show, Hide
        }

        protected override async void Invoke(Grid sender)
        {
            if (sender != null)
            {
                if (Action == AnimationAction.Show)
                {
                    await ShowCalendar(sender);
                }
                else
                {
                    await HideCalendar(sender);
                }
            }
        }

        private async Task HideCalendar(Grid grid)
        {
            var height = grid.Children[0]?.Height ?? 0;

            if (screenHeight <= 0)
            {
                screenHeight = grid.Height + height;
            }

            if (screenHeight > 0)
            {
                await grid.LayoutTo(new Rectangle(0, -height - 10, grid.Width, screenHeight + 10), 250, Easing.Linear);
            }
        }

        private async Task ShowCalendar(Grid grid)
        {
            await grid.LayoutTo(new Rectangle(0, 0, grid.Width, grid.Height), 250, Easing.Linear);
        }
    }
}