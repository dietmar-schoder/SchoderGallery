using SchoderGallery.Painters;

namespace SchoderGallery.Settings;

public class LandscapeSettings : ISettings
{
    public ScreenMode ScreenMode => ScreenMode.Landscape;
    public int NbrOfHorizontalWindowSections => 5;
    public int NbrOfVerticalWindowSections => 3;
    public int NbrOfDoorDecoColumns => 33;
    public void DrawFacadeLetters(
        SvgPainter svgPainter,
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
            svgPainter.Append(Letter(settings.SchoderText[column], x + 1, y + 1, windowWidth, settings.Gray));
            svgPainter.Append(Letter(settings.SchoderText[column], x - 1, y - 1, windowWidth, settings.DarkGray));
            y = margin + windowHeight * 9 / 10;
            svgPainter.Append(Letter(settings.GalleryText[column], x + 1, y + 1, windowWidth, settings.Gray));
            svgPainter.Append(Letter(settings.GalleryText[column], x - 1, y - 1, windowWidth, settings.DarkGray));

            string Letter(char letter, int x, int y, int w, string colour) =>
                $"<text x='{x + w / 2.0}' y='{y}' " +
                $"text-anchor='middle' dominant-baseline='middle' " +
                $"font-size='{shortWindowSize * 0.5}' font-family='sans-serif' " +
                $"fill='{colour}'>{letter.ToString().ToUpper()}</text>";
        }
    }
}