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

    protected IConstants _constants;
    protected int _width;
    protected int _height;

    protected ScreenMode ScreenMode => _width > _height ? ScreenMode.Landscape : ScreenMode.Portrait;

    public string GetSvg(int screenWidth, int screenHeight)
    {
        _constants = constantsFactory.GetConstants(ScreenMode);
        _width = screenWidth - _constants.ScreenMargin * 2;
        _height = screenHeight - _constants.ScreenMargin * 2;
        _svg = new StringBuilder();
        Draw();
        return $"<svg width='{_width}' height='{_height}'>{_svg}</svg>";
    }

    protected void Svg(string svgCode) => _svg.Append(svgCode);

    protected abstract void Draw();
}