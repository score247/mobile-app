namespace LiveScore.Core.Controls.TabStrip
{
    using System.Collections;
    using System.Collections.Generic;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabStripHeader : ContentView
    {
        public TabStripHeader()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
              "ItemsSource",
              typeof(IEnumerable),
              typeof(TabStripHeader),
              propertyChanged: OnItemsSourceChanged);

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty PositionProperty = BindableProperty.Create(
              "Position",
              typeof(int),
              typeof(TabStripHeader),
              defaultBindingMode: BindingMode.TwoWay,
              propertyChanging: OnPositionChanging);

        public int Position
        {
            get { return (int)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (TabStripHeader)bindable;

            if (control != null)
            {
                var tabs = (IList<TabModel>)newValue;

                for (int index = 0; index < tabs.Count; index++)
                {
                    var item = tabs[index];
                    var itemLayout = CreateItemLayout(control, index);
                    var itemLabel = new Label
                    {
                        Text = item.Name.ToUpperInvariant(),
                        Style = (Style)control.Resources["TabText"]
                    };
                    ContentView activeTabIndicator = CreateActiveTabIndicator(control, index);

                    itemLayout.Children.Add(itemLabel);
                    itemLayout.Children.Add(activeTabIndicator);
                    control.scrollLayOut.Children.Add(itemLayout);
                }

                MessagingCenter.Subscribe<string, string>("Tab", "TabChange", (_, index) =>
                {
                    var children = control.scrollLayOut.Children;
                    var currentPosition = int.Parse(index);

                    for (int i = 0; i < children.Count; i++)
                    {
                        var childLayout = (StackLayout)children[i];
                        childLayout.Children[1].IsVisible = i == currentPosition;
                    }

                    control.scrollView.ScrollToAsync(children[currentPosition], ScrollToPosition.Center, true);
                });
            }
        }

        private static StackLayout CreateItemLayout(TabStripHeader control, int index)
        {
            var itemLayout = new StackLayout
            {
                Style = (Style)control.Resources["Tab"]
            };

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (sender, e) =>
            {
                control.Position = index;
                MessagingCenter.Send("Tab", "TabChange", index.ToString());
            };

            itemLayout.GestureRecognizers.Add(tapGestureRecognizer);

            return itemLayout;
        }

        private static ContentView CreateActiveTabIndicator(TabStripHeader control, int index)
        {
            return new ContentView
            {
                Content = new BoxView
                {
                    Style = (Style)control.Resources["TabActiveLine"]
                },

                IsVisible = index == 0
            };
        }

        private static void OnPositionChanging(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (TabStripHeader)bindable;

            if (control != null)
            {
                var children = control.scrollLayOut.Children;
                var currentPosition = (int)newValue;

                for (int i = 0; i < children.Count; i++)
                {
                    var childLayout = (StackLayout)children[i];
                    childLayout.Children[1].IsVisible = i == currentPosition;
                }

                control.scrollView.ScrollToAsync(children[currentPosition], ScrollToPosition.Center, true);
            }
        }
    }
}