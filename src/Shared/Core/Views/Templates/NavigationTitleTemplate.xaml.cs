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

        private void ShowSportSelection(object sender, EventArgs args)
        {
            var vm = BindingContext as ViewModelBase;
            if (vm == null)
            {
                return;
            }

            vm.IsShowSportSelection = !vm.IsShowSportSelection;
            arrowDownSelection.IsVisible = !vm.IsShowSportSelection;
            arrowUpSelection.IsVisible = vm.IsShowSportSelection;
        }
    }
}