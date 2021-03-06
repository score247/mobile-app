namespace LiveScore.Core.Views.Templates
{
    using System.Windows.Input;
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
            favoriteButton.BindingContext = navigationTitleTemplate;
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

        public static readonly BindableProperty VisibleFavoriteProperty
            = BindableProperty.Create(
                propertyName: "VisibleFavorite",
                returnType: typeof(bool),
                declaringType: typeof(NavigationTitleTemplate),
                defaultValue: false);

        public bool VisibleFavorite
        {
            get => (bool)GetValue(VisibleFavoriteProperty);
            set => SetValue(VisibleFavoriteProperty, value);
        }

        public static readonly BindableProperty IsFavoriteProperty
            = BindableProperty.Create("IsFavorite", typeof(bool), typeof(NavigationTitleTemplate), false);

        public bool IsFavorite
        {
            get => (bool)GetValue(IsFavoriteProperty);
            set => SetValue(IsFavoriteProperty, value);
        }

        public static readonly BindableProperty FavoriteCommandProperty
            = BindableProperty.Create("FavoriteCommand", typeof(ICommand), typeof(NavigationTitleTemplate));

        public ICommand FavoriteCommand
        {
            get => (ICommand)GetValue(FavoriteCommandProperty);
            set => SetValue(FavoriteCommandProperty, value);
        }
    }
}