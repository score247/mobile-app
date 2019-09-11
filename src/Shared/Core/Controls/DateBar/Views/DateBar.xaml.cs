namespace LiveScore.Core.Controls.DateBar.Views
{
    using System;
    using System.Windows.Input;
    using EventArgs;
    using MethodTimer;
    using Xamarin.Forms;

    public partial class DateBar : ContentView
    {
        private const string SelectedIndexChangedEvent = "SelectedIndexChanged";

        public DateBar()
        {
            InitializeComponent();
        }

        [Time]
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
            var liveIcon = new Label { Style = (Style)Resources["HomeIcon"] };
            liveBox.Children.Add(liveIcon);

            MessagingCenter.Subscribe<string, int>(nameof(DateBar), SelectedIndexChangedEvent, (_, selectedIndex) =>
            {
                if (selectedIndex == 0)
                {
                    SetSelectedTextColor(liveIcon);
                }
                else
                {
                    SetTextColor(liveIcon);
                }
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
            var dayNumberLbl = new Label
            {
                Text = date.Day.ToString(),
                Style = (Style)Resources["DateBarDayNumberLabel"],
            };

            var dayNameLbl = new Label
            {
                Text = date.Date.DayOfWeek.ToString().Substring(0, 3).ToUpperInvariant(),
                Style = (Style)Resources["DateBarDayNameLabel"]
            };

            if (index == SelectedIndex)
            {
                SetSelectedTextColor(dayNumberLbl);
                SetSelectedTextColor(dayNameLbl);
            }

            dateBarItemLayout.Children.Add(dayNumberLbl);
            dateBarItemLayout.Children.Add(dayNameLbl);

            SubsribeSelectedIndexChanged(index, dayNumberLbl, dayNameLbl);

            dateBarItemLayout.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = BuildTapDateBarItemCommand(date, index, dayNumberLbl, dayNameLbl)
            });

            return dateBarItemLayout;
        }

        private void SubsribeSelectedIndexChanged(int index, Label dayNumberLbl, Label dayNameLbl)
        {
            MessagingCenter.Subscribe<string, int>(nameof(DateBar), SelectedIndexChangedEvent, (_, selectedIndex) =>
            {
                if (index == selectedIndex)
                {
                    SetSelectedTextColor(dayNumberLbl);
                    SetSelectedTextColor(dayNameLbl);
                }
                else
                {
                    SetTextColor(dayNumberLbl);
                    SetTextColor(dayNameLbl);
                }
            });
        }

        private Command BuildTapDateBarItemCommand(DateTime date, int index, Label dayNumberLbl, Label dayNameLbl)
        {
            return new Command(() =>
            {
                var args = new DateBarItemTappedEventArgs(index, date);
                ItemTappedCommand?.Execute(args);

                SelectedIndex = index;
            });
        }

        private void SetTextColor(Label item)
        {
            item.TextColor = (Color)Resources["DateBarTextColor"];
        }

        private void SetSelectedTextColor(Label item)
        {
            item.TextColor = (Color)Resources["DateBarSelectedTextColor"];
        }
    }
}