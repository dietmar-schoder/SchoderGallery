namespace SchoderGallery.Constants;

public interface IConstants
{
    ScreenMode ScreenMode { get; }
    int ScreenMargin => 2;
    int RowsColumns => 7;

    string White => "#FFFFFF";
    string LightGray => "#C0C3C3";
    string MediumGray => "#808484";
    string DarkGray => "#404343";
    string Black => "#000303";

    double GapToRowColumnWidthRatio => .3;
    double WindowMarginToGapRatio => .5;
}