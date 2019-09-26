namespace LiveScore.iOS.Renderers
{
    using System;
    using UIKit;

    public static class FontManager
    {
        public static UIFont GetFont(nfloat fontSize) => GetFont(StandardFontName, fontSize);

        public static UIFont GetFont(string fontFamily, nfloat fontSize)
        {
            return UIFont.FromName(fontFamily, fontSize);
        }

        public static string StandardFontName => App.Current.Resources["RobotoRegular"].ToString();
    }
}