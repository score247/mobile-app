namespace LiveScore.Core.Controls.TabStrip
{
    using System;
    using LiveScore.Core.ViewModels;
    using Xamarin.Forms;

    public class TabModel
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public View Template { get; set; }

        public ViewModelBase ViewModel { get; set; }
    }
}