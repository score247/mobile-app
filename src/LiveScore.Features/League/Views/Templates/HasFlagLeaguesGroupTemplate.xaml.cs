using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Features.League.Views.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HasFlagLeaguesGroupTemplate : DataTemplate
    {
        public HasFlagLeaguesGroupTemplate() : base(typeof(HasFlagLeaguesGroupTemplate))
        {
            InitializeComponent();
        }
    }
}