using Score.Views.Templates;
using Xamarin.Forms;

namespace Score.Converters
{
    public class MatchDetailItemTemplateSelector : DataTemplateSelector
    {
        public MatchDetailItemTemplateSelector()
        {
        }


        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var itemName = item.ToString();

            switch (itemName)
            {
                case "MatchInfo":
                    return new DataTemplate(typeof(MatchInfoTemplate));

                case "MatchStats":
                    return new DataTemplate(typeof(MatchStatsTemplate));

                default:
                    return new DataTemplate(typeof(MatchInfoTemplate)); ;
            }
        }
    }
}
