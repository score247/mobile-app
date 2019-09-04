﻿namespace LiveScore.Soccer.Views.Templates
{
    using Enumerations;
    using FFImageLoading.Forms;
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
                stackLayout.Children.Add(BuildRedCardImage());
            }

            control.Content = stackLayout;
        }

#pragma warning disable S109 // Magic numbers should not be used

        private static CachedImage BuildRedCardImage()
        {
            return new CachedImage
            {
                Source = Images.RedCard.Value,
                DownsampleToViewSize = true,
                Margin = new Thickness(4, 0, 0, 0)
            };
        }

#pragma warning restore S109 // Magic numbers should not be used
    }
}