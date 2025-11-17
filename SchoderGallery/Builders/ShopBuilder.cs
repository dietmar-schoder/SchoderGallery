using SchoderGallery.Navigation;
using SchoderGallery.Painters;
using SchoderGallery.Services;
using SchoderGallery.Settings;

namespace SchoderGallery.Builders;

public class ShopBuilder(
    SettingsFactory settingsFactory,
    SvgPainter svgPainter,
    NavigationService navigation,
    GalleryService galleryService)
    : BaseFloorBuilder(settingsFactory, svgPainter, navigation, galleryService), IBuilder
{
    public override FloorType FloorType => FloorType.Shop;
}
