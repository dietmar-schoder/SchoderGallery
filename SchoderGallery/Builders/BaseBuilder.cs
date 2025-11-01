using SchoderGallery.Navigation;
using SchoderGallery.Painters;
using SchoderGallery.Services;
using SchoderGallery.Settings;

namespace SchoderGallery.Builders;

public abstract class BaseBuilder(
    SettingsFactory settingsFactory,
    SvgPainter svgPainter,
    NavigationService navigation,
    IGalleryService galleryService)
{
    protected readonly SvgPainter _svgPainter = svgPainter;
    protected readonly NavigationService _navigation = navigation;
    protected readonly SettingsFactory _settingsFactory = settingsFactory;
    protected readonly IGalleryService _galleryService = galleryService;
    protected Random _random = new();
    protected ISettings _settings;

    protected int _width20;
    protected int _width33;
    protected int _width50;
    protected int _width80;
    protected int _height25;
    protected int _height33;
    protected int _height50;
    protected int _height66;

    protected int _rowsColumns;
    protected int _gap;
    protected int _margin;
    protected int _windowWidth;
    protected int _windowHeight;
    protected int _windowGlassColumns;
    protected int _windowGlassRows;
    protected double _windowGlassColumnWidth;
    protected double _windowGlassRowHeight;
    protected int _smallFontSize;
    protected int _fontSize;
    protected int _largeFontSize;

    protected ScreenMode ScreenMode => SvgWidth > SvgHeight ? ScreenMode.Landscape : ScreenMode.Portrait;
    protected int ShortSize => ScreenMode == ScreenMode.Portrait ? SvgWidth : SvgHeight;
    protected int ShortWindowSize => ScreenMode == ScreenMode.Portrait ? _windowWidth : _windowHeight;

    public abstract FloorType FloorType { get; }

    public int SvgWidth { get; set; }

    public int SvgHeight { get; set; }

    public bool IsMobile => SvgWidth <= 768;

    public List<ClickableArea> ClickableAreas { get; } = [];

    public Visitor Visitor { get; set; }

    public void Init(int screenWidth, int screenHeight)
    {
        _settings = _settingsFactory.GetSettings(screenWidth, screenHeight);

        SvgWidth = Math.Max(240, screenWidth - _settings.OuterMargin * 2);
        SvgHeight = Math.Max(240, screenHeight - _settings.OuterMargin * 2);

        _smallFontSize = IsMobile ? _settings.SmallFontSizeMobile : _settings.SmallFontSizeDesktop;
        _fontSize = IsMobile ? _settings.FontSizeMobile : _settings.FontSizeDesktop;
        _largeFontSize = IsMobile ? _settings.LargeFontSizeMobile : _settings.LargeFontSizeDesktop;

        _width20 = SvgWidth / 5;
        _width50 = SvgWidth / 2;
        _width33 = SvgWidth / 3;
        _width80 = SvgWidth * 4 / 5;
        _height25 = SvgHeight / 4;
        _height33 = SvgHeight / 3;
        _height50 = SvgHeight / 2;
        _height66 = _height33 * 2;

        _svgPainter.Clear();
        ClickableAreas.Clear();
    }

    public async Task<string> GetSvgContentAsync(int screenWidth, int screenHeight)
    {
        Init(screenWidth, screenHeight);

        Visitor = await _navigation.GetInitVisitorAsync();

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
        await DrawAsync();
        Draw();
        return _svgPainter.SvgContent();
    }

    protected void Svg(string svgCode) => _svgPainter.Append(svgCode);

    protected int IconSize => IsMobile ? _settings.IconSizeMobile : _settings.IconSizeDesktop;

    protected virtual async Task DrawAsync() { await Task.CompletedTask; }

    protected virtual void Draw() { }

    protected static string ConvertToParagraphs(string text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        var paragraphs = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var paragraphTags = paragraphs.Select(p => $"<p>{p.Trim()}</p>");
        return string.Join("", paragraphTags);
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
