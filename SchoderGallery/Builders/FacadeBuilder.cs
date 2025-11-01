using SchoderGallery.Navigation;
using SchoderGallery.Painters;
using SchoderGallery.Services;
using SchoderGallery.Settings;

namespace SchoderGallery.Builders;

public class FacadeBuilder(
    SettingsFactory settingsFactory,
    SvgPainter svgPainter,
    NavigationService navigation,
    IGalleryService galleryService,
    Colours colours)
    : BaseBuilder(settingsFactory, svgPainter, navigation, galleryService), IBuilder
{
    public override FloorType FloorType => FloorType.Facade;
    public int Interval => 5000;

    protected override void Draw()
    {
        var schoderText = "Schoder";
        var galleryText = "Gallery";
        var entrance = "Entrance";
        var welcome = "Welcome";

        DrawFrontWall();
        DrawDoorAndWindows();

        void DrawFrontWall() => _svgPainter.Area(0, 0, SvgWidth, SvgHeight, Colours.LightGray);

        void DrawDoorAndWindows()
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
                        var latestFloor = _navigation.GetFloor(FloorType.GroundFloor); // _navigation.GetFloor(Visitor.CurrentFloorType);
                        DrawDoor(lineX, lineY, latestFloor);
                        continue;
                    }

                    if (row != _rowsColumns - 1 || column < _rowsColumns / 2 - 1 || column > _rowsColumns / 2 + 1)
                    {
                        DrawWindowFrames(lineX - 1, lineY - 1, _windowWidth - 2, _windowHeight - 2);
                        DrawWindowGlasses(x, y);
                    }
                }
            }

            _settings.DrawFacadeLetters(_svgPainter, _settings, schoderText, galleryText, SvgWidth, _rowsColumns, _margin, _gap, _windowWidth, _windowHeight, ShortWindowSize);
        }

        void DrawWindowGlasses(int x, int y)
        {
            for (int col = 0; col < _windowGlassColumns; col++)
            {
                for (int row = 0; row < _windowGlassRows; row++)
                {
                    double glassX = x + col * _windowGlassColumnWidth - 0.5;
                    double glassY = y + row * _windowGlassRowHeight - 0.5;
                    _svgPainter.Area(glassX, glassY, _windowGlassColumnWidth - 1, _windowGlassRowHeight - 1, SkyColour());
                }
            }
        }

        void DrawWindowFrames(double x, double y, int width, int height)
        {
            _svgPainter.Sunlight(x, y, width, height, _settings);
            _svgPainter.Area(x, y, width, height, Colours.Gray);
        }

        void DrawDoor(double x, double y, FloorInfo floor)
        {
            int doorWidth = 3 * _windowWidth + 2 * _gap;
            int doorHeight = _windowHeight + _margin - 3;

            _svgPainter.Sunlight(x, y, doorWidth, doorHeight, _settings);
            _svgPainter.Area(x, y, doorWidth, doorHeight, Colours.Gray);

            DrawDoorDeco(x, y, doorWidth / 2, doorHeight, colours.MixedColoursBW);
            DrawDoorDeco(x + doorWidth / 2, y, doorWidth / 2, doorHeight, GetRandomColours());

            var xMiddle = x + doorWidth / 2;
            _svgPainter.VerticalLine(xMiddle, y, doorHeight, Colours.DarkGray, 2);

            DrawEntranceText((int)xMiddle + 1, (int)y + 1, _gap, Colours.White); // Shadow
            DrawEntranceText((int)xMiddle, (int)y, _gap, Colours.Black);

            ClickableAreas.Add(new ClickableArea(0, _height66, SvgWidth, _height33, floor.Page, welcome));
        }

        string[] GetRandomColours() =>
            _random.Next(2) == 0 ? colours.Blueish20Colours : colours.Warm20AccentColours;

        void DrawDoorDeco(double x, double y, int doorWidth, int doorHeight, string[] colourPalette)
        {
            var decoColumns = MakeOdd(_settings.NbrOfDoorDecoColumns);
            var decoColumnWidth = (double)doorWidth / decoColumns;

            var decoRows = MakeOdd(decoColumns * doorHeight / doorWidth);
            var decoRowHeight = (double)(doorHeight - 1) / decoRows;

            int matrixColumns = (decoColumns + 1) / 2;
            int matrixRows = (decoRows + 1) / 2;
            var colourMatrix = colours.FillMatrixWithColours(_random, matrixColumns, matrixRows, colourPalette.Length);

            for (int column = 1, cx = 0; column < decoColumns; column += 2, cx++)
            {
                for (int row = 1, ry = 0; row < decoRows; row += 2, ry++)
                {
                    double decoX = x + column * decoColumnWidth;
                    double decoY = y + row * decoRowHeight;
                    _svgPainter.Area(decoX, decoY, decoColumnWidth - 1, decoRowHeight - 1, colourPalette[colourMatrix[cx, ry]]);
                }
            }
        }

        void DrawEntranceText(int x, int y, int gap, string colour) =>
            Svg($@"<text 
                    x='{x}' 
                    y='{y - gap / 2 + 2}' 
                    text-anchor='middle' 
                    dominant-baseline='middle' 
                    font-size='{gap * 0.75}' 
                    font-family='sans-serif' 
                    fill='{colour}' 
                    letter-spacing='6'>{entrance.ToUpper()}</text>");

        int MakeOdd(int value) =>
            (value % 2 == 0) ? value + 1 : value;

        string SkyColour()
        {
            int gray = _random.Next(245, 256);
            int blue = Math.Min(255, gray + _random.Next(0, 256 - gray));
            return $"#{gray:X2}{gray:X2}{blue:X2}";
        }
    }
}