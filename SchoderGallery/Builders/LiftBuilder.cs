using SchoderGallery.Navigation;
using SchoderGallery.Painters;
using SchoderGallery.Settings;

namespace SchoderGallery.Builders;

public class LiftBuilder(
    SettingsFactory settingsFactory,
    SvgPainter svgPainter,
    NavigationService navigation)
    : BaseBuilder(settingsFactory, svgPainter), IBuilder
{
    public BuilderType Type => BuilderType.Lift;
    public int Interval => 0;

    protected override void Draw()
    {
        var floors = navigation.Floors.Values;

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

            _svg.Circle(x - 4, y - 6, buttonSize + 8, _settings.DarkGray, 2);
            _svg.Circle(x, y - 2, buttonSize, _settings.DarkGray, 1);

            _svg.Text(x + radius, y + radius, floor.LiftButtonCaption.ToString(), (int)(_gap * 0.8), _settings.Black, 0);

            if (floor.LiftColumn == 0)
            {
                ClickableAreas.Add(new ClickableArea(0, y, SvgWidth / 2, buttonSize, floor.Page));

                Svg($@"<text x='{x - _gap}' y='{y + buttonSize / 2}' 
                        text-anchor='end' dominant-baseline='middle' 
                        font-size='{_gap * 0.6}' font-family='sans-serif'>
                        {floor.LiftLabel}</text>");
            }
            else
            {
                ClickableAreas.Add(new ClickableArea(SvgWidth / 2, y, SvgWidth / 2, buttonSize, floor.Page));

                Svg($@"<text x='{x + buttonSize + _gap}' y='{y + buttonSize / 2}' 
                        text-anchor='start' dominant-baseline='middle' 
                        font-size='{_gap * 0.6}' font-family='sans-serif'>
                        {floor.LiftLabel}</text>");
            }
        }
    }
}