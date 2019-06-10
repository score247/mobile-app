namespace LiveScore.Soccer.Views.Templates.MatchDetailInfo
{
    using System;
    using LiveScore.Soccer.ViewModels;
    using Xamarin.Forms;
    using LiveScore.Core.Enumerations;

    public class MatchDetailInfoItemTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var itemViewModel = (MatchDetailInfoItemViewModel)item;
            var timelineEvent = itemViewModel.TimelineEvent;
            var viewModel = container.BindingContext as MatchDetailInfoItemViewModel;

            switch (timelineEvent.Type)
            {
                case EventTypes.ScoreChange:
                    container.BindingContext = new ScoreChangeItemViewModel(
                        viewModel.TimelineEvent, viewModel.Result, viewModel.NavigationService, viewModel.DependencyResolver);

                    return new ScoreChangeItemTemplate();
            }

            return new ScoreChangeItemTemplate();
        }
    }
}