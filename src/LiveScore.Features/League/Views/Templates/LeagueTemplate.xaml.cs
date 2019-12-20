using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Features.League.Views.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeagueTemplate : DataTemplate
    {
        public LeagueTemplate() : base(typeof(LeagueTemplate))
        {
            InitializeComponent();
        }
    }
}