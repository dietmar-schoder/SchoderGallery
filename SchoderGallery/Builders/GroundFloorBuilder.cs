using SchoderGallery.Navigation;
using SchoderGallery.Painters;
using SchoderGallery.Services;
using SchoderGallery.Settings;

namespace SchoderGallery.Builders;

public class GroundFloorBuilder(
    SettingsFactory settingsFactory,
    SvgPainter svgPainter,
    NavigationService navigation,
    GalleryService galleryService)
    : BaseFloorBuilder(settingsFactory, svgPainter, navigation, galleryService), IBuilder
{
    public override FloorType FloorType => FloorType.GroundFloor;

    protected override async Task DrawAsync()
    {
        await base.DrawAsync();

        var wall = _settings.WallThickness;

        ClickableAreas.Add(new ClickableArea(0, 0, _width33 - 2, _height50 - 2, "/Cafe", "Café"));
        _svgPainter.TextLink(_width25, _height33, "CAFE", _fontSize * 3 / 2);

        ClickableAreas.Add(new ClickableArea(_width66 + 2, 0, _width33 - 2, _height50 - 2, "/Shop", "Shop"));
        _svgPainter.TextLink(_width75, _height33, "SHOP", _fontSize * 3 / 2);

        ClickableAreas.Add(new ClickableArea(0, _height50 + 2, _width33 - 2, _height50 - 2, "/Info", "Information"));
        _svgPainter.TextLink(_width25, _height66, "INFO", _fontSize * 3 / 2);

        ClickableAreas.Add(new ClickableArea(_width66 + 2, _height50 + 2, _width33 - 2, _height50 - 2, "/Toilets", "Toilets"));
        _svgPainter.TextLink(_width75, _height66, "TOILETS", _fontSize * 3 / 2);

        ClickableAreas.Add(new ClickableArea(_width33 + 2, _height75 + 2, _width33 - 4, _height25 - 2, "/", "Good buy"));
        _svgPainter.TextLink(_width50, SvgHeight - wall - _largeFontSize * 2, "EXIT", _fontSize * 3 / 2);
    }
}
