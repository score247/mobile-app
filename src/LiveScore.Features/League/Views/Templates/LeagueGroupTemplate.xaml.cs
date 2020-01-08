using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Features.League.Views.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeagueGroupTemplate : DataTemplate
    {
        public LeagueGroupTemplate() : base(typeof(LeagueGroupTemplate))
        {
            InitializeComponent();
        }
    }
}