using SchoderGallery.Navigation;
using SchoderGallery.Painters;
using SchoderGallery.Services;
using SchoderGallery.Settings;

namespace SchoderGallery.Builders;

public class LiftBuilder(
    SettingsFactory settingsFactory,
    SvgPainter svgPainter,
    NavigationService navigation,
    IGalleryService galleryService)
    : BaseBuilder(settingsFactory, svgPainter, navigation, galleryService), IBuilder
{
    public override FloorType FloorType => FloorType.Lift;
    public int Interval => 0;
    public FloorInfo CurrentFloor { get; set; }

    protected override async Task DrawAsync()
    {
        var floors = _navigation.GetFloors();

        int buttonSize = _largeFontSize * 2;
        int buttonGap = (int)(buttonSize * 0.75);

        int totalWidth = 2 * buttonSize + buttonGap;
        int totalHeight = 6 * buttonSize + 5 * buttonGap;

        int startX = _width50 - totalWidth / 2;
        int startY = _height50 - totalHeight / 2;

        CurrentFloor = await _navigation.GetVisitorFloorAsync();

        foreach (var floor in floors)
        {
            int x = startX + floor.LiftColumn * (buttonSize + buttonGap);
            int y = startY + floor.LiftRow * (buttonSize + buttonGap);
            int radius = buttonSize / 2;
            bool isGroundFloor = floor.FloorType == FloorType.GroundFloor;
            var exhibition = await _galleryService.GetExhibitionAsync(floor.FloorNumber);
            var label = exhibition?.Title ?? floor.LiftLabel;
            var colour = isGroundFloor ? Colours.WarmAccentRed : exhibition?.Colour ?? Colours.DarkGray;

            _svgPainter.Circle(x - 4, y - 6, buttonSize + 8, Colours.Black, 2);
            _svgPainter.Circle(x, y - 2, buttonSize, Colours.DarkGray, isGroundFloor ? 2 : 1, Colours.LighterGray);

            _svgPainter.Text(x + radius + 1, y + radius + 1, ((int)floor.FloorType).ToString(), _largeFontSize, Colours.White, 0);
            _svgPainter.Text(x + radius, y + radius, ((int)floor.FloorType).ToString(), _largeFontSize, Colours.Black, 0);

            if (floor.LiftColumn == 0)
            {
                ClickableAreas.Add(new ClickableArea(0, y - 6, _width50 - 2, buttonSize + 8, floor.FloorNumber.ToString()));
            }
            else
            {
                ClickableAreas.Add(new ClickableArea(_width50 + 2, y - 6, _width50 - 2, buttonSize + 8, floor.FloorNumber.ToString()));
            }

            DrawLiftLabel(x, y, label, colour, isLeftSide: floor.LiftColumn == 0);
        }

        void DrawLiftLabel(int x, int y, string label, string colour, bool isLeftSide = false)
        {
            var textAnchor = isLeftSide ? "end" : "start";
            var xPos = isLeftSide
                ? x - buttonGap / 2
                : x + buttonSize + buttonGap / 2;

            Svg($@"<text x='{xPos}' y='{y + buttonSize / 2}' 
            text-anchor='{textAnchor}' dominant-baseline='middle' fill='{colour}' 
            font-size='{(IsMobile ? _fontSize : _largeFontSize)}' font-family='sans-serif'>
            {label}</text>");
        }
    }
}