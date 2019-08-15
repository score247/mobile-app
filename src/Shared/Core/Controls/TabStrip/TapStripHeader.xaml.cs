namespace LiveScore.Core.Controls.TabStrip
{
    using System.Collections.Generic;
    using System.Linq;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabStripHeader : ContentView
    {
        private const int TabLineIndex = 2;

        public TabStripHeader()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
              nameof(ItemsSource), typeof(IEnumerable<TabItemViewModelBase>), typeof(TabStripHeader), propertyChanged: OnItemsSourceChanged);

        public IEnumerable<TabItemViewModelBase> ItemsSource
        {
            get { return (IEnumerable<TabItemViewModelBase>)GetValue(ItemsSourceProperty); }
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
                var tabHeaders = control.scrollLayOut.Children;

                for (int i = 0; i < tabHeaders.Count; i++)
                {
                    var tabHeader = (StackLayout)tabHeaders[i];
                    var tabModel = control.ItemsSource.ToList()[i];

                    (tabHeader.Children[0] as Image).Source = i == index
                        ? tabModel.TabHeaderActiveIcon?.Value
                        : tabModel.TabHeaderIcon?.Value;

                    tabHeader.Children[1].Style = i == index
                        ? (Style)control.Resources["TabActiveText"]
                        : (Style)control.Resources["TabText"];

                    ((ContentView)tabHeader.Children[TabLineIndex]).Content.Style = i == index
                        ? (Style)control.Resources["TabActiveLine"]
                        : (Style)control.Resources["TabInactiveLine"];
                }

                control.scrollView.ScrollToAsync(tabHeaders[index], ScrollToPosition.Center, true);
            });
        }

        private static void InitTabHeader(TabStripHeader control, IList<TabItemViewModelBase> tabs)
        {
            for (int index = 0; index < tabs.Count; index++)
            {
                var item = tabs[index];
                var itemLayout = CreateItemLayout(control, index);

                var itemIcon = new Image
                {
                    Source = index == 0 ? item.TabHeaderActiveIcon?.Value : item.TabHeaderIcon?.Value,
                    Style = (Style)control.Resources["TabIcon"],
                    WidthRequest = 16,
                    HeightRequest = 16
                };

                var itemLabel = new Label
                {
                    Text = item.TabHeaderTitle.ToUpperInvariant(),
                    Style = index == 0 ? (Style)control.Resources["TabActiveText"] : (Style)control.Resources["TabText"]
                };
                var activeTabIndicator = CreateTabIndicator(control, index);

                itemLayout.Children.Add(itemIcon);
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