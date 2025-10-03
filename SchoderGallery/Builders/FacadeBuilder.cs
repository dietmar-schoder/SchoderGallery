using SchoderGallery.Settings;

namespace SchoderGallery.Builders;

public interface IFacadeBuilder : IBuilder { }

public class FacadeBuilder(ISettingsFactory constantsFactory)
    : BaseBuilder(constantsFactory), IBuilder, IFacadeBuilder
{
    protected override void Draw()
    {
        ClickableAreas.Clear();
        DrawFrontWall();
        DrawWindowsAndDoor();

        void DrawFrontWall() =>
            Svg($"<rect x='0' y='0' width='{SvgWidth}' height='{SvgHeight}' fill='{_constants.LightGray}' stroke='none' />");

        void DrawWindowsAndDoor()
        {
            for (int row = 1; row < _rowsColumns; row++)
            {
                for (int column = 0; column < _rowsColumns; column++)
                {
                    int x = _margin + column * (_windowWidth + _gap);
                    int y = _margin + row * (_windowHeight + _gap);
                    double lineX = x + 0.5;
                    double lineY = y + 0.5;
                    if (row == _rowsColumns - 1 && column == _rowsColumns / 2 - 1)
                    {
                        DrawDoor(lineX, lineY);
                        continue;
                    }

                    if (row != _rowsColumns - 1 || column < _rowsColumns / 2 - 1 || column > _rowsColumns / 2 + 1)
                    {
                        DrawWindowFrames1(lineX, lineY, _windowWidth - 1, _windowHeight - 1);
                        DrawWindowGlasses(x, y, _windowWidth - 1, _windowHeight - 1);
                        DrawWindowFrames2(lineX, lineY, _windowWidth - 1, _windowHeight - 1);
                    }
                }
            }

            for (int column = 0; column < _rowsColumns; column++)
            {
                int x = _margin + column * (_windowWidth + _gap);
                DrawLetters(column, x, _margin, _windowWidth - 1, _windowHeight - 1);
            }
        }

        void DrawWindowGlasses(int x, int y, int width, int height)
        {
            Svg($"<rect x='{x}' y='{y}' width='{width}' height='{height}' fill='{_constants.White}' stroke='none' />");

            for (int col = 0; col < _windowGlassColumns; col++)
            {
                for (int row = 0; row < _windowGlassRows; row++)
                {
                    double glassX = x + col * _windowGlassColumnWidth;
                    double glassY = y + row * _windowGlassRowHeight;
                    Svg($"<rect x='{glassX}' y='{glassY}'" +
                        $" width='{_windowGlassColumnWidth - 1}' height='{_windowGlassRowHeight - 1}'" +
                        $" fill='{SkyColour()}' stroke='none' />");
                }
            }
        }

        void DrawLetters(int column, int x, int y, int width, int height)
        {
            var posY = y + height / 4;
            Svg(Letter(_constants.SchoderText[column], x + 1, posY + 1, width, height, _constants.Gray));
            Svg(Letter(_constants.SchoderText[column], x - 1, posY - 1, width, height, _constants.DarkGray));
            posY = y + height * 3 / 4;
            Svg(Letter(_constants.GalleryText[column], x + 1, posY + 1, width, height, _constants.Gray));
            Svg(Letter(_constants.GalleryText[column], x - 1, posY - 1, width, height, _constants.DarkGray));
        }

        void DrawWindowFrames1(double x, double y, int width, int height) =>
            Svg($"<rect x='{x - _constants.ShadowOffset}' y='{y - _constants.ShadowOffset}'" +
                $" width='{width}' height='{height}'" +
                $" fill='{_constants.DarkGray}' stroke='{_constants.Black}' stroke-width='1' />");

        void DrawWindowFrames2(double x, double y, int width, int height)
        {
            for (int i = 1; i < _windowGlassColumns; i++)
            {
                double xPos = x + i * _windowGlassColumnWidth;
                Svg($"<line x1='{xPos}' y1='{y}' x2='{xPos}' y2='{y + height}' stroke='{_constants.DarkGray}' stroke-width='1' />");
            }

            for (int j = 1; j < _windowGlassRows; j++)
            {
                double yPos = y + j * _windowGlassRowHeight;
                Svg($"<line x1='{x}' y1='{yPos}' x2='{x + width}' y2='{yPos}' stroke='{_constants.DarkGray}' stroke-width='1' />");
            }

            Svg($"<rect x='{x}' y='{y}' width='{width}' height='{height}' fill='none' stroke='{_constants.Gray}' stroke-width='1' />");
        }

        void DrawDoor(double x, double y)
        {
            int doorWidth = 3 * _windowWidth + 2 * _gap;
            int doorHeight = _windowHeight + _margin + 1;

            Svg($"<rect x='{x - _constants.ShadowOffset}' y='{y - _constants.ShadowOffset}'" +
                $" width='{doorWidth}' height='{doorHeight}' " +
                $"fill='{_constants.DarkGray}' stroke='{_constants.Black}' stroke-width='1' />");
            Svg($"<rect x='{x}' y='{y}' width='{doorWidth}' height='{doorHeight}' " +
                $"fill='{_constants.Gray}' stroke='{_constants.DarkGray}' stroke-width='1' />");

            DrawDoorDeco(x, y, doorWidth / 2, doorHeight);
            DrawDoorDeco(x + doorWidth / 2, y, doorWidth / 2, doorHeight);

            var xMiddle = x + doorWidth / 2;
            Svg($"<line x1='{xMiddle}' y1='{y}' x2='{xMiddle}' y2='{y + doorHeight}' stroke='{_constants.DarkGray}' stroke-width='1' />");

            Svg(EntranceText((int)x + 1, (int)y + 1, doorWidth, _windowHeight, _constants.DarkGray));
            Svg(EntranceText((int)x - 1, (int)y - 1, doorWidth, _windowHeight, _constants.White));

            ClickableAreas.Add(new ClickableArea((int)x, (int)y, doorWidth, doorHeight, "/GroundFloor"));
        }

        void DrawDoorDeco(double x, double y, int doorWidth, int doorHeight)
        {
            var _decoColumns = MakeOdd(_constants.NbrOfDoorDecoColumns);
            var _decoRows = MakeOdd(_decoColumns * doorHeight / doorWidth);
            var _decoColumnWidth = (double)doorWidth / _decoColumns;
            var _decoRowHeight = (double)(doorHeight - 1) / _decoRows;
            for (int col = 1; col < _decoColumns; col += 2)
            {
                for (int row = 1; row < _decoRows; row += 2)
                {
                    double decoX = x + col * _decoColumnWidth;
                    double decoY = y + row * _decoRowHeight;
                    Svg($"<rect x='{decoX}' y='{decoY}'" +
                        $" width='{_decoColumnWidth - 1}' height='{_decoRowHeight - 1}'" +
                        $" fill='{_constants.DarkerGray}' stroke='none' />");
                }
            }

        }

        string EntranceText(int x, int y, int w, int h, string colour) =>
            $"<text x='{x + w / 2}' y='{y + h / 2}' " +
            $"text-anchor='middle' dominant-baseline='central' " +
            $"font-size='{ShortWindowSize * 0.2}' font-family='sans-serif' " +
            $"fill='{colour}' letter-spacing='6'>ENTRANCE</text>";

        string Letter(char letter, int x, int y, int w, int h, string colour) =>
            $"<text x='{x + w / 2}' y='{y}' " +
            $"text-anchor='middle' dominant-baseline='central' " +
            $"font-size='{ShortWindowSize * 0.5}' font-family='sans-serif' " +
            $"fill='{colour}' letter-spacing='6'>{letter.ToString().ToUpper()}</text>";

        int MakeOdd(int value) =>
            (value % 2 == 0) ? value + 1 : value;
    }
}