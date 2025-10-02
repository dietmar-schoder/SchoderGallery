using SchoderGallery.Constants;

namespace SchoderGallery.Builders;

public interface IFacadeBuilder : IBuilder
{
    string GetSvg(int width, int height);
}

public class FacadeBuilder(IConstantsFactory constantsFactory)
    : BaseBuilder(constantsFactory), IBuilder, IFacadeBuilder
{
    protected override void Draw()
    {
        ClickableAreas.Clear();
        DrawFrontWall();
        DrawWindows();

        void DrawFrontWall() =>
            Svg($"<rect x='0' y='0' width='{SvgWidth}' height='{SvgHeight}' fill='{_constants.LightGray}' stroke='none' />");

        void DrawWindows()
        {
            for (int row = 0; row < _rowsColumns; row++)
            {
                for (int column = 0; column < _rowsColumns; column++)
                {
                    double posX = _margin + column * (_windowWidth + _gap) + 0.5;
                    double posY = _margin + row * (_windowHeight + _gap) + 0.5;
                    if (row == _rowsColumns - 1 && column == _rowsColumns / 2 - 1)
                    {
                        DrawDoor(posX, posY);
                    }
                    if (row != _rowsColumns - 1 || column < _rowsColumns / 2 - 1 || column > _rowsColumns / 2 + 1)
                    {
                        DrawWindowGlasses(posX, posY, _windowWidth - 1, _windowHeight - 1);
                        DrawWindowFrames(posX, posY, _windowWidth - 1, _windowHeight - 1);
                    }
                }
            }
        }

        void DrawWindowGlasses(double x, double y, int width, int height)
        {
            Svg($"<rect x='{x}' y='{y}' width='{width}' height='{height}' fill='{_constants.White}' stroke='none' />");

            for (int col = 0; col < _windowGlassColumns; col++)
            {
                for (int row = 0; row < _windowGlassRows; row++)
                {
                    double glassX = x + col * _windowGlassColumnWidth - 0.5;
                    double glassY = y + row * _windowGlassRowHeight - 0.5;
                    Svg($"<rect x='{glassX}' y='{glassY}'" +
                        $" width='{_windowGlassColumnWidth - 1}' height='{_windowGlassRowHeight - 1}'" +
                        $" fill='{SkyColour()}' stroke='none' />");
                }
            }
        }

        void DrawWindowFrames(double x, double y, int width, int height)
        {
            for (int i = 1; i < _windowGlassColumns; i++)
            {
                double xPos = x + i * _windowGlassColumnWidth;
                Svg($"<line x1='{xPos}' y1='{y}' x2='{xPos}' y2='{y + height}' stroke='darkgray' stroke-width='1' />");
            }

            for (int j = 1; j < _windowGlassRows; j++)
            {
                double yPos = y + j * _windowGlassRowHeight;
                Svg($"<line x1='{x}' y1='{yPos}' x2='{x + width}' y2='{yPos}' stroke='darkgray' stroke-width='1' />");
            }

            Svg($"<rect x='{x}' y='{y}' width='{width}' height='{height}' fill='none' stroke='{_constants.Black}' stroke-width='1' />");
        }

        void DrawDoor(double doorX, double doorY)
        {
            int doorWidth = 3 * _windowWidth + 2 * _gap;
            int doorHeight = _windowHeight + _margin + 1;

            Svg($"<rect x='{doorX}' y='{doorY}' width='{doorWidth}' height='{doorHeight}' " +
                $"fill='{_constants.Gray}' stroke='black' stroke-width='1' />");

            Svg(EntranceText((int)doorX + 1, (int)doorY + 1, doorWidth, _windowHeight, _constants.DarkGray));
            Svg(EntranceText((int)doorX - 1, (int)doorY - 1, doorWidth, _windowHeight, _constants.White));

            ClickableAreas.Add(new ClickableArea((int)doorX, (int)doorY, doorWidth, doorHeight, "/GroundFloor"));
        }

        string EntranceText(int x, int y, int w, int h, string colour) =>
            $"<text x='{x + w / 2}' y='{y + h / 2}' " +
            $"text-anchor='middle' dominant-baseline='central' " +
            $"font-size='{ShortWindowSize * 0.2}' font-family='sans-serif' " +
            $"fill='{colour}' letter-spacing='6'>ENTRANCE</text>";
    }
}