﻿namespace LiveScore.Soccer.Views.Templates
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

        public static readonly BindableProperty RedCardsProperty = BindableProperty.Create(
          nameof(RedCards),
          typeof(byte),
          typeof(RedCardTemplate),
          propertyChanged: OnRedCardCountChanged);

        public byte RedCards
        {
            get { return (byte)GetValue(RedCardsProperty); }
            set { SetValue(RedCardsProperty, value); }
        }

        private static void OnRedCardCountChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RedCardTemplate)bindable;

            if (control == null || newValue == null)
            {
                return;
            }

            var redCards = (byte)newValue;
            var stackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };

            for (int i = 0; i < redCards; i++)
            {
                stackLayout.Children.Add(BuildRedCardImage());
            }

            control.Content = stackLayout;
        }

#pragma warning disable S109 // Magic numbers should not be used

        private static Image BuildRedCardImage()
        {
            return new Image
            {
                Source = Images.RedCard.Value,
                Margin = new Thickness(4, 0, 0, 0)
            };
        }

#pragma warning restore S109 // Magic numbers should not be used
    }
}