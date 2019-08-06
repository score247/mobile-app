﻿namespace LiveScore.Core.Views.Selectors
{
    using LiveScore.Core.ViewModels;
    using Xamarin.Forms;

    public class MatchItemTemplateSelector : DataTemplateSelector
    {
        private DataTemplate matchItemTemplate;

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (matchItemTemplate == null)
            {
                var viewModel = container.BindingContext as ViewModelBase;
                var sportType = viewModel.SettingsService.CurrentSportType;

                matchItemTemplate = viewModel.DependencyResolver.Resolve<DataTemplate>(sportType.Value.ToString());
            }

            return matchItemTemplate;
        }
    }
}