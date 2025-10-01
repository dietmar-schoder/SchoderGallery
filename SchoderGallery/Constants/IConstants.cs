namespace SchoderGallery.Constants;

public interface IConstants
{
    ScreenMode ScreenMode { get; }

    int NumberOfFloors { get; }
    int NumberOfWindowsPerFloor { get; }

    double GapToColumnWidthRatio { get; }
    double WindowMarginToGapRatio { get; }
    double WindowHeightToWidthRatio { get; }
}