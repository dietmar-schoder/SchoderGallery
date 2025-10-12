namespace SchoderGallery.Painters;

public class Colours
{
    public const string White = "#FFFFFF";
    public const string LightGray = "#C0C3C3";
    public const string Gray = "#808484";
    public const string DarkGray = "#404343";
    public const string Black = "#000303";
    public const string LinkBackground = "#C0C3C3";

    public readonly string[] MixedColoursBW =
    [
        "#111111", "#333333", "#555555", "#777777", "#AAAAAA", "#DDDDDD"
    ];

    public const string Orange = "#FF6700";
    public const string Red = "#FF073A";
    public const string Pink = "#FF6EC7";
    public const string Blue = "#1F51FF";
    public const string LimeGreen = "#39FF14";
    public const string Yellow = "#FFFF33";

    public readonly string[] MixedColours =
    [
        Orange, Red, Pink, Blue, LimeGreen, Yellow
    ];

    public const string DeepBlue = "#0303ED";
    public const string DeepPurple = "#7403ED";
    public const string BrightCyan = "#03E3DF";
    public const string Magenta = "#ED03D1";

    public readonly string[] BlueishColours =
    [
        DeepBlue, DeepPurple, BrightCyan, Magenta
    ];

    public const string WarmAccentRed = "#ED0400";
    public const string WarmAccentOrange = "#FF8400";
    public const string WarmAccentMagenta = "#FF007B";
    public const string WarmAccentYellow = "#FFD800";

    public readonly string[] WarmAccentColours =
    [
        WarmAccentRed, WarmAccentOrange, WarmAccentMagenta, WarmAccentYellow
    ];

    public int[,] FillMatrixWithColours(Random random, int columns, int rows, int nbrOfColours)
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