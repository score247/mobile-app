namespace LiveScore.Core.Controls.TabStrip
{
    using System;
    using System.Threading.Tasks;
    using Prism.Events;
    using Prism.Navigation;
    using ViewModels;
    using Xamarin.Forms;

    public class TabItemViewModel : ViewModelBase
    {
        protected TabItemViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            DataTemplate dataTemplate,
            IEventAggregator eventAggregator,
            string tabHeaderName)
            : base(navigationService, dependencyResolver, eventAggregator)
        {
            Template = dataTemplate;
            TabHeaderName = tabHeaderName;
        }

        public bool IsFirstLoad { get; protected set; } = true;

        public DataTemplate Template { get; }

        public string TabHeaderName { get; }

        protected override async Task LoadDataAsync(Func<Task> loadDataFunc, bool showBusy = true)
        {
            await base.LoadDataAsync(loadDataFunc, showBusy && IsFirstLoad);

            IsFirstLoad = false;
        }
    }
}