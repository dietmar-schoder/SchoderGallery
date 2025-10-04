using SchoderGallery.Navigation;
using SchoderGallery.Painters;
using SchoderGallery.Settings;

namespace SchoderGallery.Builders;

public class GroundFloorBuilder(
    SettingsFactory settingsFactory,
    SvgPainter svgPainter,
    NavigationService navigation)
    : BaseFloorBuilder(settingsFactory, svgPainter, navigation), IBuilder
{
    public override BuilderType Type => BuilderType.GroundFloor;
    public int Interval => 0;

    protected override void Draw()
    {
        base.Draw();

        var wall = _settings.WallThickness;
        int doorWidth = 3 * _windowWidth + 2 * _gap;
        var x = (SvgWidth - (3 * _windowWidth + 2 * _gap)) / 2;
        var xMiddle = x + doorWidth / 2;

        ClickableAreas.Add(new ClickableArea(x, SvgHeight - wall - _gap * 2, doorWidth, wall + _gap * 2, "/"));

        _svg.TextLink(xMiddle, SvgHeight - wall - _gap, "EXIT", _gap / 2, _settings);
    }
}