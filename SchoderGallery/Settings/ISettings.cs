using SchoderGallery.Painters;

namespace SchoderGallery.Settings;

public interface ISettings
{
    string SchoderText => "Schoder";
    string GalleryText => "Gallery";
    ScreenMode ScreenMode { get; }
    int ScreenMargin => 4;
    int RowsColumns => 7;
    int NbrOfHorizontalWindowSections { get; }
    int NbrOfVerticalWindowSections { get; }
    int NbrOfDoorDecoColumns { get; }
    int ShadowOffset => 1;

    string White => "#FFFFFF";
    string LightGray => "#C0C3C3";
    string Gray => "#808484";
    string DarkGray => "#404343";
    string Black => "#000303";
    string LinkBackground => "#C0C3C3";
    //string[] MixedColours => ["#FF6700", "#FF073A", "#FF6EC7", "#1F51FF", "#39FF14", "#FFFF33"];
    string[] MixedColours => ["#111111", "#333333", "#555555", "#777777", "#AAAAAA", "#DDDDDD"];

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