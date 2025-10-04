using SchoderGallery.Navigation;
using SchoderGallery.Painters;
using SchoderGallery.Settings;

namespace SchoderGallery.Builders;

public class Basement1Builder(
    SettingsFactory settingsFactory,
    SvgPainter svgPainter,
    NavigationService navigation)
    : BaseFloorBuilder(settingsFactory, svgPainter, navigation), IBuilder
{
    public override BuilderType Type => BuilderType.Basement1;
    public int Interval => 0;
}