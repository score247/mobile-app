using System;
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
        private const float TabBarHeight = 50f;

        private readonly UITextAttributes uiTextAttributes
            = new UITextAttributes
            {
                Font = FontManager.GetFont(nfloat.Parse(App.Current.Resources["FunctionBarFontSize"].ToString()))
            };

        public override void ViewWillAppear(bool animated)
        {
            SetSelectedTabColor();
            UpdateAllTabBarItems();
            base.ViewWillAppear(animated);
        }

        private static void SetSelectedTabColor()
        {
            var color = App.Current.Resources["FunctionBarActiveColor"];

            if (color == null)
            {
                return;
            }

            var selectedColor = (Color)color;
            var selectedTabColor = UIColor.FromRGB((nfloat)selectedColor.R, (nfloat)selectedColor.G,
                (nfloat)selectedColor.B);

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

        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();

            var tabFrame = TabBar.Frame;
            tabFrame.Height = TabBarHeight;
            tabFrame.Y = View.Frame.Height - TabBarHeight;

            TabBar.Frame = tabFrame;

            foreach (var vc in ViewControllers)
            {
                vc.TabBarItem.TitlePositionAdjustment = titlePositionOffset;
                vc.TabBarItem.ImageInsets = iconInsets;
            }
        }
    }
}