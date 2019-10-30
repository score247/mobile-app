namespace LiveScore.Core.Controls.TabStrip.EventArgs
{
    public class TabItemScrollingEventArgs
    {
        public const string EventName = "TabItemScrolling";

        public TabItemScrollingEventArgs(double scrollOffset)
        {
            ScrollOffset = scrollOffset;
        }

        public double ScrollOffset { get; }
    }
}