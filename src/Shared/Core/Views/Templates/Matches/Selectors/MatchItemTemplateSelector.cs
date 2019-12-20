﻿namespace LiveScore.Core.Views.Selectors
{
    using ViewModels;
    using Xamarin.Forms;

    public class MatchItemTemplateSelector : DataTemplateSelector
    {
        private DataTemplate matchItemTemplate;

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (matchItemTemplate != null || !(container.BindingContext is ViewModelBase viewModel))
            {
                return matchItemTemplate;
            }

            var sportType = viewModel.CurrentSportId.ToString();

            matchItemTemplate = viewModel.DependencyResolver.Resolve<DataTemplate>(sportType);

            return matchItemTemplate;
        }
    }
}