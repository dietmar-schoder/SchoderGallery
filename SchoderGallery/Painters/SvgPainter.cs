using SchoderGallery.Settings;
using System.Text;

namespace SchoderGallery.Painters;

public class SvgPainter
{
    private StringBuilder _svg;

    public void Clear() =>
        _svg = new StringBuilder();

    public string SvgContent() =>
        _svg.ToString();

    public void Append(string svgCode) => _svg.Append(svgCode);

    public void Border(double x, double y, int width, int height, string colour, int thickness = 1) =>
        Append($"<rect x='{x + 0.5}' y='{y + 0.5}' width='{width - 1}' height='{height - 1}' fill='none' stroke='{colour}' stroke-width='{thickness}' />");

    public void Circle(int x, int y, int diameter, string strokeColour, int strokeWidth = 1, string fillColour = "none")
    {
        var radius = diameter / 2;
        Append($"<circle cx='{x + radius + 0.5}' cy='{y + radius + 0.5}' r='{radius}' fill='{fillColour}' stroke='{strokeColour}' stroke-width='{strokeWidth}' />");
    }

    public void Circle2(int cx, int cy, int diameter, string strokeColour, int strokeWidth = 1, string fillColour = "none")
    {
        var radius = diameter / 2;
        Append($"<circle cx='{cx + 0.5}' cy='{cy + 0.5}' r='{radius}' fill='{fillColour}' stroke='{strokeColour}' stroke-width='{strokeWidth}' />");
    }

    public void Area(double x, double y, double width, double height, string colour, string cssClass = default) =>
        Append($"<rect x='{x}' y='{y}' width='{width}' height='{height}' fill='{colour}' stroke='none' {cssClass} />");

    public void Area(double x, double y, int width, int height, string colour, string borderColour, int thickness = 1) =>
        Append($"<rect x='{x + 0.5}' y='{y + 0.5}' width='{width - 1}' height='{height - 1}' fill='{colour}' stroke='{borderColour}' stroke-width='{thickness}' />");

    public void Sunlight(double x, double y, int width, int height, ISettings settings)
    {
        var offset = settings.ShadowOffset;
        Area(x - offset, y - offset, width + offset * 2, height + offset * 2, Colours.Black, Colours.Black);
        Area(x, y, width + offset, height + offset, Colours.White, Colours.White);
    }

    public void VerticalLine(double x, double y, int length, string colour, int thickness = 1) =>
        Append($"<line x1='{x}' y1='{y}' x2='{x}' y2='{y + length}' stroke='{colour}' stroke-width='{thickness}' />");

    public void HorizontalLine(double x, double y, int length, string colour, int thickness = 1) =>
        Append($"<line x1='{x}' y1='{y}' x2='{x + length}' y2='{y}' stroke='{colour}' stroke-width='{thickness}' />");

    public void Text(int x, int y, string content, int fontSize, string colour, int letterSpacing = 6) =>
        Append($"<text x='{x}' y='{y}' text-anchor='middle' dominant-baseline='middle' font-size='{fontSize}' font-family='sans-serif' fill='{colour}' letter-spacing='{letterSpacing}'>{content}</text>");

    public void TextLeft(int x, int y, string content, int fontSize, string colour, double letterSpacing = 1.6) =>
        Append($"<text x='{x}' y='{y}' dominant-baseline='middle' font-size='{fontSize}' font-family='sans-serif' fill='{colour}' letter-spacing='{letterSpacing}'>{content}</text>");

    public void TextRight(int x, int y, string content, int fontSize, string colour, double letterSpacing = 1.6) =>
        Append($"<text x='{x}' y='{y}' text-anchor='end' dominant-baseline='middle' font-size='{fontSize}' font-family='sans-serif' fill='{colour}' letter-spacing='{letterSpacing}'>{content}</text>");

    public void TextLink(int x, int y, string content, int fontSize)
    {
        Text(x + 1, y + 1, content, fontSize, Colours.White);
        Text(x - 1, y - 1, content, fontSize, Colours.Black);
        Text(x, y, content, fontSize, Colours.Gray);
    }

    public void IconLeftArrow(int x, int y, int size, int thickness = 1)
    {
        var (xMid, yMid) = IconMiddle(x, y, size);
        Append($"<path d='M{xMid},{y}L{x},{yMid}L{xMid},{y + size}M{x},{yMid}L{x + size},{yMid}' fill='none' stroke='{Colours.DarkGray}' stroke-width='{thickness}' />");
    }

    public void IconRefresh(int x, int y, int size, int thickness = 1)
    {
        var (xMid, yMid) = IconMiddle(x, y, size);
        Append($"<path d='M{xMid},{y} A{size / 2},{size / 2} 0 1,0 {x + size},{yMid} L{x + size * 0.7},{y + size * 0.8} M{x + size},{yMid} L{x + size},{y + size}' fill='none' stroke='{Colours.DarkGray}' stroke-width='{thickness}' />");
    }

    public void IconLeft(int x, int y, int size, int thickness = 1)
    {
        var (xMid, yMid) = IconMiddle(x, y, size);
        Append($"<path d='M{xMid},{y}L{x},{yMid}L{xMid},{y + size}' fill='none' stroke='{Colours.DarkGray}' stroke-width='{thickness}' />");
    }

    public void IconRight(int x, int y, int size, int thickness = 1)
    {
        var (xMid, yMid) = IconMiddle(x, y, size);
        Append($"<path d='M{xMid},{y}L{x + size},{yMid}L{xMid},{y + size}' fill='none' stroke='{Colours.DarkGray}' stroke-width='{thickness}' />");
    }

    public void IconQuestionMark(int x, int y, int size, int thickness = 1)
    {
        var (xMid, yMid) = IconMiddle(x, y, size);
        int quarterX = x + size / 4;
        int threeQuarterX = x + 3 * size / 4;
        int quarterY = y + size / 4;
        int threeQuarterY = y + 3 * size / 4;
        Append($"<path d='M{quarterX},{quarterY} A{size / 4},{size / 4} 0 0 1 {xMid},{y} A{size / 4},{size / 4} 0 0 1 {threeQuarterX},{quarterY} A{size / 4},{size / 4} 0 0 1 {xMid},{yMid} L{xMid},{threeQuarterY}' fill='none' stroke='{Colours.DarkGray}' stroke-width='{thickness}' />");
        int circleRadius = 2;
        Append($"<circle cx='{xMid}' cy='{y + size - circleRadius}' r='{circleRadius}' fill='{Colours.DarkGray}' />");
    }

    public void IconClose(int x, int y, int size, int thickness = 1) =>
        Append($"<path d='M{x},{y}L{x + size},{y + size}M{x + size},{y}L{x},{y + size}' fill='none' stroke='{Colours.DarkGray}' stroke-width='{thickness}' />");

    public void IconCafe(int x, int y, int sizeFactor, string colour, double thickness) =>
        Append($"<g transform='translate({x},{y}) scale({sizeFactor})'>" +
            $"<path d='M 1 1 H 9 V 9 H 1 Z M 7 6 V 5 H 3 V 6 A 1 1 0 0 0 7 6 A 0.5 0.5 0 0 0 8 6 A 0.5 0.5 0 0 0 7 6 M 4 3 C 3.6667 3.3333 4.3333 3.6667 4 4 M 5 2 C 4.3333 2.6667 5.6667 3.3333 5 4 M 6 3 C 5.6667 3.3333 6.3333 3.6667 6 4' fill='none' stroke='{colour}' stroke-width='{thickness}' />" +
            $"</g>");

    public void IconInfo(int x, int y, int sizeFactor, string colour, double thickness) =>
        Append($"<g transform='translate({x},{y}) scale({sizeFactor})'>" +
            $"<path d='M 1 1 H 9 V 9 H 1 Z M 4 3 A 1 1 0 0 0 6 3 A 1 1 0 0 0 4 3 M 4 5 H 6 V 8 H 4 Z' fill='none' stroke='{colour}' stroke-width='{thickness}' />" +
            $"</g>");

    public void IconShop(int x, int y, int sizeFactor, string colour, double thickness) =>
        Append($"<g transform='translate({x},{y}) scale({sizeFactor})'>" +
            $"<path d='M 1 1 H 9 V 9 H 1 Z M 2 4 C 2 5 2 7 4 7 H 6 C 8 7 8 5 8 4 Z M 3 4 L 4 3 M 7 4 L 6 3 M 3.5 5 V 6 M 5 5 V 6 M 6.5 5 V 6' fill='none' stroke='{colour}' stroke-width='{thickness}' />" +
            $"</g>");

    public void IconToilets(int x, int y, int sizeFactor, string colour, double thickness) =>
        Append($"<g transform='translate({x},{y}) scale({sizeFactor})'>" +
            $"<path d='M 1 1 H 9 V 9 H 1 Z M 2 3 L 2.875 7 L 3.75 3 L 4.625 7 L 5.5 3 M 8 4 A 1 1 0 0 0 6 4 V 6 A 1 1 0 0 0 8 6' fill='none' stroke='{colour}' stroke-width='{thickness}' />" +
            $"</g>");

    private static (int xMid, int yMid) IconMiddle(int x, int y, int size) =>
        (x + size / 2, y + size / 2);

    public void Image(int width, int height, string filename) =>
        Append($"<image x='0' y='0' width='{width}' height='{height}' href='{filename}' preserveAspectRatio='none' />");

    public void Thumbnail(int x, int y, int width, int height, string filename) =>
        Append($"<image x='{x}' y='{y}' width='{width}' height='{height}' href='{filename}' preserveAspectRatio='none' />");

    public void FloorPattern1(double x, double y, int width, int height, int spacing, string colour = Colours.FloorPattern)
    {
        int cols = width / spacing;
        int rows = height / spacing;
        double remainderX = width - cols * spacing;
        double remainderY = height - rows * spacing;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                double sx = x + col * spacing + remainderX / 2;
                double sy = y + row * spacing + remainderY / 2;
                double s = spacing;
                double offset = spacing / 4.0;

                Append($@"<path d='M {sx + s / 2},{sy + offset} L {sx + s - offset},{sy + s / 2} L {sx + s / 2},{sy + s - offset} L {sx + offset},{sy + s / 2} Z' fill='none' stroke='{colour}' stroke-width='1' />");
            }
        }

        for (int row = 1; row < rows; row++)
        {
            for (int col = 1; col < cols; col++)
            {
                double sx = x + col * spacing + remainderX / 2;
                double sy = y + row * spacing + remainderY / 2;
                double radius = spacing;
                Append($@"<circle cx='{sx}' cy='{sy}' r='{radius}' fill='none' stroke='{colour}' stroke-width='1' />");
            }
        }
    }

    public void FloorPattern2(double x, double y, int width, int height, int spacing, string colour = Colours.FloorPattern)
    {
        int cols = width / spacing;
        int rows = height / spacing;
        double remainderX = width - cols * spacing;
        double remainderY = height - rows * spacing;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                double sx = x + col * spacing + remainderX / 2;
                double sy = y + row * spacing + remainderY / 2;
                double s = spacing;
                double offset = spacing / 8.0;

                Append($@"<path d='M {sx + s / 2},{sy + offset} L {sx + s - offset},{sy + s / 2} L {sx + s / 2},{sy + s - offset} L {sx + offset},{sy + s / 2} Z' fill='{colour}' stroke-width='0' />");
            }
        }
    }

    public void FloorPattern3(double x, double y, int width, int height, int spacing, string colour = Colours.FloorPattern)
    {
        int cols = width / spacing;
        int rows = height / spacing;
        double remainderX = width - cols * spacing;
        double remainderY = height - rows * spacing;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                double sx = x + col * spacing + remainderX / 2;
                double sy = y + row * spacing + remainderY / 2;
                double s = spacing;
                double offset = s / 8.0;

                Append($@"<path d='M {sx + s / 2},{sy + offset} L {sx + s - offset},{sy + s / 2} L {sx + s / 2},{sy + s - offset} L {sx + offset},{sy + s / 2} Z' fill='{colour}' stroke-width='0' />");
                double radius = spacing / 3.8;
                Append($@"<circle cx='{sx + s / 2}' cy='{sy + s / 2}' r='{radius}' fill='{Colours.Background}' stroke-width='0' />");
            }
        }
    }
}
