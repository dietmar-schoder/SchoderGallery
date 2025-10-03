using SchoderGallery.Settings;
using System.Text;

namespace SchoderGallery.Builders;

public interface IBuilder
{
    string GetSvgContent(int screenWidth, int screenHeight);
    int SvgWidth { get; set; }
    int SvgHeight { get; set; }
    List<ClickableArea> ClickableAreas { get; }
}

public abstract class BaseBuilder(ISettingsFactory constantsFactory) : IBuilder
{
    private StringBuilder _svg;

    protected Random _rand = new();
    protected ISettings _constants;

    protected int _rowsColumns;
    protected int _gap;
    protected int _margin;
    protected int _windowWidth;
    protected int _windowHeight;
    protected int _windowGlassColumns;
    protected int _windowGlassRows;
    protected int _windowGlassColumnWidth;
    protected int _windowGlassRowHeight;

    protected ScreenMode ScreenMode => SvgWidth > SvgHeight ? ScreenMode.Landscape : ScreenMode.Portrait;
    protected int ShortSize => ScreenMode == ScreenMode.Portrait ? SvgWidth : SvgHeight;
    protected int ShortWindowSize => ScreenMode == ScreenMode.Portrait ? _windowWidth : _windowHeight;

    public int SvgWidth { get; set; }
    public int SvgHeight { get; set; }
    public List<ClickableArea> ClickableAreas { get; } = [];

    public string GetSvgContent(int screenWidth, int screenHeight)
    {
        _constants = constantsFactory.GetConstants(screenWidth, screenHeight);
        SvgWidth = Math.Max(240, screenWidth - _constants.ScreenMargin * 2);
        SvgHeight = Math.Max(240, screenHeight - _constants.ScreenMargin * 2);
        _svg = new StringBuilder();
        _rowsColumns = _constants.RowsColumns;

        _gap = (int)(ShortSize / _rowsColumns * _constants.GapToRowColumnWidthRatio);
        _margin = (int)(_gap * _constants.WindowMarginToGapRatio);
        int totalGapSpace = _gap * (_rowsColumns - 1) + 2 * _margin;
        _windowWidth = WindowWidth(totalGapSpace);
        _windowHeight = WindowHeight(totalGapSpace);

        _windowGlassColumns = _constants.NbrOfHorizontalWindowSections;
        _windowGlassRows = _constants.NbrOfVerticalWindowSections;
        _windowGlassColumnWidth = (int)Math.Round((double)(_windowWidth - 1) / _windowGlassColumns);
        _windowGlassRowHeight = (int)Math.Round((double)(_windowHeight - 1) / _windowGlassRows);
        Draw();
        return _svg.ToString();
    }

    protected void Svg(string svgCode) => _svg.Append(svgCode);

    protected abstract void Draw();

    protected string SkyColour()
    {
        int gray = _rand.Next(245, 256);
        int blue = Math.Min(255, gray + _rand.Next(0, 256 - gray));
        return $"#{gray:X2}{gray:X2}{blue:X2}";
    }

    private int WindowWidth(int totalGapSpace)
    {
        int horizontalSpaceForWindows = SvgWidth - totalGapSpace;
        int windowWidth = horizontalSpaceForWindows / _rowsColumns;
        SvgWidth -= horizontalSpaceForWindows - (windowWidth * _rowsColumns);
        return windowWidth;
    }

    private int WindowHeight(int totalGapSpace)
    {
        int verticalSpaceForWindows = SvgHeight - totalGapSpace; ;
        int windowHeight = verticalSpaceForWindows / _rowsColumns;
        SvgHeight -= verticalSpaceForWindows - (windowHeight * _rowsColumns);
        return windowHeight;
    }
}