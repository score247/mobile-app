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

        protected override async Task LoadData(Func<Task> loadDataFunc, bool showBusy = true)
        {
            await base.LoadData(loadDataFunc, showBusy && IsFirstLoad);

            IsFirstLoad = false;
        }
    }
}