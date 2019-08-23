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

        public int SelectedTabIndex { get; private set; }

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (TabStrip)bindable;
            var tabItems = newValue as IEnumerable<TabItemViewModelBase>;

            if (control == null || tabItems == null || !tabItems.Any())
            {
                MessagingCenter.Unsubscribe<string, int>(nameof(TabStrip), TabChangeEvent);
                return;
            }

            control.TabContent.ItemAppearing += TabContent_ItemAppearing;
            control.TabContent.ItemDisappearing += TabContent_ItemDisappearing;

            tabItems.ToArray()[control.SelectedTabIndex]?.OnAppearing();

            MessagingCenter.Subscribe<string, int>(
                nameof(TabStrip), TabChangeEvent, (_, index) => control.SelectedTabIndex = index);
        }

        public static void TabContent_ItemAppearing(CardsView view, ItemAppearingEventArgs args)
        {
            if (args.Item != null)
            {
                MessagingCenter.Send(nameof(TabStrip), TabChangeEvent, args.Index);

                (args.Item as TabItemViewModelBase)?.OnAppearing();
            }
        }

        public static void TabContent_ItemDisappearing(CardsView view, ItemDisappearingEventArgs args)
        {
            if (args.Item != null)
            {
                (args.Item as TabItemViewModelBase)?.OnDisappearing();
            }
        }
    }
}