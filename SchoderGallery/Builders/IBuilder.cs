using SchoderGallery.Navigation;

namespace SchoderGallery.Builders;

public interface IBuilder
{
    BuilderType Type { get; }
    int Interval { get; }
    Task<string> GetSvgContentAsync(int screenWidth, int screenHeight);
    int SvgWidth { get; set; }
    int SvgHeight { get; set; }
    List<ClickableArea> ClickableAreas { get; }
}
