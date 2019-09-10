﻿namespace LiveScore.Core.Controls.DateBar.EventArgs
{
    using System;

    public class DateBarItemTappedEventArgs
    {
        public DateBarItemTappedEventArgs(int index, DateTime date)
        {
            Index = index;
            Date = date;
        }

        public int Index { get; }

        public DateTime Date { get; }
    }
}