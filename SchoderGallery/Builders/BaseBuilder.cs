using SchoderGallery.Navigation;
using SchoderGallery.Painters;
using SchoderGallery.Settings;

namespace SchoderGallery.Builders;

public abstract class BaseBuilder(
    SettingsFactory settingsFactory,
    SvgPainter svgPainter,
    NavigationService navigation)
{
    protected readonly SvgPainter _svgPainter = svgPainter;
    protected readonly NavigationService _navigation = navigation;
    protected readonly SettingsFactory _settingsFactory = settingsFactory;
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

    public abstract BuilderType Type { get; }

    public int SvgWidth { get; set; }
    public int SvgHeight { get; set; }
    public List<ClickableArea> ClickableAreas { get; } = [];

    public string GetSvgContent(int screenWidth, int screenHeight)
    {
        _navigation.SetVisitorFloor(Type);
        _settings = _settingsFactory.GetSettings(screenWidth, screenHeight);
        SvgWidth = Math.Max(240, screenWidth - _settings.ScreenMargin * 2);
        SvgHeight = Math.Max(240, screenHeight - _settings.ScreenMargin * 2);
        _svgPainter.Clear();
        ClickableAreas.Clear();
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
        return _svgPainter.SvgContent();
    }

    protected void Svg(string svgCode) => _svgPainter.Append(svgCode);

    protected virtual void Draw() { }

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