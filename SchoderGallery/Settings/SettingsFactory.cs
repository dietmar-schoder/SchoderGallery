namespace SchoderGallery.Settings;

public interface ISettingsFactory
{
    ISettings GetConstants(int screenWidth, int screenHeight);
}

public class SettingsFactory(IEnumerable<ISettings> constantsList) : ISettingsFactory
{
    public ISettings GetConstants(int screenWidth, int screenHeight) =>
        constantsList.FirstOrDefault(c => c.ScreenMode == GetScreenMode(screenWidth, screenHeight));

    private static ScreenMode GetScreenMode(int screenWidth, int screenHeight) =>
        screenWidth > screenHeight ? ScreenMode.Landscape : ScreenMode.Portrait;
}