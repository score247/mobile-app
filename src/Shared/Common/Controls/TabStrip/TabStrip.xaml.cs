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
        public TabStrip()
        {
            InitializeComponent();
            TabHeader.BindingContext = this;
        }

        public TabStripViewModel ViewModel { get; set; }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            "ItemsSource",
            typeof(IEnumerable),
            typeof(TabStrip),
            propertyChanged: OnItemsSourceChanged);

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
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
                        control.TabContent.Content = tab.ContentTemplate;
                        control.TabContent.Content.BindingContext = tab.ViewModel;
                    });
                }
            }
        }
    }
}