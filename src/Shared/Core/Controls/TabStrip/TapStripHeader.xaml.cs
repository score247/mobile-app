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
              nameof(ItemsSource), typeof(IEnumerable), typeof(TabStripHeader), propertyChanged: OnItemsSourceChanged);

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (TabStripHeader)bindable;

            if (control == null || newValue == null)
            {
                MessagingCenter.Unsubscribe<string, string>(nameof(TabStrip), "TabChange");
                return;
            }

            var tabs = (IList<TabModel>)newValue;
            InitTabHeader(control, tabs);
            SubscribeTabChange(control);
        }

        private static void SubscribeTabChange(TabStripHeader control)
        {
            MessagingCenter.Subscribe<string, string>(nameof(TabStrip), "TabChange", (_, index) =>
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

        private static void InitTabHeader(TabStripHeader control, IList<TabModel> tabs)
        {
            for (int index = 0; index < tabs.Count; index++)
            {
                var item = tabs[index];
                var itemLayout = CreateItemLayout(control, index);
                var itemLabel = new Label
                {
                    Text = item.Name.ToUpperInvariant(),
                    Style = (Style)control.Resources["TabText"]
                };
                var activeTabIndicator = CreateActiveTabIndicator(control, index);

                itemLayout.Children.Add(itemLabel);
                itemLayout.Children.Add(activeTabIndicator);
                control.scrollLayOut.Children.Add(itemLayout);
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
    }
}