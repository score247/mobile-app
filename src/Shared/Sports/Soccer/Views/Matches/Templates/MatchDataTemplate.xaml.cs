using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Soccer.Views.Matches.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MatchDataTemplate : DataTemplate
    {
        public MatchDataTemplate() : base(typeof(MatchDataTemplate))
        {
            InitializeComponent();
        }
    }
}