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
        var smallerWindowSize = Math.Min(_windowWidth, _windowHeight);
        var factor = Math.Min(smallerWindowSize / 5, 15);
        var offset = factor * 10 / 2;
        var thickness = 1.0 / factor;

        ClickableAreas.Add(new ClickableArea(0, 0, _width33 - 2, _height50 - 2, "/Cafe", "Café", FloorType: FloorType.Cafe));
        foreach (var (off, colour) in ShadowList)
        {
            _svgPainter.IconCafe(_width25 - offset + off, _height33 - offset + off, factor, colour, thickness);
        }

        ClickableAreas.Add(new ClickableArea(_width66 + 2, 0, _width33 - 2, _height50 - 2, "/Shop", "Shop", FloorType: FloorType.Shop));
        foreach (var (off, colour) in ShadowList)
        {
            _svgPainter.IconShop(_width75 - offset + off, _height33 - offset + off, factor, colour, thickness);
        }

        ClickableAreas.Add(new ClickableArea(0, _height50 + 2, _width33 - 2, _height50 - 2, "/Toilets", "Toilets", FloorType: FloorType.Toilets));
        foreach (var (off, colour) in ShadowList)
        {
            _svgPainter.IconToilets(_width25 - offset + off, _height66 - offset + off, factor, colour, thickness);
        }

        ClickableAreas.Add(new ClickableArea(_width66 + 2, _height50 + 2, _width33 - 2, _height50 - 2, "/Info", "Information", FloorType: FloorType.Info));
        foreach (var (off, colour) in ShadowList)
        {
            _svgPainter.IconInfo(_width75 - offset + off, _height66 - offset + off, factor, colour, thickness);
        }

        ClickableAreas.Add(new ClickableArea(_width33 + 2, _height75 + 2, _width33 - 4, _height25 - 2, "/", "Good buy"));
        _svgPainter.TextLink(_width50, SvgHeight - wall - _largeFontSize * 2, "EXIT", _fontSize * 3 / 2);
    }
}
