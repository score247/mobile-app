using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}