namespace LiveScore.Core.Controls.TabStrip
{
    using System.Collections.Generic;
    using System.Linq;
    using LiveScore.Core.ViewModels;
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
            typeof(IEnumerable<TabModel>),
            typeof(TabStrip),
            propertyChanged: OnItemsSourceChanged);

        public IEnumerable<TabModel> ItemsSource
        {
            get { return (IEnumerable<TabModel>)GetValue(ItemsSourceProperty); }
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

            MessagingCenter.Subscribe<string, int>(nameof(TabStrip), "TabChange", (_, index) =>
            {
                control.SelectedTabIndex = index;
            });
        }

        private static void TabContent_ItemBeforeAppearing(CardsView view, ItemBeforeAppearingEventArgs args)
        {
            MessagingCenter.Send(nameof(TabStrip), TabChangeEvent, args.Index);
        }

        private static void TabContent_ItemAppearing(CardsView view, ItemAppearingEventArgs args)
        {
            var currentView = view.CurrentView;
            var viewModel = (args.Item as TabModel)?.ViewModel;
            viewModel?.OnAppearing();
            currentView.BindingContext = viewModel;
        }

        private static void TabContent_ItemDisappearing(CardsView view, ItemDisappearingEventArgs args)
        {
            var tab = args.Item as TabModel;
            tab.ViewModel?.OnDisappearing();
        }
    }
}