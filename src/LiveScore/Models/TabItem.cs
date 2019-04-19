namespace LiveScore.Models
{
    public class TabItem
    {
        public TabItem(string name, string view, string icon)
        {
            Name = name;
            View = view;
            Icon = icon;
        }

        public string Name { get; set; }

        public string View { get; set; }

        public string Icon { get; set; }
    }
}