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
    string DarkerGray => "#707474";
    string DarkGray => "#404343";
    string Black => "#000303";

    double GapToRowColumnWidthRatio => .3;
    double WindowMarginToGapRatio => 1;
}