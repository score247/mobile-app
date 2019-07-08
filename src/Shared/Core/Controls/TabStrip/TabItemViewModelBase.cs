namespace LiveScore.Core.Controls.TabStrip
{
    using LiveScore.Core.ViewModels;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class TabItemViewModelBase : ViewModelBase
    {
        public TabItemViewModelBase()
        {
        }

        public TabItemViewModelBase(INavigationService navigationService, IDependencyResolver depdendencyResolver, DataTemplate dataTemplate)
            : base(navigationService, depdendencyResolver)
        {
            Template = dataTemplate;
        }

        public DataTemplate Template { get; }

        public string HeaderTitle { get; set; }
    }
}