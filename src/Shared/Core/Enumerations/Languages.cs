namespace LiveScore.Core.Enumerations
{
    public class Languages : Enumeration
    {
        public static readonly Languages English = new Languages("En", nameof(English));
        public static readonly Languages Vietnamese = new Languages("Vi", nameof(Vietnamese));
        public static readonly Languages Thailand = new Languages("Th", nameof(Thailand));
        public static readonly Languages Indonesia = new Languages("Id", nameof(Indonesia));
        public static readonly Languages TraditionalChinese = new Languages("Zht", "Traditional Chinese");
        public static readonly Languages SimplifiedChinese = new Languages("Zh", "Simplified Chinese");
        
        public Languages()
        {
        }

        private Languages(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}