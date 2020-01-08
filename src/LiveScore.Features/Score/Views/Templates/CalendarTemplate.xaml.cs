using LiveScore.Core.Controls.MatchesListView;
using LiveScore.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Features.Score.Views.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarTemplate : DataTemplate
    {
        public CalendarTemplate()
        {
            InitializeComponent();
        }

#pragma warning disable S2325 // Methods and properties that don't access instance data should be static

        public void MatchesListView_Scrolled(object sender, ScrolledEventArgs e)
        {
            if (sender is MatchesListView listView && listView.BindingContext is MatchesViewModel viewModel)
            {
                viewModel.IsHeaderVisible = e.ScrollY <= 0;
            }
        }

#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
    }
}