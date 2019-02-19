using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScoreApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigationTitleView : ContentView
    {
        public NavigationTitleView()
        {
            this.InitComponent(InitializeComponent);
            titleLabel.BindingContext = this;
        }

        public static readonly BindableProperty TitleProperty
            = BindableProperty.Create("Title", typeof(string), typeof(NavigationTitleView), string.Empty);

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
    }
}