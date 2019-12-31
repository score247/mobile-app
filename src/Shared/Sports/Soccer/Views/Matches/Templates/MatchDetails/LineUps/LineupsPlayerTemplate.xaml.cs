using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Soccer.Views.Templates.MatchDetails.LineUps
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LineupsPlayerTemplate : DataTemplate
    {
        public LineupsPlayerTemplate() : base(typeof(LineupsPlayerTemplate))
        {
            InitializeComponent();
        }
    }
}