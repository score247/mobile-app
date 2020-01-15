using LiveScore.Core.Tests.Mocks;
using Xamarin.Forms;

namespace LiveScore.Core.Tests.Fixtures
{
    public class ResourcesFixture
    {
        public ResourcesFixture()
        {
            MockXamForms.Init();

            var app = new ApplicationMock
            {
                Resources =
                {
                    ["GeneralFontSize"] = 12,
                    ["SmallTextFontSize"] = 11,
                    ["MediumTextFontSize"] = 10,
                    ["UppercaseTextFontSize"] = 10,
                    ["SmalleastTextFontSize"] = 10,
                    ["TabFontSize"] = 10,
                    ["TableRowFontSize"] = 10,
                    ["FunctionGroupHeight"] = 10,
                    ["TabsHeight"] = 12,
                    ["SubTabsHeight"] = 12,
                    ["TabLayoutBgColor"] = Color.FromHex("#F24822"),
                    ["FunctionGroupColor"] = Color.FromHex("#F24822"),
                    ["PrimaryColor"] = Color.FromHex("#1D2133"),
                    ["PrimaryAccentColor"] = Color.FromHex("#F24822"),
                    ["FourthTextColor"] = Color.FromHex("#939393"),
                    ["FourthAccentColor"] = Color.FromHex("#1D2133"),
                    ["SecondColor"] = Color.FromHex("#1D2133"),
                    ["LineColor"] = Color.FromHex("#1D2133"),
                    ["HomeTeamColor"] = Color.FromHex("#1D2133"),
                    ["AwayTeamColor"] = Color.FromHex("#1D2133"),
                    ["PrimaryTextColor"] = Color.FromHex("#1D2133"),
                    ["SecondTextColor"] = Color.FromHex("#1D2133"),
                    ["SecondAccentColor"] = Color.FromHex("#1D2133"),
                    ["TabColor"] = Color.FromHex("#1D2133"),
                    ["ActiveTabColor"] = Color.FromHex("#1D2133"),
                    ["ActiveLineColor"] = Color.FromHex("#1D2133"),
                    ["InactiveLineColor"] = Color.FromHex("#1D2133"),
                    ["SubTabContainerBgColor"] = Color.FromHex("#1D2133"),
                    ["SubTabColor"] = Color.FromHex("#fff"),
                    ["SubTabBgColor"] = Color.FromHex("#0F111C"),
                    ["ActiveSubTabBgColor"] = Color.FromHex("#1A1C28"),
                    ["ActiveSubTabColor"] = Color.FromHex("#3EC28F"),
                    ["UpLiveOddColor"] = Color.FromHex("#FF222C"),
                    ["DownLiveOddColor"] = Color.FromHex("#66FF59"),
                    ["TableBgColor"] = Color.FromHex("#66FF59"),
                    ["FirstPositive"] = Color.FromHex("#66FF59"),
                    ["FirstPositiveOutcomeColor"] = Color.FromHex("#66FF59"),
                    ["Icons"] = new Style(typeof(Label)),
                    ["LiveIcon"] = new Style(typeof(Label)),
                    ["DropdownIcon"] = new Style(typeof(Label)),
                    ["WinnerInfoIcon"] = new Style(typeof(Image)),
                    ["TableText"] = new Style(typeof(Label)),
                    ["TableHeaderText"] = new Style(typeof(Label)),
                    ["RobotoMedium"] = new Style(typeof(Label)),
                    ["RobotoLight"] = new Style(typeof(Label)),
                    ["FlexLayoutHorizontalCenter"] = new Style(typeof(FlexLayout)),
                    ["FlexLayoutAlignLeft"] = new Style(typeof(FlexLayout)),
                    ["FlexLayoutAlignCenter"] = new Style(typeof(FlexLayout)),
                    ["DropdownListBox"] = new Style(typeof(AbsoluteLayout)),
                    ["TableContainer"] = new Style(typeof(StackLayout)),
                    ["GridTableContainer"] = new Style(typeof(Grid)),
                    ["CenterActivityIndicator"] = new Style(typeof(ActivityIndicator)),
                    ["DropdownList"] = new Style(typeof(Picker)),
                    ["HorizontalLine"] = new Style(typeof(BoxView)),
                    ["ListViewBgColor"] = Color.FromHex("#66FF59"),
                    ["FunctionGroupBgColor"] = Color.FromHex("#1D2133"),
                    ["HighlightCommentaryColor"] = Color.FromHex("#1D2133"),
                    ["CommentaryColor"] = Color.FromHex("#1D2133")
                }
            };
        }
    }
}