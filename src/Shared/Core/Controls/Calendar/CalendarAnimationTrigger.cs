using System.Threading.Tasks;
using Xamarin.Forms;

namespace LiveScore.Core.Controls.Calendar
{
    public class CalendarAnimationTrigger : TriggerAction<Grid>
    {
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

        private static async Task HideCalendar(Grid grid)
        {
            var height = grid.Children[0]?.Height ?? 0;

            await grid.TranslateTo(0, -height, 250, Easing.Linear);
        }

        private static async Task ShowCalendar(Grid grid)
        {
            await grid.TranslateTo(0, 0, 250, Easing.Linear);
        }
    }
}