using SchoderGallery.Navigation;
using SchoderGallery.Painters;
using SchoderGallery.Services;
using SchoderGallery.Settings;

namespace SchoderGallery.Builders;

public class Floor6Builder(
    SettingsFactory settingsFactory,
    SvgPainter svgPainter,
    NavigationService navigation,
    IGalleryService galleryService)
    : BaseFloorBuilder(settingsFactory, svgPainter, navigation, galleryService), IBuilder
{
    public override FloorType FloorType => FloorType.Floor6;
    public int Interval => 0;
}