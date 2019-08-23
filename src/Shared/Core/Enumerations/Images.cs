namespace LiveScore.Core.Enumerations
{
    public class Images : TextEnumeration
    {
        public static readonly Images TabIcon = new Images("images/common/tab_icon.png", nameof(TabIcon));

        public Images()
        {
        }

        protected Images(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}