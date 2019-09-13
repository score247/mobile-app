namespace LiveScore.iOS.Renderers
{
    using System;
    using UIKit;

    public static class FontManager
    {
        /// <summary>
        /// Gets a <see cref="UIFont"/> object for the standard font with the desired size.
        /// </summary>
        /// <returns>UIFont object for use.</returns>
        /// <param name="fontSize">Font size.</param>
        public static UIFont GetFont(nfloat fontSize) => GetFont(StandardFontName, fontSize);

        public static UIFont GetFont(string fontFamily, nfloat fontSize)
        {
            return UIFont.FromName(fontFamily, fontSize);
        }

        /// <summary>
        /// Gets the iOS font name for the standard font for this application.
        /// </summary>
        /// <value>The name of the standard font.</value>
        public static string StandardFontName => new UIKit.UITextField().Font.Name;
    }
}