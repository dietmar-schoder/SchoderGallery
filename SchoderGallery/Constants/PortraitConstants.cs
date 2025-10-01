namespace SchoderGallery.Constants;

public class PortraitConstants : IConstants
{
    public ScreenMode ScreenMode => ScreenMode.Portrait;

    public int NumberOfFloors => 7;
    public int NumberOfWindowsPerFloor => 7;

    public double GapToColumnWidthRatio => .3;
    public double WindowMarginToGapRatio => .5;
    public double WindowHeightToWidthRatio => 7.0 / 5.0;
}