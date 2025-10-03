namespace SchoderGallery.Algorithms;

public class ColourGenerator
{
    public static int[,] FillMatrixWithColours(Random random, int columns, int rows, int nbrOfColours)
    {
        int[,] matrix = new int[columns, rows];
        bool[] allowed = new bool[nbrOfColours];

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                for (int i = 0; i < nbrOfColours; i++) allowed[i] = true;

                if (x > 0) allowed[matrix[x - 1, y]] = false;
                if (y > 0) allowed[matrix[x, y - 1]] = false;

                int colorIndex = random.Next(nbrOfColours);
                while (!allowed[colorIndex])
                    colorIndex = (colorIndex + 1) % nbrOfColours;

                matrix[x, y] = colorIndex;
            }
        }

        return matrix;
    }
}