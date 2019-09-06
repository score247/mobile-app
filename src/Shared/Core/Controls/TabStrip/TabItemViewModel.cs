namespace LiveScore.Core.Controls.TabStrip
{
    using System;
    using System.Threading.Tasks;
    using Enumerations;
    using ViewModels;
    using Prism.Events;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class TabItemViewModel : ViewModelBase
    {
        public TabItemViewModel()
        {
        }

        public TabItemViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            DataTemplate dataTemplate,
            IEventAggregator eventAggregator = null)
            : base(navigationService, dependencyResolver, eventAggregator)
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

            if (loadDataFunc != null)
            {
                await loadDataFunc.Invoke().ConfigureAwait(false);
            }

            IsLoading = false;
            IsFirstLoad = false;
        }
    }
}