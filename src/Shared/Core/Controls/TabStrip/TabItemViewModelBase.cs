namespace LiveScore.Core.Controls.TabStrip
{
    using System;
    using System.Threading.Tasks;
    using LiveScore.Core.Enumerations;
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

        public bool IsFirstLoad { get; protected set; } = true;

        public DataTemplate Template { get; }

        public string TabHeaderTitle { get; set; }

        public Images TabHeaderIcon { get; set; }

        public Images TabHeaderActiveIcon { get; set; }

        protected override async Task LoadData(Func<Task> loadDataFunc, bool showLoading = true)
        {
            IsLoading = showLoading && IsFirstLoad;

            await loadDataFunc.Invoke();

            IsLoading = false;
            IsFirstLoad = false;
        }
    }
}