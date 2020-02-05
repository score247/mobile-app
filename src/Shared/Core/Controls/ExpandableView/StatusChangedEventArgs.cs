using System;

namespace LiveScore.Core.Controls.ExpandableView
{
    public sealed class StatusChangedEventArgs : EventArgs
    {
        public StatusChangedEventArgs(ExpandStatus status)
        {
            Status = status;
        }

        public ExpandStatus Status { get; }
    }
}