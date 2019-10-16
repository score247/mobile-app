namespace LiveScore.Core.Controls.TabStrip.EventArgs
{
    public class TabStripItemTappedEventArgs
    {
        public TabStripItemTappedEventArgs(byte index)
        {
            Index = index;
        }

        public byte Index { get; }
    }
}