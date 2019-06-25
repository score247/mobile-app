namespace LiveScore.Common.Controls.TabStrip
{
    using System;
    using Prism.Mvvm;
    using Xamarin.Forms;

    public class TabModel
    {
        public string Name { get; set; }

        public Type Template { get; set; }

        public View ContentTemplate { get; set; }

        public BindableBase ViewModel { get; set; }
    }
}