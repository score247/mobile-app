﻿namespace LiveScore.Soccer.Views.Templates
{
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MatchDataTemplate : DataTemplate
    {
        public MatchDataTemplate() : base(typeof(MatchDataTemplate))
        {
            InitializeComponent();
        }
    }
}