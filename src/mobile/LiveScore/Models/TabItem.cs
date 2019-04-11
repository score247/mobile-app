namespace LiveScore.Models
{
    public class TabItem
    {
        public TabItem(string name, string view)
        {
            Name = name;
            View = view;
        }

        public string Name { get; set; }

        public string View { get; set; }
    }
}
