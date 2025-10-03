using SchoderGallery.Algorithms;
using SchoderGallery.Settings;

namespace SchoderGallery.Builders;

public class GroundFloorBuilder(SettingsFactory settingsFactory, ColourGenerator colourGenerator)
    : BaseBuilder(settingsFactory), IBuilder
{
    public BuilderType Type => BuilderType.GroundFloor;
    public int Interval => 0;

    protected override void Draw()
    {
        var wall = _settings.WallThickness;

        ClickableAreas.Clear();
        DrawOuterWalls();
        DrawWindowsAndDoor();

        void DrawOuterWalls()
        {
            Area(0, 0, SvgWidth, SvgHeight, _settings.LightGray, _settings.Black);
            Area(wall, wall, SvgWidth - 2 * wall, SvgHeight - 2 * wall, _settings.White, _settings.DarkGray);
        }

        void DrawWindowsAndDoor()
        {
            for (int column = 0; column < _rowsColumns; column++)
            {
                int x = _margin + column * (_windowWidth + _gap);
                if (column == _rowsColumns / 2 - 1)
                {
                    DrawDoor((SvgWidth - (3 * _windowWidth + 2 * _gap)) / 2, SvgHeight - wall);
                    continue;
                }

                if (column < _rowsColumns / 2 - 1 || column > _rowsColumns / 2 + 1)
                {
                    DrawWindow(x, SvgHeight - wall);
                }
            }
        }

        void DrawDoor(double x, double y)
        {
            int doorWidth = 3 * _windowWidth + 2 * _gap;

            Area(x, y - 1, doorWidth, wall + 1, _settings.Gray, _settings.Black);

            var xMiddle = x + doorWidth / 2;
            VerticalLine(xMiddle, y, wall, _settings.DarkGray, 2);

            DrawExitText((int)xMiddle + 1, (int)y + 1 - _gap / 2, _gap, _settings.Gray);
            DrawExitText((int)xMiddle - 1, (int)y - 1 - _gap / 2, _gap, _settings.DarkGray);

            ClickableAreas.Add(new ClickableArea((int)x, (int)y - _gap * 2, doorWidth, wall + _gap * 2, "/"));
        }

        void DrawWindow(int x, int y) =>
            Area(x, y - 1, _windowWidth, wall + 1, _settings.White, _settings.Black);

        void DrawExitText(int x, int y, int gap, string colour) =>
            Svg($@"<text 
                    x='{x}' 
                    y='{y - gap / 2 + 2}' 
                    text-anchor='Middle' 
                    dominant-baseline='Middle' 
                    font-size='{gap * 0.5}' 
                    font-family='sans-serif' 
                    fill='{colour}' 
                    letter-spacing='6'>EXIT</text>");
    }
}