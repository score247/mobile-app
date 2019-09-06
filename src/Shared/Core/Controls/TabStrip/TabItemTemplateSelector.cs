namespace LiveScore.Core.Controls.TabStrip
{
    using Xamarin.Forms;

    internal class TabItemTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var tabItem = item as TabItemViewModel;

            return tabItem?.Template;
        }
    }
}