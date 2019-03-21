using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Logging;
using Prism.Services;

namespace Common.ViewModels
{
    public class MatchViewViewModel : ViewModelBase
    {
        public MatchViewViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}
