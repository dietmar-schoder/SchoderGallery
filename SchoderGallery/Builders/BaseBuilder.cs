using SchoderGallery.Settings;
using System.Text;

namespace SchoderGallery.Builders;

public abstract class BaseBuilder(SettingsFactory settingsFactory)
{
    protected StringBuilder _svg;
    protected Random _random = new();
    protected ISettings _settings;

    protected int _rowsColumns;
    protected int _gap;
    protected int _margin;
    protected int _windowWidth;
    protected int _windowHeight;
    protected int _windowGlassColumns;
    protected int _windowGlassRows;
    protected double _windowGlassColumnWidth;
    protected double _windowGlassRowHeight;

    protected ScreenMode ScreenMode => SvgWidth > SvgHeight ? ScreenMode.Landscape : ScreenMode.Portrait;
    protected int ShortSize => ScreenMode == ScreenMode.Portrait ? SvgWidth : SvgHeight;
    protected int ShortWindowSize => ScreenMode == ScreenMode.Portrait ? _windowWidth : _windowHeight;

    public int SvgWidth { get; set; }
    public int SvgHeight { get; set; }
    public List<ClickableArea> ClickableAreas { get; } = [];

    public string GetSvgContent(int screenWidth, int screenHeight)
    {
        _settings = settingsFactory.GetSettings(screenWidth, screenHeight);
        SvgWidth = Math.Max(240, screenWidth - _settings.ScreenMargin * 2);
        SvgHeight = Math.Max(240, screenHeight - _settings.ScreenMargin * 2);
        _svg = new StringBuilder();
        _rowsColumns = _settings.RowsColumns;

        _gap = (int)(ShortSize / _rowsColumns * _settings.GapToRowColumnWidthRatio);
        _margin = (int)(_gap * _settings.WindowMarginToGapRatio);
        int totalGapSpace = _gap * (_rowsColumns - 1) + 2 * _margin;
        _windowWidth = WindowWidth(totalGapSpace);
        _windowHeight = WindowHeight(totalGapSpace);

        _windowGlassColumns = _settings.NbrOfHorizontalWindowSections;
        _windowGlassRows = _settings.NbrOfVerticalWindowSections;
        _windowGlassColumnWidth = (_windowWidth - 1.0) / _windowGlassColumns;
        _windowGlassRowHeight = (_windowHeight - 1.0) / _windowGlassRows;
        Draw();
        return _svg.ToString();
    }

    protected void Svg(string svgCode) => _svg.Append(svgCode);

    protected abstract void Draw();

    protected string SkyColour()
    {
        int gray = _random.Next(245, 256);
        int blue = Math.Min(255, gray + _random.Next(0, 256 - gray));
        return $"#{gray:X2}{gray:X2}{blue:X2}";
    }

    protected void Border(double x, double y, int width, int height, string colour, int thickness = 1) =>
        Svg($"<rect x='{x + 0.5}' y='{y + 0.5}'" +
            $" width='{width - 1}' height='{height - 1}'" +
            $" fill='none' stroke='{colour}' stroke-width='{thickness}' />");

    protected void Area(double x, double y, double width, double height, string colour) =>
        Svg($"<rect x='{x}' y='{y}'" +
            $" width='{width}' height='{height}'" +
            $" fill='{colour}' stroke='none' />");

    protected void Area(double x, double y, int width, int height, string colour, string borderColour, int thickness = 1) =>
        Svg($"<rect x='{x + 0.5}' y='{y + 0.5}'" +
            $" width='{width - 1}' height='{height - 1}'" +
            $" fill='{colour}' stroke='{borderColour}' stroke-width='{thickness}' />");

    protected void VerticalLine(double x, double y, int length, string colour, int thickness = 1) =>
        Svg($"<line x1='{x}' y1='{y}' x2='{x}' y2='{y + length}' stroke='{colour}' stroke-width='{thickness}' />");

    protected void HorizontalLine(double x, double y, int length, string colour, int thickness = 1) =>
        Svg($"<line x1='{x}' y1='{y}' x2='{x + length}' y2='{y}' stroke='{colour}' stroke-width='{thickness}' />");

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