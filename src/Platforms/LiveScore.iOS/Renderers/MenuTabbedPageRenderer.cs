using System;
using CoreGraphics;
using LiveScore.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(MenuTabbedPageRenderer))]

namespace LiveScore.iOS.Renderers
{
    public class MenuTabbedPageRenderer : TabbedRenderer
    {
        private readonly UIOffset titlePositionOffset = new UIOffset(0, -4);
        private readonly UIEdgeInsets iconInsets = new UIEdgeInsets(-2, 0, 2, 0);

        private readonly UITextAttributes uiTextAttributes
            = new UITextAttributes
            {
                Font = FontManager.GetFont(nfloat.Parse(App.Current.Resources["FunctionBarFontSize"].ToString()))
            };

        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();

            foreach (var vc in ViewControllers)
            {
                vc.TabBarItem.TitlePositionAdjustment = titlePositionOffset;
                vc.TabBarItem.ImageInsets = iconInsets;
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            AddShadow();
            SetSelectedTabColor();
            UpdateAllTabBarItems();

            base.ViewWillAppear(animated);
        }

        private void AddShadow()
        {
            var control = ViewController as UITabBarController;

            if (control == null)
            {
                return;
            }

            var layer = control.TabBar.Layer;
            layer.ShadowColor = UIColor.Black.CGColor;
            layer.ShadowOffset = new CGSize(0.0, 1);
            layer.ShadowRadius = 8;
            layer.ShadowOpacity = (float)0.9;
        }

        private static void SetSelectedTabColor()
        {
            var selectedTabColor = GetColorFromResource("FunctionBarActiveColor");

            UITabBar.Appearance.SelectedImageTintColor = selectedTabColor;

            UITabBarItem.Appearance.SetTitleTextAttributes(
                new UITextAttributes
                {
                    TextColor = selectedTabColor
                },
                UIControlState.Selected);
        }

        private void UpdateAllTabBarItems()
        {
            foreach (var controller in ViewControllers)
            {
                controller.TabBarItem.SetTitleTextAttributes(uiTextAttributes, UIControlState.Normal);
            }
        }

        private static UIColor GetColorFromResource(string resourceName)
        {
            var color = (Color)App.Current.Resources[resourceName];

            if (color == default)
            {
                return UIColor.Black;
            }

            return UIColor.FromRGB((nfloat)color.R, (nfloat)color.G, (nfloat)color.B);
        }
    }
}