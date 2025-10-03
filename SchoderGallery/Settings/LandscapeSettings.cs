using System.Runtime;
using System.Text;

namespace SchoderGallery.Settings;

public class LandscapeSettings : ISettings
{
    public ScreenMode ScreenMode => ScreenMode.Landscape;
    public int NbrOfHorizontalWindowSections => 5;
    public int NbrOfVerticalWindowSections => 3;
    public int NbrOfDoorDecoColumns => 21;
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
        windowWidth /= 2;
        var spaceNeeded = rowsColumns * (windowWidth + gap);
        var leftMargin = margin / 2 + totalWidth / 2 - spaceNeeded / 2;
        for (int column = 0; column < rowsColumns; column++)
        {
            int x = leftMargin + column * (windowWidth + gap);
            var y = margin + windowHeight / 5;
            svg.Append(Letter(settings.SchoderText[column], x + 1, y + 1, windowWidth, settings.Gray));
            svg.Append(Letter(settings.SchoderText[column], x - 1, y - 1, windowWidth, settings.DarkGray));
            y = margin + windowHeight * 9 / 10;
            svg.Append(Letter(settings.GalleryText[column], x + 1, y + 1, windowWidth, settings.Gray));
            svg.Append(Letter(settings.GalleryText[column], x - 1, y - 1, windowWidth, settings.DarkGray));

            string Letter(char letter, int x, int y, int w, string colour) =>
                $"<text x='{x + w / 2.0}' y='{y}' " +
                $"text-anchor='Middle' dominant-baseline='Middle' " +
                $"font-size='{shortWindowSize * 0.5}' font-family='sans-serif' " +
                $"fill='{colour}'>{letter.ToString().ToUpper()}</text>";
        }
    }
}