namespace LiveScore.Core.Enumerations
{
    public class Languages : Enumeration
    {
        public static readonly Languages English = new Languages(1, "en-US");
        public static readonly Languages Vietnamese = new Languages(2, "vi-VN");
        public static readonly Languages Thailand = new Languages(3, "th-TH");
        public static readonly Languages Indonesia = new Languages(4, "id-ID");
        public static readonly Languages TraditionalChinese = new Languages(5, "zh-TW");
        public static readonly Languages SimplifiedChinese = new Languages(6, "zh-CN");

        public Languages()
        {
        }

        private Languages(byte value, string displayName)
            : base(value, displayName)
        {
        }
    }
}