using SchoderGallery.Navigation;
using SchoderGallery.Painters;
using SchoderGallery.Settings;

namespace SchoderGallery.Builders;

public abstract class BaseFloorBuilder(
    SettingsFactory settingsFactory,
    SvgPainter svgPainter,
    NavigationService navigation)
    : BaseBuilder(settingsFactory, svgPainter, navigation)
{
    protected override void Draw()
    {
        var wall = _settings.WallThickness;
        var floor = _navigation.GetFloor(Type);

        DrawOuterWalls();

        if (Type == BuilderType.Atelier)
        {
        }
        else if (Type == BuilderType.GroundFloor)
        {
            DrawWindowsAndDoor();
        }
        else if (Type > 0)
        {
            DrawWindows();
        }

        if (floor.IsArtworksFloor)
        {
            // DrawExhibitionInfo();
            DrawArtworksLink();
        }

        DrawFloorCaption();

        DrawLiftLink();

        void DrawOuterWalls()
        {
            _svgPainter.Area(0, 0, SvgWidth, SvgHeight, _settings.LightGray, _settings.Black);
            _svgPainter.Area(wall, wall, SvgWidth - 2 * wall, SvgHeight - 2 * wall, _settings.White, _settings.DarkGray);
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

        void DrawWindows()
        {
            for (int column = 0; column < _rowsColumns; column++)
            {
                int x = _margin + column * (_windowWidth + _gap);
                DrawWindow(x, SvgHeight - wall);
            }
        }

        void DrawDoor(double x, double y)
        {
            int doorWidth = 3 * _windowWidth + 2 * _gap;

            _svgPainter.Area(x, y - 1, doorWidth, wall + 1, _settings.Gray, _settings.Black);

            var xMiddle = x + doorWidth / 2;
            _svgPainter.VerticalLine(xMiddle, y, wall, _settings.DarkGray, 2);
        }

        void DrawWindow(int x, int y) =>
            _svgPainter.Area(x, y - 1, _windowWidth, wall + 1, _settings.White, _settings.Black);

        void DrawFloorCaption() =>
            _svgPainter.Text(SvgWidth / 2, SvgHeight / 2, floor.LiftLabel, _gap * 2, _settings.LightGray);

        void DrawLiftLink()
        {
            int doorWidth = 3 * _windowWidth + 2 * _gap;
            var x = (SvgWidth - (3 * _windowWidth + 2 * _gap)) / 2;
            var xMiddle = x + doorWidth / 2;

            ClickableAreas.Add(new ClickableArea(x, 0, doorWidth, wall + _gap * 4, "/Lift"));
            _svgPainter.TextLink(xMiddle, wall + _gap * 2, "LIFT", (int)(_gap * _settings.LinkFontSizeToGapRatio), _settings);
        }

        void DrawArtworksLink()
        {
            var widthHalf = SvgWidth / 2;
            var heightHalf = SvgHeight / 2;

            ClickableAreas.Add(new ClickableArea(0, heightHalf, SvgWidth, heightHalf, $"/Artwork/0"));
            _svgPainter.TextLink(widthHalf, SvgHeight * 2 / 3, "ARTWORKS", (int)(_gap * _settings.LinkFontSizeToGapRatio), _settings);
        }
    }
}