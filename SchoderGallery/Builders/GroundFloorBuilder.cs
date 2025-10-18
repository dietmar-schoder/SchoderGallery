using SchoderGallery.Navigation;
using SchoderGallery.Painters;
using SchoderGallery.Services;
using SchoderGallery.Settings;

namespace SchoderGallery.Builders;

public class GroundFloorBuilder(
    SettingsFactory settingsFactory,
    SvgPainter svgPainter,
    NavigationService navigation,
    IGalleryService galleryService)
    : BaseFloorBuilder(settingsFactory, svgPainter, navigation, galleryService), IBuilder
{
    public override BuilderType Type => BuilderType.GroundFloor;
    public int Interval => 0;

    protected override async Task DrawAsync()
    {
        await base.DrawAsync();

        var wall = _settings.WallThickness;
        int doorWidth = 3 * _windowWidth + 2 * _gap;
        var x = (SvgWidth - (3 * _windowWidth + 2 * _gap)) / 2;
        var xMiddle = x + doorWidth / 2;

        ClickableAreas.Add(new ClickableArea(x, SvgHeight - wall - _largeFontSize * 4, doorWidth, wall + _largeFontSize * 4, "/", "Good buy"));

        _svgPainter.TextLink(xMiddle, SvgHeight - wall - _largeFontSize * 2, "EXIT", _fontSize);
    }
}