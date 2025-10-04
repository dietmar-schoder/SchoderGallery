using SchoderGallery.Navigation;
using SchoderGallery.Painters;
using SchoderGallery.Settings;

namespace SchoderGallery.Builders;

public class Floor1Builder(
    SettingsFactory settingsFactory,
    SvgPainter svgPainter,
    NavigationService navigation)
    : BaseFloorBuilder(settingsFactory, svgPainter, navigation), IBuilder
{
    public override BuilderType Type => BuilderType.Floor1;
    public int Interval => 0;

    protected override void Draw()
    {
        base.Draw();
    }
}