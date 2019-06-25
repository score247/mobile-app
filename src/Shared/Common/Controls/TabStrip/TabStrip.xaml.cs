namespace LiveScore.Common.Controls.TabStrip
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabStrip : ContentView
    {
        private static int currentTabIndex;

        public TabStrip()
        {
            InitializeComponent();
            TabHeader.BindingContext = this;
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            "ItemsSource",
            typeof(IEnumerable<TabModel>),
            typeof(TabStrip),
            propertyChanged: OnItemsSourceChanged);

        public IEnumerable<TabModel> ItemsSource
        {
            get { return (IEnumerable<TabModel>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (TabStrip)bindable;

            if (control != null)
            {
                var tabs = (IEnumerable<TabModel>)newValue;

                if (tabs != null)
                {
                    control.TabContent.Content = tabs.First().ContentTemplate;

                    MessagingCenter.Subscribe<string, string>("Tab", "TabChange", (_, index) =>
                    {
                        var tab = tabs.ToArray()[int.Parse(index)];
                        currentTabIndex = int.Parse(index);
                        control.TabContent.Content = tab.ContentTemplate;
                    });
                }
            }
        }

        private void OnSwiped(object sender, SwipedEventArgs e)
        {
            var index = 0;
            switch (e.Direction)
            {
                case SwipeDirection.Left:
                    index = currentTabIndex + 1;

                    if (index <= ItemsSource.Count())
                    {
                        MessagingCenter.Send("Tab", "TabChange", index.ToString());
                    }
                    break;

                case SwipeDirection.Right:
                    index = currentTabIndex - 1;

                    if (index >= 0)
                    {
                        MessagingCenter.Send("Tab", "TabChange", index.ToString());
                    }
                    break;
            }
        }
    }
}