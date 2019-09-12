﻿namespace LiveScore.Core.Controls.TabStrip
{
    using System.Collections.Generic;
    using System.Linq;
    using MethodTimer;
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

            TabContent.ItemAppearing += TabContent_ItemAppearing;
            TabContent.ItemDisappearing += TabContent_ItemDisappearing;
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            nameof(ItemsSource),
            typeof(IEnumerable<TabItemViewModel>),
            typeof(TabStrip),
            propertyChanged: OnItemsSourceChanged);

        public IEnumerable<TabItemViewModel> ItemsSource
        {
            get => (IEnumerable<TabItemViewModel>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public int SelectedTabIndex { get; set; }

        [Time]
        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (TabStrip)bindable;
            var tabItems = newValue as IEnumerable<TabItemViewModel>;

            if (control == null || tabItems == null || !tabItems.Any())
            {
                MessagingCenter.Unsubscribe<string, int>(nameof(TabStrip), TabChangeEvent);
                return;
            }

            //TODO use SelectedIndex instead of First
            tabItems.First().OnAppearing();

            MessagingCenter.Subscribe<string, int>(
                nameof(TabStrip), TabChangeEvent, (_, index) => control.SelectedTabIndex = index);
        }

        [Time]
        public static void TabContent_ItemAppearing(CardsView view, ItemAppearingEventArgs args)
        {
            if (args.Item != null)
            {
                MessagingCenter.Send(nameof(TabStrip), TabChangeEvent, args.Index);

                (args.Item as TabItemViewModel)?.OnAppearing();
            }
        }

        public static void TabContent_ItemDisappearing(CardsView view, ItemDisappearingEventArgs args)
        {
            if (args.Item != null)
            {
                (args.Item as TabItemViewModel)?.OnDisappearing();
            }
        }
    }
}