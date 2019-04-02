namespace Common.Controls.TabStrip
{
    using System;
    using Xamarin.Forms;

    public class TabModel
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public Type TemplateType { get; set; }

        public View Template { get; set; }

        public object ViewModel { get; set; }
    }
}
