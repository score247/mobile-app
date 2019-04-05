namespace LiveScore.Common.Controls.TabStrip
{
    using Xamarin.Forms;

    public class TabViewSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var tab = (TabModel)item;

            return new DataTemplate(() =>
            {
                return new ContentView
                {
                    Content = tab.Template
                };
            });
        }
    }
}
