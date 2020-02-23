using System.Windows.Input;
using LiveScore.Common.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Core.Controls.CustomSearchBar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchBarTemplate : ContentView
    {
        public SearchBarTemplate()
        {
            InitializeComponent();

            Layout.BindingContext = new SearchViewModel();
        }

        public void OnAppearing()
        {
            SearchTextBox.Focus();
        }

        public static readonly BindableProperty PlaceHolderTextProperty
            = BindableProperty.Create(
                nameof(PlaceHolderText),
                typeof(string),
                typeof(SearchBarTemplate),
                propertyChanged: OnPlaceHolderTextChanged);

        private static void OnPlaceHolderTextChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is SearchBarTemplate control
                && newvalue is string placeHolder
                && control.Layout.BindingContext is SearchViewModel viewModel)
            {
                viewModel.PlaceholderText = placeHolder;
            }
        }

        public string PlaceHolderText
        {
            get => GetValue(PlaceHolderTextProperty) as string;
            set => SetValue(PlaceHolderTextProperty, value);
        }

        public static readonly BindableProperty SearchCommandProperty
            = BindableProperty.Create(
                nameof(SearchCommand),
                typeof(ICommand),
                typeof(SearchBarTemplate),
                propertyChanged: OnSearchCommandChanged);

        private static void OnSearchCommandChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is SearchBarTemplate control
                && newvalue is ICommand searchCommand
                && control.Layout.BindingContext is SearchViewModel viewModel)
            {
                viewModel.SearchCommand = searchCommand;
            }
        }

        public ICommand SearchCommand
        {
            get => GetValue(SearchCommandProperty) as ICommand;
            set => SetValue(SearchCommandProperty, value);
        }

        public static readonly BindableProperty CancelCommandProperty
            = BindableProperty.Create(
                nameof(CancelCommand),
                typeof(ICommand),
                typeof(SearchBarTemplate),
                propertyChanged: OnCancelCommandChanged);

        private static void OnCancelCommandChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is SearchBarTemplate control
                && newvalue is ICommand command
                && control.Layout.BindingContext is SearchViewModel viewModel)
            {
                viewModel.CancelCommand = command as DelegateAsyncCommand;
            }
        }

        public ICommand CancelCommand
        {
            get => GetValue(CancelCommandProperty) as ICommand;
            set => SetValue(CancelCommandProperty, value);
        }

        private async void SearchTextBox_Cancelled(object sender, System.EventArgs e)
        {
            if (Layout.BindingContext is SearchViewModel viewModel)
            {
                await viewModel.CancelCommand?.ExecuteAsync();
            }
        }
    }
}