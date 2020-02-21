using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Soccer.Views.Teams
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchTeamsView : ContentPage
    {
        public SearchTeamsView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            SearchBarControl.OnAppearing();
        }
    }
}