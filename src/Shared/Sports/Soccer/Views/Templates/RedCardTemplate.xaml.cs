namespace LiveScore.Soccer.Views.Templates
{
    using LiveScore.Soccer.Enumerations;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RedCardTemplate : ContentView
    {
        public RedCardTemplate()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty RedCardCountProperty = BindableProperty.Create(
          nameof(RedCardCount),
          typeof(int),
          typeof(RedCardTemplate),
          propertyChanged: OnRedCardCountChanged);

        public int RedCardCount
        {
            get { return (int)GetValue(RedCardCountProperty); }
            set { SetValue(RedCardCountProperty, value); }
        }

        private static void OnRedCardCountChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RedCardTemplate)bindable;

            if (control == null || newValue == null)
            {
                return;
            }

            var redCardCount = (int)newValue;

            for (int i = 0; i < redCardCount; i++)
            {
                control.RedCardContainer.Children.Add(BuildRedCardImage());
            }
        }

        private static Image BuildRedCardImage()
        {
            return new Image
            {
                Source = Images.RedCard.Value,
                Margin = new Thickness(4, 0, 0, 0)
            };
        }
    }
}