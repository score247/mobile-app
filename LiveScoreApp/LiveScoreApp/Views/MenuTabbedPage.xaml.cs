using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScoreApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuTabbedPage : TabbedPage
    {
        public MenuTabbedPage()
        {
            InitializeComponent();
        }
    }
}