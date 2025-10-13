using SchoderGallery.DTOs;
using SchoderGallery.Painters;
using SchoderGallery.Settings;
using System.Text;

namespace SchoderGallery.Algorithms;

public class TurtleGraphics(Colours colours, SvgPainter svgPainter) : IAlgorithm
{
    public AlgorithmType AlgorithmType => AlgorithmType.TurtleGraphics;

    public ArtworkType Turtle1(ISettings settings, int width, int height, int columns, int rows, bool closePath = false)
    {
        if (settings.ScreenMode == ScreenMode.Portrait)
        {
            (rows, columns) = (columns, rows);
        }

        var random = new Random();
        var palette = colours.BlueishColours;
        var colourMatrix = colours.FillMatrixWithColours(random, columns, rows, palette.Length);

        double cellWidth = (double)width / columns;
        double cellHeight = (double)height / rows;

        var sectionCenters = new List<(double X, double Y)>();
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                double x = col * cellWidth + cellWidth / 2.0;
                double y = row * cellHeight + cellHeight / 2.0;
                (x, y) = RandomPointInSection(x, y, cellWidth, cellHeight, random);
                sectionCenters.Add((x, y));
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

        if (closePath && sectionCenters.Count > 0)
        {
            sb.Append(" Z");
        }

        svgPainter.Append($"<path d='{sb}' fill='none' stroke='{Colours.Black}' stroke-width='2' stroke-linecap='round' stroke-linejoin='round' />");

        return ArtworkType.Generative;
    }

    public ArtworkType Turtle2(ISettings settings, int width, int height, int columns, int rows, int strokeThickness = 2)
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
            (x, y) = RandomPointInSection(x, y, cellWidth, cellHeight, random);
            sb.Append($" L{x},{y}");

            steps++;
        }

        svgPainter.Append($"<path d='{sb}' fill='none' stroke='{Colours.Black}' stroke-width='{strokeThickness}' stroke-linecap='round' stroke-linejoin='round' />");

        return ArtworkType.Generative;
    }

    public ArtworkType Turtle1Smooth(ISettings settings, int width, int height, int columns, int rows, bool closePath = false)
    {
        if (settings.ScreenMode == ScreenMode.Portrait)
            (rows, columns) = (columns, rows);

        var random = new Random();
        var palette = colours.BlueishColours;

        double cellWidth = (double)width / columns;
        double cellHeight = (double)height / rows;

        var sectionCenters = new List<(double X, double Y)>();
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                double x = col * cellWidth + cellWidth / 2.0;
                double y = row * cellHeight + cellHeight / 2.0;
                (x, y) = RandomPointInSection(x, y, cellWidth, cellHeight, random);
                sectionCenters.Add((x, y));
            }
        }

        sectionCenters = [.. sectionCenters.OrderBy(_ => random.Next())];
        sectionCenters = [.. sectionCenters.Take(sectionCenters.Count / 2)];

        if (sectionCenters.Count < 2)
            return 0;

        var sb = new StringBuilder();
        sb.Append($"M{sectionCenters[0].X},{sectionCenters[0].Y}");

        for (int i = 0; i < sectionCenters.Count - 1; i++)
        {
            var (X, Y) = i > 0 ? sectionCenters[i - 1] : sectionCenters[i];
            var p1 = sectionCenters[i];
            var p2 = sectionCenters[i + 1];
            var p3 = i < sectionCenters.Count - 2 ? sectionCenters[i + 2] : p2;

            double cp1x = p1.X + (p2.X - X) / 6.0;
            double cp1y = p1.Y + (p2.Y - Y) / 6.0;
            double cp2x = p2.X - (p3.X - p1.X) / 6.0;
            double cp2y = p2.Y - (p3.Y - p1.Y) / 6.0;

            sb.Append($" C{cp1x},{cp1y} {cp2x},{cp2y} {p2.X},{p2.Y}");
        }

        if (closePath)
        {
            var (X, Y) = sectionCenters[^2];
            var p1 = sectionCenters[^1];
            var p2 = sectionCenters[0];
            var p3 = sectionCenters.Count > 2 ? sectionCenters[1] : p2;

            double cp1x = p1.X + (p2.X - X) / 6.0;
            double cp1y = p1.Y + (p2.Y - Y) / 6.0;
            double cp2x = p2.X - (p3.X - p1.X) / 6.0;
            double cp2y = p2.Y - (p3.Y - p1.Y) / 6.0;

            sb.Append($" C{cp1x},{cp1y} {cp2x},{cp2y} {p2.X},{p2.Y} Z");
        }

        svgPainter.Append($"<path d='{sb}' fill='none' stroke='{Colours.Black}' stroke-width='2' stroke-linecap='round' stroke-linejoin='round' />");

        return ArtworkType.Generative;
    }

    private static (double X, double Y) RandomPointInSection(double centerX, double centerY, double cellWidth, double cellHeight, Random random)
    {
        double offsetX = (random.NextDouble() - 0.5) * cellWidth;
        double offsetY = (random.NextDouble() - 0.5) * cellHeight;

        return (centerX + offsetX, centerY + offsetY);
    }
}