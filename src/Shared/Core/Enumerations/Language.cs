namespace LiveScore.Core.Enumerations
{
    public class Language : Enumeration
    {
        public static readonly Language English = new Language(1, "en-US");
        public static readonly Language Vietnamese = new Language(2, "vi-VN");
        public static readonly Language Thailand = new Language(3, "th-TH");
        public static readonly Language Indonesia = new Language(4, "id-ID");
        public static readonly Language TraditionalChinese = new Language(5, "zh-TW");
        public static readonly Language SimplifiedChinese = new Language(6, "zh-CN");

        public Language()
        {
        }

        private Language(byte value, string displayName)
            : base(value, displayName)
        {
        }
    }
}