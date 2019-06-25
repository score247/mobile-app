using System;
using System.Collections.Generic;
using System.Text;
using LiveScore.Core;
using LiveScore.Core.ViewModels;
using Prism.Events;
using Prism.Navigation;

namespace LiveScore.Soccer.ViewModels.DetailOdds
{
    internal class DetailOddsViewModel : ViewModelBase
    {
        public DetailOddsViewModel(
            INavigationService navigationService, IDependencyResolver serviceLocator)
            : base(navigationService, serviceLocator)
        {
        }
    }
}