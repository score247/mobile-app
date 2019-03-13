using System;
using LiveScoreApp.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ViewCell), typeof(ScoreCalendarCellRenderer))]
namespace LiveScoreApp.iOS.Renderers
{
    public class ScoreCalendarCellRenderer : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);

            cell.SelectedBackgroundView = new UIView
            {
                BackgroundColor = UIColor.Clear,
            };

            return cell;
        }
    }
}
