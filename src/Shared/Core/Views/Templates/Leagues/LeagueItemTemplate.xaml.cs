using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Core.Views.Templates.Leagues
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeagueItemTemplate : DataTemplate
    {
        public LeagueItemTemplate() : base(typeof(LeagueItemTemplate))
        {
            InitializeComponent();
        }
    }
}