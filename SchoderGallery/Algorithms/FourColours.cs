using SchoderGallery.Painters;
using SchoderGallery.Settings;

namespace SchoderGallery.Algorithms;

public class FourColours(Colours colours, SvgPainter svgPainter) : IAlgorithm
{
    public AlgorithmType AlgorithmType => AlgorithmType.FourColours;

    public int Pattern1(ISettings settings, int width, int height, int columns, int rows, string[] palette)
    {
        if (settings.ScreenMode == ScreenMode.Portrait)
        {
            (rows, columns) = (columns, rows);
        }

        Rectangles(width, height, columns, rows, palette);

        return 0;
    }

    public int Pattern2(ISettings settings, int width, int height, int columns, int rows, string[] palette)
    {
        if (settings.ScreenMode == ScreenMode.Portrait)
        {
            (rows, columns) = (columns, rows);
        }

        int steps = 20;
        double startFactor = 0.1;
        double endFactor = 1.0;

        for (int i = 0; i <= steps; i++)
        {
            double t = (double)i / steps;

            double eased = Math.Pow(t, 2);

            double factor = startFactor + (endFactor - startFactor) * eased;

            int currentWidth = (int)(width * factor);
            int currentHeight = (int)(height * factor);

            int xOffset = (width - currentWidth) / 2;
            int yOffset = (height - currentHeight) / 2;

            Rectangles(currentWidth, currentHeight, columns, rows, palette, xOffset, yOffset);
        }

        return 0;
    }

    private int Rectangles(int width, int height, int columns, int rows, string[] palette,
        int xOffset = 0, int yOffset = 0)
    {
        var random = new Random();
        var colourMatrix = colours.FillMatrixWithColours(random, columns, rows, palette.Length);

        double cellWidth = (double)width / columns;
        double cellHeight = (double)height / rows;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int colourIndex = colourMatrix[col, row];
                string colour = palette[colourIndex];
                double x = col * cellWidth + cellWidth / 4.0;
                double y = row * cellHeight + cellHeight / 4.0;
                svgPainter.Area(x + xOffset, y + yOffset, cellWidth / 2.0, cellHeight / 2.0, colour);
            }
        }

        return 0;
    }
}