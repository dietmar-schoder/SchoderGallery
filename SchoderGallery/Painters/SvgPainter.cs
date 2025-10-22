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
        Text(x + 1, y + 1, content, fontSize, Colours.LightestGray);
        Text(x, y, content, fontSize, Colours.Black);
    }

    public void IconLeftArrow(int x, int y, int size, int thickness = 1)
    {
        var (xMid, yMid) = IconMiddle(x, y, size);
        Append($"<path d='M{xMid},{y}L{x},{yMid}L{xMid},{y + size}M{x},{yMid}L{x + size},{yMid}' fill='none' stroke='{Colours.Gray}' stroke-width='{thickness}' />");
    }

    public void IconRefresh(int x, int y, int size, int thickness = 1)
    {
        var (xMid, yMid) = IconMiddle(x, y, size);
        Append($"<path d='M{xMid},{y} A{size / 2},{size / 2} 0 1,0 {x + size},{yMid} L{x + size * 0.7},{y + size * 0.8} M{x + size},{yMid} L{x + size},{y + size}' fill='none' stroke='{Colours.Gray}' stroke-width='{thickness}' />");
    }

    public void IconLeft(int x, int y, int size, int thickness = 1)
    {
        var (xMid, yMid) = IconMiddle(x, y, size);
        Append($"<path d='M{xMid},{y}L{x},{yMid}L{xMid},{y + size}' fill='none' stroke='{Colours.Gray}' stroke-width='{thickness}' />");
    }

    public void IconRight(int x, int y, int size, int thickness = 1)
    {
        var (xMid, yMid) = IconMiddle(x, y, size);
        Append($"<path d='M{xMid},{y}L{x + size},{yMid}L{xMid},{y + size}' fill='none' stroke='{Colours.Gray}' stroke-width='{thickness}' />");
    }

    private static (int xMid, int yMid) IconMiddle(int x, int y, int size) =>
        (x + size / 2, y + size / 2);

    public void Image(int width, int height, string filename) =>
        Append($"<image x='0' y='0' width='{width}' height='{height}' href='{filename}' preserveAspectRatio='none' />");
}