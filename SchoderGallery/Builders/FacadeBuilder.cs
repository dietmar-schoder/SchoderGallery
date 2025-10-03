using SchoderGallery.Algorithms;
using SchoderGallery.Settings;

namespace SchoderGallery.Builders;

public class FacadeBuilder(SettingsFactory settingsFactory, ColourGenerator colourGenerator)
    : BaseBuilder(settingsFactory), IBuilder
{
    public override BuilderType Type => BuilderType.Facade;

    protected override void Draw()
    {
        ClickableAreas.Clear();
        DrawFrontWall();
        DrawWindowsAndDoor();

        void DrawFrontWall() =>
            Svg($"<rect x='0' y='0' width='{SvgWidth}' height='{SvgHeight}' fill='{_settings.LightGray}' stroke='none' />");

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

            _settings.DrawFacadeLetters(_svg, _settings, SvgWidth, _rowsColumns, _margin, _gap, _windowWidth, _windowHeight, ShortWindowSize);
        }

        void DrawWindowGlasses(int x, int y, int width, int height)
        {
            Svg($"<rect x='{x}' y='{y}' width='{width}' height='{height}' fill='{_settings.White}' stroke='none' />");

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

        void DrawWindowFrames1(double x, double y, int width, int height) =>
            Svg($"<rect x='{x - _settings.ShadowOffset}' y='{y - _settings.ShadowOffset}'" +
                $" width='{width}' height='{height}'" +
                $" fill='{_settings.DarkGray}' stroke='{_settings.Black}' stroke-width='1' />");

        void DrawWindowFrames2(double x, double y, int width, int height)
        {
            for (int i = 1; i < _windowGlassColumns; i++)
            {
                double xPos = x + i * _windowGlassColumnWidth;
                Svg($"<line x1='{xPos}' y1='{y}' x2='{xPos}' y2='{y + height}' stroke='{_settings.DarkGray}' stroke-width='1' />");
            }

            for (int j = 1; j < _windowGlassRows; j++)
            {
                double yPos = y + j * _windowGlassRowHeight;
                Svg($"<line x1='{x}' y1='{yPos}' x2='{x + width}' y2='{yPos}' stroke='{_settings.DarkGray}' stroke-width='1' />");
            }

            Svg($"<rect x='{x}' y='{y}' width='{width}' height='{height}' fill='none' stroke='{_settings.Gray}' stroke-width='1' />");
        }

        void DrawDoor(double x, double y)
        {
            int doorWidth = 3 * _windowWidth + 2 * _gap;
            int doorHeight = _windowHeight + _margin + 1;

            Svg($"<rect x='{x - _settings.ShadowOffset}' y='{y - _settings.ShadowOffset}'" +
                $" width='{doorWidth}' height='{doorHeight}' " +
                $"fill='{_settings.DarkGray}' stroke='{_settings.Black}' stroke-width='1' />");
            Svg($"<rect x='{x}' y='{y}' width='{doorWidth}' height='{doorHeight}' " +
                $"fill='{_settings.Gray}' stroke='{_settings.DarkGray}' stroke-width='1' />");

            DrawDoorDeco(x, y, doorWidth / 2, doorHeight);
            DrawDoorDeco(x + doorWidth / 2, y, doorWidth / 2, doorHeight);

            var xMiddle = x + doorWidth / 2;
            Svg($"<line x1='{xMiddle}' y1='{y}' x2='{xMiddle}' y2='{y + doorHeight}' stroke='{_settings.DarkGray}' stroke-width='1' />");

            DrawEntranceText((int)x + 1, (int)y + 1, doorWidth, doorHeight, _settings.DarkGray, showBackground: true);
            DrawEntranceText((int)x - 1, (int)y - 1, doorWidth, doorHeight, _settings.White);

            ClickableAreas.Add(new ClickableArea((int)x, (int)y, doorWidth, doorHeight, "/GroundFloor"));
        }

        void DrawDoorDeco(double x, double y, int doorWidth, int doorHeight)
        {
            var decoColumns = MakeOdd(_settings.NbrOfDoorDecoColumns);
            var decoColumnWidth = (double)doorWidth / decoColumns;

            var decoRows = MakeOdd(decoColumns * doorHeight / doorWidth);
            var decoRowHeight = (double)(doorHeight - 1) / decoRows;

            int matrixColumns = (decoColumns + 1) / 2;
            int matrixRows = (decoRows + 1) / 2;
            var colourMatrix = ColourGenerator.FillMatrixWithColours(_random, matrixColumns, matrixRows, _settings.MixedColours.Length);

            for (int column = 1, cx = 0; column < decoColumns; column += 2, cx++)
            {
                for (int row = 1, ry = 0; row < decoRows; row += 2, ry++)
                {
                    double decoX = x + column * decoColumnWidth;
                    double decoY = y + row * decoRowHeight;
                    Svg($"<rect x='{decoX}' y='{decoY}'" +
                        $" width='{decoColumnWidth - 1}' height='{decoRowHeight - 1}'" +
                        $" fill='{_settings.MixedColours[colourMatrix[cx, ry]]}' stroke='none' />");
                }
            }
        }

        void DrawEntranceText(int x, int y, int w, int h, string colour, bool showBackground = false)
        {
            string Label = "WELCOME";
            double fontSize = h * 0.2;
            double textWidth = fontSize * 1 * Label.Length;
            double padding = fontSize * 0.3;
            double rectX = x + w / 2 - textWidth / 2 - padding;
            double rectY = y + h / 2 - fontSize * 0.6 - padding / 2;
            double rectWidth = textWidth + padding * 2;
            double rectHeight = fontSize * 1.2 + padding;

            if (showBackground)
            {
                Svg($@"
                    <rect 
                        x='{rectX}' 
                        y='{rectY}' 
                        width='{rectWidth}' 
                        height='{rectHeight}' 
                        fill='{_settings.LinkBackground}' 
                        opacity='0.5' />");
            }
            Svg($@"
                <text 
                    x='{x + w / 2}' 
                    y='{y + h / 2}' 
                    text-anchor='middle' 
                    dominant-baseline='central' 
                    font-size='{fontSize}' 
                    font-family='sans-serif' 
                    fill='{colour}' 
                    letter-spacing='6'>{Label}</text>
            ");
        }

        int MakeOdd(int value) =>
            (value % 2 == 0) ? value + 1 : value;
    }
}