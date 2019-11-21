namespace LiveScore.Core.Controls.TabStrip
{
    using Xamarin.Forms;

    public class TabItemTemplateSelector : DataTemplateSelector
    {
        private static readonly DataTemplate EmptyTemplate = new DataTemplate();

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var tabItem = item as TabItemViewModel;

            return tabItem?.Template ?? EmptyTemplate;
        }
    }
}