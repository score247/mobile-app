﻿using System;
using LiveScore.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(MenuTabbedPageRenderer))]

namespace LiveScore.iOS.Renderers
{
    public class MenuTabbedPageRenderer : TabbedRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            SetSelectedTabColor();

            base.ViewWillAppear(animated);
        }

        private void SetSelectedTabColor()
        {
            var color = App.Current.Resources["SecondAccentColor"];

            if (color != null)
            {
                var selectedColor = (Color)color;
                UIColor selectedTabColor = UIColor.FromRGB((nfloat)selectedColor.R, (nfloat)selectedColor.G, (nfloat)selectedColor.B);

                UITabBar.Appearance.SelectedImageTintColor = selectedTabColor;

                UITabBarItem.Appearance.SetTitleTextAttributes(
                new UITextAttributes
                {
                    TextColor = selectedTabColor
                },
                    UIControlState.Selected);
            }
        }
    }
}