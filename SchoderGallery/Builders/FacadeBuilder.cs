using SchoderGallery.Algorithms;
using SchoderGallery.Settings;

namespace SchoderGallery.Builders;

public class FacadeBuilder(SettingsFactory settingsFactory, ColourGenerator colourGenerator)
    : BaseBuilder(settingsFactory), IBuilder
{
    public BuilderType Type => BuilderType.Facade;
    public int Interval => 5000;

    protected override void Draw()
    {
        ClickableAreas.Clear();
        DrawFrontWall();
        DrawWindowsAndDoor();

        void DrawFrontWall() => Area(0, 0, SvgWidth, SvgHeight, _settings.LightGray);

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
                        DrawWindowFrames(lineX - 1, lineY - 1, _windowWidth - 2, _windowHeight - 2);
                        DrawWindowGlasses(x, y, _windowWidth - 1, _windowHeight - 1);
                    }
                }
            }

            _settings.DrawFacadeLetters(_svg, _settings, SvgWidth, _rowsColumns, _margin, _gap, _windowWidth, _windowHeight, ShortWindowSize);
        }

        void DrawWindowGlasses(int x, int y, int width, int height)
        {
            for (int col = 0; col < _windowGlassColumns; col++)
            {
                for (int row = 0; row < _windowGlassRows; row++)
                {
                    double glassX = x + col * _windowGlassColumnWidth - 0.5;
                    double glassY = y + row * _windowGlassRowHeight - 0.5;
                    Area(glassX, glassY, _windowGlassColumnWidth - 1, _windowGlassRowHeight - 1, SkyColour());
                }
            }
        }

        void DrawWindowFrames(double x, double y, int width, int height)
        {
            Area(x + _settings.ShadowOffset, y + _settings.ShadowOffset, width, height, _settings.White, _settings.White);
            Area(x - _settings.ShadowOffset, y - _settings.ShadowOffset, width, height, _settings.DarkGray, _settings.Black);
        }

        void DrawDoor(double x, double y)
        {
            int doorWidth = 3 * _windowWidth + 2 * _gap;
            int doorHeight = _windowHeight + _margin - 3;

            Area(x + _settings.ShadowOffset, y + _settings.ShadowOffset, doorWidth, doorHeight, _settings.White, _settings.White);
            Area(x - _settings.ShadowOffset, y - _settings.ShadowOffset, doorWidth, doorHeight, _settings.DarkGray, _settings.Black);
            Area(x, y, doorWidth, doorHeight, _settings.Gray);

            DrawDoorDeco(x, y, doorWidth / 2, doorHeight);
            DrawDoorDeco(x + doorWidth / 2, y, doorWidth / 2, doorHeight);

            var xMiddle = x + doorWidth / 2;
            VerticalLine(xMiddle, y, doorHeight, _settings.DarkGray, 2);

            DrawEntranceText((int)xMiddle + 1, (int)y + 1, _gap, _settings.Gray);
            DrawEntranceText((int)xMiddle - 1, (int)y - 1, _gap, _settings.DarkGray);

            ClickableAreas.Add(new ClickableArea((int)x, (int)y - _gap, doorWidth, doorHeight + _gap, "/GroundFloor"));
        }

        void DrawDoorDeco(double x, double y, int doorWidth, int doorHeight)
        {
            var decoColumns = MakeOdd(_settings.NbrOfDoorDecoColumns);
            var decoColumnWidth = (double)doorWidth / decoColumns;

            var decoRows = MakeOdd(decoColumns * doorHeight / doorWidth);
            var decoRowHeight = (double)(doorHeight - 1) / decoRows;

            int matrixColumns = (decoColumns + 1) / 2;
            int matrixRows = (decoRows + 1) / 2;
            var colourMatrix = colourGenerator.FillMatrixWithColours(_random, matrixColumns, matrixRows, _settings.MixedColours.Length);

            for (int column = 1, cx = 0; column < decoColumns; column += 2, cx++)
            {
                for (int row = 1, ry = 0; row < decoRows; row += 2, ry++)
                {
                    double decoX = x + column * decoColumnWidth;
                    double decoY = y + row * decoRowHeight;
                    Area(decoX, decoY, decoColumnWidth - 1, decoRowHeight - 1, _settings.MixedColours[colourMatrix[cx, ry]]);
                }
            }
        }

        void DrawEntranceText(int x, int y, int gap, string colour) =>
            Svg($@"<text 
                    x='{x}' 
                    y='{y - gap / 2 + 2}' 
                    text-anchor='Middle' 
                    dominant-baseline='Middle' 
                    font-size='{gap * 0.5}' 
                    font-family='sans-serif' 
                    fill='{colour}' 
                    letter-spacing='6'>ENTRANCE</text>");

        int MakeOdd(int value) =>
            (value % 2 == 0) ? value + 1 : value;
    }
}