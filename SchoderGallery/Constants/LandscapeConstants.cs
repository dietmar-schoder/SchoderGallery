namespace SchoderGallery.Constants;

public class LandscapeConstants : IConstants
{
    public ScreenMode ScreenMode => ScreenMode.Landscape;

    public int NumberOfFloors => 5;
    public int NumberOfWindowsPerFloor => 15;

    public double GapToColumnWidthRatio => .3;
    public double WindowMarginToGapRatio => .5;
    public double WindowHeightToWidthRatio => 5.0 / 7.0;
}