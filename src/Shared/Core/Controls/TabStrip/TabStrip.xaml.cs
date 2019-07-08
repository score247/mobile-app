namespace LiveScore.Core.Controls.TabStrip
{
    using System.Collections.Generic;
    using System.Linq;
    using PanCardView;
    using PanCardView.EventArgs;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabStrip : ContentView
    {
        private const string TabChangeEvent = "TabChange";

        public TabStrip()
        {
            var currentInstance = this;

            InitializeComponent();
            TabHeader.BindingContext = currentInstance;
            TabContent.BindingContext = currentInstance;
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            nameof(ItemsSource),
            typeof(IEnumerable<TabItemViewModelBase>),
            typeof(TabStrip),
            propertyChanged: OnItemsSourceChanged);

        public IEnumerable<TabItemViewModelBase> ItemsSource
        {
            get { return (IEnumerable<TabItemViewModelBase>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public int SelectedTabIndex { get; set; }

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (TabStrip)bindable;

            if (control == null || newValue == null)
            {
                MessagingCenter.Unsubscribe<string, int>(nameof(TabStrip), TabChangeEvent);
                return;
            }

            control.TabContent.ItemBeforeAppearing += TabContent_ItemBeforeAppearing;
            control.TabContent.ItemAppearing += TabContent_ItemAppearing;
            control.TabContent.ItemDisappearing += TabContent_ItemDisappearing;

            var tabItems = newValue as IEnumerable<TabItemViewModelBase>;
            tabItems.ToArray()[control.SelectedTabIndex]?.OnAppearing();

            MessagingCenter.Subscribe<string, int>(nameof(TabStrip), "TabChange", (_, index) =>
            {
                control.SelectedTabIndex = index;
            });
        }

        public static void TabContent_ItemBeforeAppearing(CardsView view, ItemBeforeAppearingEventArgs args)
        {
            MessagingCenter.Send(nameof(TabStrip), TabChangeEvent, args.Index);
        }

        public static void TabContent_ItemAppearing(CardsView view, ItemAppearingEventArgs args)
        {
            (args.Item as TabItemViewModelBase)?.OnAppearing();
        }

        public static void TabContent_ItemDisappearing(CardsView view, ItemDisappearingEventArgs args)
        {
            (args.Item as TabItemViewModelBase)?.OnDisappearing();
        }
    }
}