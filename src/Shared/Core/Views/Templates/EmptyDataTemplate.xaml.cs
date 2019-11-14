using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Core.Views.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmptyDataTemplate : DataTemplate
    {
        public EmptyDataTemplate()
        {
            InitializeComponent();
        }
    }
}