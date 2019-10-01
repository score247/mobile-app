namespace LiveScore.Features.News.ViewModels
{
    using System.Threading.Tasks;
    using Core;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.ViewModels;
    using LiveScore.Core.Views;
    using Prism.Commands;
    using Prism.Navigation;
    using Rg.Plugins.Popup.Services;

    public class EmptyNewsViewModel : ViewModelBase
    {
        public EmptyNewsViewModel(
            INavigationService navigationService, 
            IDependencyResolver serviceLocator) 
            : base(navigationService, serviceLocator)
        {
            Title = "News";
        }
    }
}