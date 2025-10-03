using System.Text;

namespace SchoderGallery.Settings;

public class PortraitSettings : ISettings
{
    public ScreenMode ScreenMode => ScreenMode.Portrait;
    public int NbrOfHorizontalWindowSections => 3;
    public int NbrOfVerticalWindowSections => 5;
    public int NbrOfDoorDecoColumns => 14;
    public void DrawFacadeLetters(
        StringBuilder svg,
        ISettings settings,
        int totalWidth,
        int rowsColumns,
        int margin,
        int gap,
        int windowWidth,
        int windowHeight,
        int shortWindowSize)
    {
        for (int column = 0; column < rowsColumns; column++)
        {
            int x = margin + column * (windowWidth + gap);
            var y = margin + windowHeight / 5;
            svg.Append(Letter(settings.SchoderText[column], x + 1, y + 1, windowWidth, settings.Gray));
            svg.Append(Letter(settings.SchoderText[column], x - 1, y - 1, windowWidth, settings.DarkGray));
            y = margin + windowHeight * 4 / 5;
            svg.Append(Letter(settings.GalleryText[column], x + 1, y + 1, windowWidth, settings.Gray));
            svg.Append(Letter(settings.GalleryText[column], x - 1, y - 1, windowWidth, settings.DarkGray));

            string Letter(char letter, int x, int y, int w, string colour) =>
                $"<text x='{x + w / 2.0}' y='{y}' " +
                $"text-anchor='middle' dominant-baseline='central' " +
                $"font-size='{shortWindowSize * 0.5}' font-family='sans-serif' " +
                $"fill='{colour}'>{letter.ToString().ToUpper()}</text>";
        }
    }
}