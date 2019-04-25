namespace LiveScore.Core.Enumerations
{
    public class Languages : Enumeration
    {
        public static readonly Languages English = new Languages("en-US", nameof(English));
        public static readonly Languages Vietnamese = new Languages("vi-VN", nameof(Vietnamese));
        public static readonly Languages Thailand = new Languages("th-TH", nameof(Thailand));
        public static readonly Languages Indonesia = new Languages("id-ID", nameof(Indonesia));
        public static readonly Languages TraditionalChinese = new Languages("zh-TW", "Traditional Chinese");
        public static readonly Languages SimplifiedChinese = new Languages("zh-CN", "Simplified Chinese");

        public Languages()
        {
        }

        private Languages(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}