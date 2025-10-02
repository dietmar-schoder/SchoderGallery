namespace SchoderGallery.Constants;

public interface IConstants
{
    ScreenMode ScreenMode { get; }

    string White => "#FFFFFF";
    string LightGray => "#C0C3C3";
    string MediumGray => "#808484";
    string DarkGray => "#404343";
    string Black => "#000303";

    int NumberOfFloors { get; }
    int NumberOfWindowsPerFloor { get; }

    double GapToColumnWidthRatio { get; }
    double WindowMarginToGapRatio { get; }
    double WindowHeightToWidthRatio { get; }
}