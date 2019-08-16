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

        public string Name { get; }

        public string View { get; }

        public string Icon { get; }
    }
}