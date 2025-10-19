using SchoderGallery.Painters;

namespace SchoderGallery.Settings;

public interface ISettings
{
    string SchoderText => "Schoder";
    string GalleryText => "Gallery";
    ScreenMode ScreenMode { get; }
    int OuterMargin => 4;
    int TinyMargin => 2;

    int IconSizeMobile => 32;
    int SmallFontSizeMobile => 12;
    int FontSizeMobile => 14;
    int LargeFontSizeMobile => 16;

    int IconSizeDesktop => 24;
    int SmallFontSizeDesktop => 12;
    int FontSizeDesktop => 16;
    int LargeFontSizeDesktop => 24;

    int RowsColumns => 7;
    int NbrOfHorizontalWindowSections { get; }
    int NbrOfVerticalWindowSections { get; }
    int NbrOfDoorDecoColumns { get; }
    int ShadowOffset => 1;
    double LinkFontSizeToGapRatio => 0.75;

    double GapToRowColumnWidthRatio => .3;
    double WindowMarginToGapRatio => 1;
    int WallThickness => 8;

    void DrawFacadeLetters(
        SvgPainter svgPainter,
        ISettings settings,
        int totalWidth,
        int rowsColumns,
        int margin,
        int gap,
        int windowWidth,
        int windowHeight,
        int shortWindowSize);
}