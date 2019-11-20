namespace LiveScore.Soccer.Views.Templates
{
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RedCardTemplate : ContentView
    {
        public RedCardTemplate()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty RedCardsProperty = BindableProperty.Create(
          nameof(RedCards),
          typeof(byte),
          typeof(RedCardTemplate),
          propertyChanged: OnRedCardCountChanged);

        public byte RedCards
        {
            get => (byte)GetValue(RedCardsProperty);
            set => SetValue(RedCardsProperty, value);
        }

        private static void OnRedCardCountChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            var control = (RedCardTemplate)bindableObject;

            if (control == null || newValue == null)
            {
                return;
            }

            var redCards = (byte)newValue;
            var stackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };

            for (var i = 0; i < redCards; i++)
            {
                stackLayout.Children.Add(BuildRedCardImage(control));
            }

            control.Content = stackLayout;
        }

        private static Label BuildRedCardImage(RedCardTemplate control)
        {
            return new Label
            {
                Style = (Style)control.Resources["RedCardIcon"]
            };
        }
    }
}