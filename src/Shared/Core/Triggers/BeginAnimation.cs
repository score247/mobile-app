using System;
using System.Collections.Generic;
using System.Text;
using LiveScore.Core.Animations.Base;
using Xamarin.Forms;

namespace LiveScore.Core.Triggers
{
    public class BeginAnimation : TriggerAction<VisualElement>
    {
        public AnimationBase Animation { get; set; }

        protected override async void Invoke(VisualElement sender)
        {
            if (Animation != null)
            {
                await Animation.Begin();
            }
        }
    }
}