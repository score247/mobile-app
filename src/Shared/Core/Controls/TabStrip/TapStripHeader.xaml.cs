namespace LiveScore.Core.Controls.TabStrip
{
    using System.Collections;
    using System.Collections.Generic;
    using LiveScore.Core.ViewModels;
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
                MessagingCenter.Unsubscribe<string, int>(nameof(TabStrip), "TabChange");
                return;
            }

            var tabs = (IList<TabItemViewModelBase>)newValue;

            if (control.scrollLayOut.Children.Count == 0)
            {
                InitTabHeader(control, tabs);
            }

            SubscribeTabChange(control);
        }

        private static void SubscribeTabChange(TabStripHeader control)
        {
            MessagingCenter.Subscribe<string, int>(nameof(TabStrip), "TabChange", (_, index) =>
            {
                var children = control.scrollLayOut.Children;

                for (int i = 0; i < children.Count; i++)
                {
                    var childLayout = (StackLayout)children[i];

                    childLayout.Children[0].Style = i == index
                    ? (Style)control.Resources["TabActiveText"]
                    : (Style)control.Resources["TabText"];

                    ((ContentView)childLayout.Children[1]).Content.Style = i == index
                    ? (Style)control.Resources["TabActiveLine"]
                    : (Style)control.Resources["TabInactiveLine"];
                }

                control.scrollView.ScrollToAsync(children[index], ScrollToPosition.Center, true);
            });
        }

        private static void InitTabHeader(TabStripHeader control, IList<TabItemViewModelBase> tabs)
        {
            for (int index = 0; index < tabs.Count; index++)
            {
                var item = tabs[index];
                var itemLayout = CreateItemLayout(control, index);
                var itemLabel = new Label
                {
                    Text = item.HeaderTitle.ToUpperInvariant(),
                    Style = index == 0 ? (Style)control.Resources["TabActiveText"] : (Style)control.Resources["TabText"]
                };
                var activeTabIndicator = CreateTabIndicator(control, index);

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
                MessagingCenter.Send("Tab", "TabChange", index);
            };

            itemLayout.GestureRecognizers.Add(tapGestureRecognizer);

            return itemLayout;
        }

        private static ContentView CreateTabIndicator(TabStripHeader control, int index)
        {
            return new ContentView
            {
                Content = new BoxView
                {
                    Style = index == 0 ? (Style)control.Resources["TabActiveLine"] : (Style)control.Resources["TabInactiveLine"]
                }
            };
        }
    }
}