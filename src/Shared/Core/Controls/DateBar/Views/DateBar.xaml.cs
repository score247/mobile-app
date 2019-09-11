namespace LiveScore.Core.Controls.DateBar.Views
{
    using ViewModels;
    using Xamarin.Forms;
    using System;
    using System.Windows.Input;
    using EventArgs;

    public partial class DateBar : ContentView
    {
        private const string SelectedIndexChangedEvent = "SelectedIndexChanged";

        public DateBar()
        {
            InitializeComponent();
        }

        public DateBarViewModel ViewModel { get; set; }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            BuildDateBar();
        }

        private void BuildDateBar()
        {
            DateBarLayout.Children.Clear();

            AddLiveBox();

            AddDateBoxes();

            AddCalendarBox();
        }

        public static readonly BindableProperty NumberDisplayDaysProperty
          = BindableProperty.Create(
              nameof(NumberDisplayDays),
              typeof(int),
              typeof(DateBar));

        public int NumberDisplayDays
        {
            get => (int)GetValue(NumberDisplayDaysProperty);
            set => SetValue(NumberDisplayDaysProperty, value);
        }

        public static readonly BindableProperty SelectedIndexProperty
            = BindableProperty.Create(
                nameof(SelectedIndexProperty),
                typeof(int),
                typeof(DateBar),
                propertyChanged: OnSelectedIndexChanged);

        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        public static readonly BindableProperty ItemTappedCommandProperty
            = BindableProperty.Create(nameof(ItemTappedCommand), typeof(ICommand), typeof(DateBar), null);

        public ICommand ItemTappedCommand
        {
            get => GetValue(ItemTappedCommandProperty) as ICommand;
            set => SetValue(ItemTappedCommandProperty, value);
        }

        private static void OnSelectedIndexChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != null)
            {
                MessagingCenter.Send(nameof(DateBar), SelectedIndexChangedEvent, (int)newValue);
            }
        }

        private void AddLiveBox()
        {
            var liveBox = new StackLayout { Style = (Style)Resources["DateBarBox"] };
            liveBox.Children.Add(new Label
            {
                Style = (Style)Resources["HomeIcon"]
            });

            DateBarLayout.Children.Add(liveBox);
        }

        private void AddDateBoxes()
        {
            var currentIndex = 1;

            for (int i = -NumberDisplayDays; i <= NumberDisplayDays; i++)
            {
                var dateBarItem = BuildDateBarItem(DateTime.Today.AddDays(i), currentIndex);

                DateBarLayout.Children.Add(dateBarItem);
                currentIndex++;
            }
        }

        private void AddCalendarBox()
        {
            var calendarBox = new StackLayout { Style = (Style)Resources["DateBarBox"] };
            calendarBox.Children.Add(new Label
            {
                Style = (Style)Resources["CalendarIcon"]
            });

            DateBarLayout.Children.Add(calendarBox);
        }

        private StackLayout BuildDateBarItem(DateTime date, int index)
        {
            var dateBarItemLayout = new StackLayout { Style = (Style)Resources["DateBarBox"] };
            var currentDayNumber = new Label
            {
                Text = date.Day.ToString(),
                Style = (Style)Resources["DateBarDayNumberLabel"],
            };

            var currentDayName = new Label
            {
                Text = date.Date.DayOfWeek.ToString().Substring(0, 3).ToUpperInvariant(),
                Style = (Style)Resources["DateBarDayNameLabel"]
            };

            dateBarItemLayout.Children.Add(currentDayNumber);
            dateBarItemLayout.Children.Add(currentDayName);

            MessagingCenter.Subscribe<string, int>(nameof(DateBar), SelectedIndexChangedEvent, (_, selectedIndex) =>
            {
                if (index == SelectedIndex)
                {
                    currentDayNumber.TextColor = (Color)Resources["DateBarSelectedTextColor"];
                    currentDayName.TextColor = (Color)Resources["DateBarSelectedTextColor"];
                }
                else
                {
                    currentDayNumber.TextColor = (Color)Resources["DateBarTextColor"];
                    currentDayName.TextColor = (Color)Resources["DateBarTextColor"];
                }
            });

            dateBarItemLayout.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    var args = new DateBarItemTappedEventArgs(index, date);
                    ItemTappedCommand?.Execute(args);

                    currentDayNumber.TextColor = (Color)Resources["DateBarSelectedTextColor"];
                    currentDayName.TextColor = (Color)Resources["DateBarSelectedTextColor"];
                    SelectedIndex = index;
                })
            });

            return dateBarItemLayout;
        }
    }
}