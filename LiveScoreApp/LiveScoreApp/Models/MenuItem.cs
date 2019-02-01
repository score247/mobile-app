namespace LiveScoreApp.Models
{
    public enum MenuItemType
    {
        Soccer,
        Hockey,
        Basketball
    }

    public class MenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}