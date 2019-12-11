namespace LiveScore.Core.Views.Templates
{
    using System;
    using LiveScore.Core.ViewModels;
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

        private async void ShowSportSelection(object sender, EventArgs args)
        {
            var vm = BindingContext as ViewModelBase;
            if (vm == null)
            {
                return;
            }

            if (vm.IsShowSportSelection)
            {
                await arrowDownSelection.RotateTo(0, 200, Easing.SinIn);
            }
            else
            {
                await arrowDownSelection.RotateTo(180, 200, Easing.SinOut);
            }

            vm.IsShowSportSelection = !vm.IsShowSportSelection;
        }
    }
}