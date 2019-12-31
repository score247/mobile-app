using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Soccer.Views.Templates.MatchDetails.LineUps
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubstitutionTemplate : DataTemplate
    {
        public SubstitutionTemplate() : base(typeof(SubstitutionTemplate))
        {
            InitializeComponent();
        }
    }
}