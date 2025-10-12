using SchoderGallery.Painters;
using SchoderGallery.Settings;
using System.Text;

namespace SchoderGallery.Algorithms;

public class TurtleGraphics(Colours colourGenerator) : IAlgorithm
{
    public AlgorithmType AlgorithmType => AlgorithmType.TurtleGraphics;

    public int Turtle1(ISettings settings, SvgPainter svgPainter, int width, int height, int columns, int rows)
    {
        if (settings.ScreenMode == ScreenMode.Portrait)
        {
            (rows, columns) = (columns, rows);
        }

        var random = new Random();
        var colours = colourGenerator.BlueishColours;
        var colourMatrix = colourGenerator.FillMatrixWithColours(random, columns, rows, colours.Length);

        double cellWidth = (double)width / columns;
        double cellHeight = (double)height / rows;

        var sectionCenters = new List<(double X, double Y)>();
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                double centerX = col * cellWidth + cellWidth / 2.0;
                double centerY = row * cellHeight + cellHeight / 2.0;
                sectionCenters.Add((centerX, centerY));
            }
        }

        sectionCenters = [.. sectionCenters.OrderBy(_ => random.Next())];
        sectionCenters = [.. sectionCenters.Take(sectionCenters.Count / 2)];
        var sb = new StringBuilder();
        bool first = true;
        foreach (var (X, Y) in sectionCenters)
        {
            sb.Append(first ? $"M{X},{Y}" : $" L{X},{Y}");
            first = false;
        }

        svgPainter.Append($"<path d='{sb}' fill='none' stroke='{Colours.Black}' stroke-width='2' stroke-linecap='round' stroke-linejoin='round' />");

        return 0;
    }

    public int Turtle2(ISettings settings, SvgPainter svgPainter, int width, int height, int columns, int rows, int strokeThickness = 2)
    {
        if (settings.ScreenMode == ScreenMode.Portrait)
        {
            (rows, columns) = (columns, rows);
        }

        double cellWidth = (double)width / columns;
        double cellHeight = (double)height / rows;

        var random = new Random();
        var visited = new bool[rows, columns];
        var sectionCenters = new (double X, double Y)[rows, columns];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                sectionCenters[row, col] = (col * cellWidth + cellWidth / 2.0, row * cellHeight + cellHeight / 2.0);
                visited[row, col] = false;
            }
        }

        int currentRow = random.Next(rows);
        int currentCol = random.Next(columns);
        visited[currentRow, currentCol] = true;

        var sb = new StringBuilder();
        sb.Append($"M{sectionCenters[currentRow, currentCol].X},{sectionCenters[currentRow, currentCol].Y}");

        int steps = 0;
        int maxSteps = rows * columns * 2;

        while (steps < maxSteps && visited.Cast<bool>().Any(v => !v))
        {
            var neighbors = new List<(int r, int c)>();
            for (int dr = -1; dr <= 1; dr++)
            {
                for (int dc = -1; dc <= 1; dc++)
                {
                    if ((dr != 0 || dc != 0) &&
                        currentRow + dr >= 0 && currentRow + dr < rows &&
                        currentCol + dc >= 0 && currentCol + dc < columns)
                    {
                        neighbors.Add((currentRow + dr, currentCol + dc));
                    }
                }
            }

            var unvisitedNeighbors = neighbors.Where(n => !visited[n.r, n.c]).ToList();
            var (r, c) = unvisitedNeighbors.Count > 0
                ? unvisitedNeighbors[random.Next(unvisitedNeighbors.Count)]
                : neighbors[random.Next(neighbors.Count)];

            currentRow = r;
            currentCol = c;
            visited[currentRow, currentCol] = true;

            var (x, y) = sectionCenters[currentRow, currentCol];
            sb.Append($" L{x},{y}");

            steps++;
        }

        svgPainter.Append($"<path d='{sb}' fill='none' stroke='{Colours.Black}' stroke-width='{strokeThickness}' stroke-linecap='round' stroke-linejoin='round' />");

        return 0;
    }
}