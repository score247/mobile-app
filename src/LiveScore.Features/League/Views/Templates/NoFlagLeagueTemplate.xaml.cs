using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Features.League.Views.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoFlagLeagueTemplate : DataTemplate
    {
        public NoFlagLeagueTemplate() : base(typeof(NoFlagLeagueTemplate))
        {
            InitializeComponent();
        }
    }
}