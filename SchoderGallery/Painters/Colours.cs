namespace SchoderGallery.Painters;

public class Colours
{
    public const string White = "#FFFFFF";
    public const string LightGray = "#C0C3C3";
    public const string LightestGray = "#E0E3E3";
    public const string Gray = "#808484";
    public const string DarkGray = "#404343";
    public const string Black = "#000303";
    public const string LinkBackground = "#C0C3C3";

    public readonly string[] MixedColoursBW =
    [
        "#111111",
        "#1C1C1C",
        "#262626",
        "#303030",
        "#3B3B3B",
        "#454545",
        "#505050",
        "#5A5A5A",
        "#656565",
        "#6F6F6F",
        "#7A7A7A",
        "#858585",
        "#8F8F8F",
        "#9A9A9A",
        "#A4A4A4",
        "#AFAFAF",
        "#B9B9B9",
        "#C4C4C4",
        "#CECECE",
        "#D9D9D9"
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

    public readonly string[] Blueish20Colours =
    [
        "#0303ED",
        "#1A03ED",
        "#3203ED",
        "#4A03ED",
        "#6203ED",
        "#7403ED",
        "#8303ED",
        "#9103ED",
        "#A003ED",
        "#B003ED",
        "#C003ED",
        "#D003ED",
        "#ED03D1",
        "#ED03B0",
        "#ED0390",
        "#03E3DF",
        "#03C8DF",
        "#03ADEF",
        "#0392ED",
        "#0377ED"
    ];

    public const string WarmAccentRed = "#ED0400";
    public const string WarmAccentOrange = "#FF8400";
    public const string WarmAccentMagenta = "#FF007B";
    public const string WarmAccentYellow = "#FFD800";

    public readonly string[] WarmAccentColours =
    [
        WarmAccentRed, WarmAccentOrange, WarmAccentMagenta, WarmAccentYellow
    ];

    public readonly string[] Warm20AccentColours =
    [
        "#4B0000",
        "#630000",
        "#7A0000",
        "#A30000",
        "#ED0400",
        "#FF3700",
        "#FF6400",
        "#FF8400",
        "#FF9C00",
        "#FFB400",
        "#FFCC00",
        "#FFD800",
        "#FFE200",
        "#FF007B",
        "#FF3380",
        "#FF6699",
        "#FF99B3",
        "#FFCC66",
        "#FFE999",
        "#FFFDEE"
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