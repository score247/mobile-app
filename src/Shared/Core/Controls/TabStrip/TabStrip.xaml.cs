using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using LiveScore.Core.Controls.TabStrip.EventArgs;
using MethodTimer;
using PanCardView;
using PanCardView.EventArgs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Core.Controls.TabStrip
{
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
            typeof(TabStrip));

        public IEnumerable<TabItemViewModel> ItemsSource
        {
            get => (IEnumerable<TabItemViewModel>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(
              nameof(SelectedIndex),
              typeof(byte),
              typeof(TabItemViewModel),
              0,
              propertyChanged: (bindable, oldValue, newValue) =>
              {
                  var newIndex = (byte)newValue;
                  var oldIndex = (byte)oldValue;


                  if (!(bindable is TabStrip control))
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