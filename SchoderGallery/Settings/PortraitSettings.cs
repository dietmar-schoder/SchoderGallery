namespace SchoderGallery.Settings;

public class PortraitSettings : ISettings
{
    public ScreenMode ScreenMode => ScreenMode.Portrait;
    public int NbrOfHorizontalWindowSections => 3;
    public int NbrOfVerticalWindowSections => 5;
    public int NbrOfDoorDecoColumns => 14;
}