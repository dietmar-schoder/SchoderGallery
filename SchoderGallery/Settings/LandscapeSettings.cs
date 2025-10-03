namespace SchoderGallery.Settings;

public class LandscapeSettings : ISettings
{
    public ScreenMode ScreenMode => ScreenMode.Landscape;
    public int NbrOfHorizontalWindowSections => 5;
    public int NbrOfVerticalWindowSections => 3;
    public int NbrOfDoorDecoColumns => 21;
}