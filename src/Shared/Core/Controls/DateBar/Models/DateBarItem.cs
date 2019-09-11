namespace LiveScore.Core.Controls.DateBar.Models
{
    using System;

    public class DateBarItem : IEquatable<DateBarItem>
    {
        public DateBarItem(DateTime date) : this(date, false)
        {
        }

        public DateBarItem(DateTime date, bool isSelected)
        {
            Date = date.Date;
            IsSelected = isSelected;
        }

        public DateTime Date { get; }

        public bool IsSelected { get; set; }

        public bool Equals(DateBarItem other)
            => other != null && Date.Date == other.Date.Date;
    }
}