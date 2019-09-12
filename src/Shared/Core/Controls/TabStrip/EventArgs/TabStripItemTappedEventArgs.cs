namespace LiveScore.Core.Controls.TabStrip.EventArgs
{
    public class TabStripItemTappedEventArgs
    {
        public TabStripItemTappedEventArgs(int index)
        {
            Index = index;
        }

        public int Index { get; }
    }
}
