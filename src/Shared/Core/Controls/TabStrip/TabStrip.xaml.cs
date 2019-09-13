namespace LiveScore.Core.Controls.TabStrip
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows.Input;
    using LiveScore.Core.Controls.TabStrip.EventArgs;
    using MethodTimer;
    using PanCardView;
    using PanCardView.EventArgs;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabStrip : ContentView
    {
        public TabStrip()
        {
            var currentInstance = this;

            InitializeComponent();
            TabHeader.BindingContext = currentInstance;
            TabContent.BindingContext = currentInstance;

            TabHeader.ItemTappedCommand = new Command((index) =>
            {
                Debug.WriteLine($"TabHeader tapped {index}");
                SelectedIndex = (int)index;
            });

            TabContent.ItemDisappearing += TabContent_ItemDisappearing;
            TabContent.ItemAppeared += TabContent_ItemAppeared;
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

        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(
              nameof(SelectedIndex),
              typeof(int),
              typeof(TabItemViewModel),
              0,
              propertyChanged: (bindable, oldValue, newValue) =>
              {

                  var newIndex = (int)newValue;
                  var oldIndex = (int)oldValue;

                  var control = bindable as TabStrip;

                  if (control == null || newIndex < 0 || oldIndex < 0)
                  {
                      return;
                  }

                  control.TabHeader.SetSelectedTab(oldIndex, newIndex);

                  control.ItemTappedCommand?.Execute(new TabStripItemTappedEventArgs(newIndex));
              });


        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        public static readonly BindableProperty ItemTappedCommandProperty = BindableProperty.Create(nameof(ItemTappedCommand), typeof(ICommand), typeof(TabItemViewModel), null);

        public ICommand ItemTappedCommand
        {
            get => GetValue(ItemTappedCommandProperty) as ICommand;
            set => SetValue(ItemTappedCommandProperty, value);
        }

        [Time]
        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (TabStrip)bindable;
            var tabItems = newValue as IEnumerable<TabItemViewModel>;

            if (control == null || tabItems == null || !tabItems.Any())
            {
                return;
            }
        }

        [Time]
        public void TabContent_ItemAppeared(CardsView view, ItemAppearedEventArgs args)
        {
            if (args.Item != null)
            {
                (args.Item as TabItemViewModel)?.OnAppearing();
            }
        }

        [Time]
        public void TabContent_ItemDisappearing(CardsView view, ItemDisappearingEventArgs args)
        {
            if (args.Item != null)
            {
                (args.Item as TabItemViewModel)?.OnDisappearing();
            }
        }
    }
}