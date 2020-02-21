using System;
using Xamarin.Forms;

namespace LiveScore.Core.Controls.SearchPage
{
    public class CustomSearchBar : SearchBar
    {
        public event EventHandler Cancelled;

        public void OnCancelled()
        {
            Cancelled?.Invoke(this, EventArgs.Empty);
        }
    }
}