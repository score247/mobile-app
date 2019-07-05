using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Soccer.Views.Templates.DetailTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrackerTemplate : DataTemplate
    {
        public TrackerTemplate()
        {
            InitializeComponent();
        }
    }
}