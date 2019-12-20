using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Core.Views.Templates.Leagues
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoFlagLeagueItemTemplate : DataTemplate
    {
        public NoFlagLeagueItemTemplate() : base(typeof(NoFlagLeagueItemTemplate))
        {
            InitializeComponent();
        }
    }
}