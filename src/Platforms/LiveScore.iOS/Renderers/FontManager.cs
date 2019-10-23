using System;
using UIKit;

namespace LiveScore.iOS.Renderers
{
    public static class FontManager
    {
        public static UIFont GetFont(nfloat fontSize)
            => GetFont(StandardFontName, fontSize);

        public static UIFont GetFont(string fontFamily, nfloat fontSize)
            => UIFont.FromName(fontFamily, fontSize);

        public static string StandardFontName
            => App.Current.Resources["RobotoRegular"].ToString();
    }
}