using System.Text;

namespace SchoderGallery.Settings;

public interface ISettings
{
    string SchoderText => "Schoder";
    string GalleryText => "Gallery";
    ScreenMode ScreenMode { get; }
    int ScreenMargin => 2;
    int RowsColumns => 7;
    int NbrOfHorizontalWindowSections { get; }
    int NbrOfVerticalWindowSections { get; }
    int NbrOfDoorDecoColumns { get; }
    double ShadowOffset => 1;

    string White => "#FFFFFF";
    string LightGray => "#C0C3C3";
    string Gray => "#808484";
    string DarkGray => "#404343";
    string Black => "#000303";
    string LinkBackground => "#C0C3C3";
    string[] MixedColours => ["#FF6700", "#FF073A", "#FF6EC7", "#1F51FF", "#39FF14", "#FFFF33"];

    double GapToRowColumnWidthRatio => .3;
    double WindowMarginToGapRatio => 1;

    void DrawFacadeLetters(
        StringBuilder svg,
        ISettings settings,
        int totalWidth,
        int rowsColumns,
        int margin,
        int gap,
        int windowWidth,
        int windowHeight,
        int shortWindowSize);
}