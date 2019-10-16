using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Core.Controls.TabStrip
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabStripHeader : ContentView
    {
        public TabStripHeader()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            nameof(ItemsSource),
            typeof(IEnumerable<TabItemViewModel>),
            typeof(TabStripHeader),
            propertyChanged: OnItemsSourceChanged);

        public IEnumerable<TabItemViewModel> ItemsSource
        {
            get => (IEnumerable<TabItemViewModel>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly BindableProperty ItemTappedCommandProperty
            = BindableProperty.Create(
                nameof(ItemTappedCommand),
                typeof(ICommand),
                typeof(TabItemViewModel));

        public ICommand ItemTappedCommand
        {
            get => GetValue(ItemTappedCommandProperty) as ICommand;
            set => SetValue(ItemTappedCommandProperty, value);
        }

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (TabStripHeader)bindable;

            if (control == null || newValue == null)
            {
                return;
            }

            var tabs = (IList<TabItemViewModel>)newValue;

            if (control.scrollLayOut.Children.Count == 0)
            {
                InitTabHeader(control, tabs);
            }
        }

        private static void InitTabHeader(TabStripHeader control, IList<TabItemViewModel> tabs)
        {
            for (byte index = 0; index < tabs.Count; index++)
            {
                var item = tabs[index];
                var itemLayout = CreateItemLayout(control, index);

                var itemLabel = new Label
                {
                    Text = item.TabHeaderName,
                    Style = index == 0 ? (Style)control.Resources["TabActiveText"] : (Style)control.Resources["TabText"]
                };

                var activeTabIndicator = CreateTabIndicator(control, index);

                itemLayout.Children.Add(itemLabel);
                itemLayout.Children.Add(activeTabIndicator);
                control.scrollLayOut.Children.Add(itemLayout);
            }
        }

        private static StackLayout CreateItemLayout(TabStripHeader control, byte index)
        {
            var itemLayout = new StackLayout
            {
                Style = (Style)control.Resources["Tab"]
            };

            var tapGestureRecognizer = new TapGestureRecognizer
            {
                Command = new Command(() => control.ItemTappedCommand?.Execute(index))
            };

            itemLayout.GestureRecognizers.Add(tapGestureRecognizer);

            return itemLayout;
        }

        private static ContentView CreateTabIndicator(TabStripHeader control, int index) => new ContentView
        {
            Content = new BoxView
            {
                Style = index == 0
                ? (Style)control.Resources["TabActiveLine"]
                : (Style)control.Resources["TabInactiveLine"]
            }
        };

        public void SetSelectedTab(int oldIndex, int newIndex)
        {
            var tabHeaders = scrollLayOut.Children;

            if (tabHeaders[oldIndex] is StackLayout oldTab)
            {
                oldTab.Children[0].Style = (Style)Resources["TabText"];
                ((ContentView)oldTab.Children[1]).Content.Style = (Style)Resources["TabInactiveLine"];
            }

            if (tabHeaders[newIndex] is StackLayout newTab)
            {
                newTab.Children[0].Style = (Style)Resources["TabActiveText"];
                ((ContentView)newTab.Children[1]).Content.Style = (Style)Resources["TabActiveLine"];
            }

            scrollView.ScrollToAsync(tabHeaders[newIndex], ScrollToPosition.Center, true);
        }
    }
}