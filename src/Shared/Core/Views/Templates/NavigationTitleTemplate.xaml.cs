namespace LiveScore.Core.Views.Templates
{
    using System;
    using Rg.Plugins.Popup.Extensions;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigationTitleTemplate : ContentView
    {
        public NavigationTitleTemplate()
        {
            InitializeComponent();
            var navigationTitleTemplate = this;
            sportLabel.BindingContext = navigationTitleTemplate;
        }

        public static readonly BindableProperty TitleProperty
            = BindableProperty.Create("Title", typeof(string), typeof(NavigationTitleTemplate));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly BindableProperty CurrentSportProperty
           = BindableProperty.Create(nameof(CurrentSport), typeof(string), typeof(NavigationTitleTemplate));

        public string CurrentSport
        {
            get => (string)GetValue(CurrentSportProperty);
            set => SetValue(CurrentSportProperty, value);
        }

        private async void OnSportSelectionClosed(object sender, EventArgs eventArgs)
        {
            await Navigation.PopAllPopupAsync();
            await arrowDownSelection.RotateTo(0, 200, Easing.SinOut);
        }

        private async void ShowSportSelection(object sender, EventArgs args)
        {
            await arrowDownSelection.RotateTo(180, 200, Easing.SinOut);

            var selectionPage = new SelectSportView();
            selectionPage.CallbackEvent += OnSportSelectionClosed;
            await Navigation.PushPopupAsync(selectionPage);
        }
    }
}