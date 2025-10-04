using SchoderGallery.Navigation;
using SchoderGallery.Painters;
using SchoderGallery.Settings;

namespace SchoderGallery.Builders;

public class Floor3Builder(
    SettingsFactory settingsFactory,
    SvgPainter svgPainter,
    NavigationService navigation)
    : BaseFloorBuilder(settingsFactory, svgPainter, navigation), IBuilder
{
    public override BuilderType Type => BuilderType.Floor3;
    public int Interval => 0;
}