using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Core.Views.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MatchesTemplate : ContentView
    {
        public MatchesTemplate()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty LoadMoreCommandProperty
           = BindableProperty.Create(
                nameof(LoadMoreCommand),
                typeof(ICommand),
                typeof(MatchesTemplate),
                propertyChanged: (bindable, _, newValue) =>
                {
                    var matchesTemplate = bindable as MatchesTemplate;

                    if (newValue != null && matchesTemplate?.MatchesListView != null)
                    {
                        matchesTemplate.MatchesListView.LoadMoreCommand = newValue as ICommand;
                    }
                });

        public ICommand LoadMoreCommand
        {
            get => (ICommand)GetValue(LoadMoreCommandProperty);
            set => SetValue(LoadMoreCommandProperty, value);
        }

        public static readonly BindableProperty TriggerLoadMoreIndexProperty
            = BindableProperty.Create(
                nameof(TriggerLoadMoreIndex),
                typeof(int),
                typeof(MatchesTemplate),
                propertyChanged: (bindable, _, newValue) =>
                {
                    if (newValue != null && bindable is MatchesTemplate matchesTemplate && matchesTemplate.MatchesListView != null)
                    {
                        matchesTemplate.MatchesListView.TriggerLoadMoreIndex = (int)newValue;
                    }
                });

        public int TriggerLoadMoreIndex
        {
            get => (int)GetValue(TriggerLoadMoreIndexProperty);
            set => SetValue(TriggerLoadMoreIndexProperty, value);
        }

        public static readonly BindableProperty ListViewFooterTemplateProperty
           = BindableProperty.Create(
               nameof(ListViewFooterTemplate),
               typeof(DataTemplate),
               typeof(MatchesTemplate),
               propertyChanged: (bindable, _, newValue) =>
               {
                   if (newValue != null && bindable is MatchesTemplate matchesTemplate && matchesTemplate.MatchesListView != null)
                   {
                       matchesTemplate.MatchesListView.FooterTemplate = (DataTemplate)newValue;
                   }
               });

        public DataTemplate ListViewFooterTemplate
        {
            get => (DataTemplate)GetValue(ListViewFooterTemplateProperty);
            set => SetValue(ListViewFooterTemplateProperty, value);
        }
    }
}