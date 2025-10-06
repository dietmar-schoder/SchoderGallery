using SchoderGallery.Painters;
using SchoderGallery.Settings;

namespace SchoderGallery.Algorithms;

public class FourColours(ColourGenerator colourGenerator) : IAlgorithm
{
    public AlgorithmType AlgorithmType => AlgorithmType.FourColours;

    public int Pattern1(ISettings settings, SvgPainter svgPainter, int width, int height, int columns, int rows)
    {
        if (settings.ScreenMode == ScreenMode.Portrait)
        {
            (rows, columns) = (columns, rows);
        }

        var random = new Random();
        var colours = settings.BlueishColours;
        var colourMatrix = colourGenerator.FillMatrixWithColours(random, columns, rows, colours.Length);

        double cellWidth = (double)width / columns;
        double cellHeight = (double)height / rows;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int colourIndex = colourMatrix[col, row];
                string colour = colours[colourIndex];
                double x = col * cellWidth + cellWidth / 4.0;
                double y = row * cellHeight + cellHeight / 4.0;
                svgPainter.Area(x, y, cellWidth / 2.0, cellHeight / 2.0, colour);
            }
        }

        return 0;
    }
}