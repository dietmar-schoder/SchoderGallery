using SchoderGallery.Constants;
using System.Text;

namespace SchoderGallery.Builders;

public interface IFacadeBuilder
{
    string GetSvg(int width, int height);
}

public abstract class BaseBuilder(IConstantsFactory constantsFactory) : IFacadeBuilder
{
    private StringBuilder _svg;

    protected int _width;
    protected int _height;
    protected IConstants _constants;

    protected ScreenMode ScreenMode => _width > _height ? ScreenMode.Landscape : ScreenMode.Portrait;

    public string GetSvg(int width, int height)
    {
        _width = width;
        _height = height;
        _constants = constantsFactory.GetConstants(ScreenMode);
        _svg = new StringBuilder();
        Draw();
        return $"<svg width='{_width}' height='{_height}'>{_svg}</svg>";
    }

    protected void Svg(string svgCode) => _svg.Append(svgCode);

    protected abstract void Draw();
}