using SchoderGallery.Navigation;
using SchoderGallery.Painters;
using SchoderGallery.Settings;

namespace SchoderGallery.Builders;

public class LiftBuilder(
    SettingsFactory settingsFactory,
    SvgPainter svgPainter,
    NavigationService navigation)
    : BaseBuilder(settingsFactory, svgPainter, navigation), IBuilder
{
    private const double LiftButtonCaptionSizeFactor = 1;
    private const double LiftLabelSizeFactor = 1;

    public override BuilderType Type => BuilderType.Lift;
    public int Interval => 0;

    protected override void Draw()
    {
        var floors = _navigation.GetFloors();

        int buttonSize = ShortWindowSize;

        int totalWidth = 2 * buttonSize + _gap;
        int totalHeight = 6 * buttonSize + 5 * _gap;

        int startX = (SvgWidth - totalWidth) / 2;
        int startY = (SvgHeight - totalHeight) / 2;

        foreach (var floor in floors)
        {
            int x = startX + floor.LiftColumn * (buttonSize + _gap);
            int y = startY + floor.LiftRow * (buttonSize + _gap);
            int radius = buttonSize / 2;
            bool isGroundFloor = floor.FloorType == BuilderType.GroundFloor;
            bool isCurrentFloor = floor.FloorType == _navigation.GetVisitorFloor();
            var colour = isCurrentFloor ? _settings.LightGray: (isGroundFloor ? _settings.Black : _settings.DarkGray);

            _svg.Circle(x - 4, y - 6, buttonSize + 8, _settings.Black, 2);
            _svg.Circle(x, y - 2, buttonSize, colour, isGroundFloor ? 2 : 1);

            _svg.Text(x + radius, y + radius, ((int)floor.FloorType).ToString(), (int)(_gap * LiftButtonCaptionSizeFactor), colour, 0);

            if (floor.LiftColumn == 0)
            {
                ClickableAreas.Add(new ClickableArea(0, y - 6, SvgWidth / 2 - 2, buttonSize + 8, PageAndParam(floor)));

                Svg($@"<text x='{x - _gap}' y='{y + buttonSize / 2}' 
                        text-anchor='end' dominant-baseline='middle' fill='{colour}' 
                        font-size='{_gap * LiftLabelSizeFactor}' font-family='sans-serif'>
                        {floor.LiftLabel}</text>");
            }
            else
            {
                ClickableAreas.Add(new ClickableArea(SvgWidth / 2, y - 6, SvgWidth / 2 + 2, buttonSize + 8, PageAndParam(floor)));

                Svg($@"<text x='{x + buttonSize + _gap}' y='{y + buttonSize / 2}' 
                        text-anchor='start' dominant-baseline='middle' fill='{colour}' 
                        font-size='{_gap * LiftLabelSizeFactor}' font-family='sans-serif'>
                        {floor.LiftLabel}</text>");
            }
        }

        static string PageAndParam(FloorInfo floorInfo) =>
            ((int)floorInfo.FloorType > 0 && (int)floorInfo.FloorType < 7)
            || ((int)floorInfo.FloorType < 0 && (int)floorInfo.FloorType > -3)
            ? $"{floorInfo.Page}/{(int)floorInfo.FloorType}"
            : floorInfo.Page;
    }
}