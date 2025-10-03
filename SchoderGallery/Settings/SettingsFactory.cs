namespace SchoderGallery.Settings;

public class SettingsFactory(IEnumerable<ISettings> settingsList)
{
    private readonly Dictionary<ScreenMode, ISettings> _settingsList = settingsList.ToDictionary(b => b.ScreenMode, b => b);

    public ISettings GetSettings(int screenWidth, int screenHeight) =>
        _settingsList[GetScreenMode(screenWidth > screenHeight)];

    private static ScreenMode GetScreenMode(bool landscapeMode) =>
        landscapeMode ? ScreenMode.Landscape : ScreenMode.Portrait;
}