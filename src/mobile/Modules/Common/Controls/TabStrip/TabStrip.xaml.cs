namespace Common.Controls.TabStrip
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabStrip : ContentView
    {
        public TabStripViewModel ViewModel { get; set; }

        public TabStrip()
        {
            InitializeComponent();
            ViewModel = new TabStripViewModel();
            TabContent.BindingContext = ViewModel;
            TabHeader.BindingContext = ViewModel;
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            "ItemsSource",
            typeof(IEnumerable),
            typeof(TabStrip),
            null,
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
                control.ViewModel.Tabs = new ObservableCollection<TabModel>(tabs);
            }
        }
    }
}
