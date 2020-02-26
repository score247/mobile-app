using System;
using Xamarin.Forms;

namespace LiveScore.Core.Controls.CustomSearchBar
{
    public class CustomSearchBar : SearchBar
    {
        public event EventHandler Cancelled;

        public void OnCancelled()
        {
            Cancelled?.Invoke(this, EventArgs.Empty);
        }

        public static readonly BindableProperty ShowCancelButtonProperty
            = BindableProperty.Create(
                nameof(ShowCancelButton),
                typeof(bool),
                typeof(SearchBarTemplate));

        public bool ShowCancelButton
        {
            get => (bool)GetValue(ShowCancelButtonProperty);
            set => SetValue(ShowCancelButtonProperty, value);
        }
    }
}