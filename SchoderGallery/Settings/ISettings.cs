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
    int IconDesktop => 16;
    int FontSizeMobile => 20;
    int FontSizeDesktop => 12;
    int RowsColumns => 7;
    int NbrOfHorizontalWindowSections { get; }
    int NbrOfVerticalWindowSections { get; }
    int NbrOfDoorDecoColumns { get; }
    int ShadowOffset => 1;
    double LinkFontSizeToGapRatio => 0.75;

    string White => "#FFFFFF";
    string LightGray => "#C0C3C3";
    string Gray => "#808484";
    string DarkGray => "#404343";
    string Black => "#000303";
    string LinkBackground => "#C0C3C3";
    string[] MixedColoursBW => ["#111111", "#333333", "#555555", "#777777", "#AAAAAA", "#DDDDDD"];

    string Orange => "#FF6700";
    string Red => "#FF073A";
    string Pink => "#FF6EC7";
    string Blue => "#1F51FF";
    string LimeGreen => "#39FF14";
    string Yellow => "#FFFF33";
    string[] MixedColours => [Orange, Red, Pink, Blue, LimeGreen, Yellow];

    string DeepBlue => "#0303ED";
    string DeepPurple => "#7403ED";
    string BrightCyan => "#03E3DF";
    string Magenta => "#ED03D1";

    string[] BlueishColours => [DeepBlue, DeepPurple, BrightCyan, Magenta];

    string WarmAccentRed => "#ED0400";
    string WarmAccentOrange => "#FF8400";
    string WarmAccentMagenta => "#FF007B";
    string WarmAccentYellow => "#FFD800";

    string[] WarmAccentColours => [WarmAccentRed, WarmAccentOrange, WarmAccentMagenta, WarmAccentYellow];

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