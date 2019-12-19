using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Core.Views.Templates.Leagues
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegionGroupTemplate : DataTemplate
    {
        public RegionGroupTemplate() : base(typeof(RegionGroupTemplate))
        {
            InitializeComponent();
        }
    }
}