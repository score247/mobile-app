﻿namespace LiveScore.Common.LangResources
{
    using System;
    using System.Reflection;
    using System.Resources;
    using Plugin.Multilingual;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        private const string ResourceId = "LiveScore.Common.LangResources.AppResources";

        private static readonly Lazy<ResourceManager> ResourceManager
            = new Lazy<ResourceManager>(() => new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly));

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
            {
                return string.Empty;
            }

            var cultureInfo = CrossMultilingual.Current.CurrentCultureInfo;
            var translation = ResourceManager.Value.GetString(Text, cultureInfo);

            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(
                    $"Key '{Text}' was not found in resources '{ResourceId}' for culture '{cultureInfo.Name}'.", Text);
#else
                translation = Text; // returns the key, which GETS DISPLAYED TO THE USER
#endif
            }

            return translation;
        }
    }
}