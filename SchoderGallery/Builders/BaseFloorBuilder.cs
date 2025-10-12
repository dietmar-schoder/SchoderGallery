using SchoderGallery.DTOs;
using SchoderGallery.Navigation;
using SchoderGallery.Painters;
using SchoderGallery.Services;
using SchoderGallery.Settings;

namespace SchoderGallery.Builders;

public abstract class BaseFloorBuilder(
    SettingsFactory settingsFactory,
    SvgPainter svgPainter,
    NavigationService navigation,
    IGalleryService galleryService)
    : BaseBuilder(settingsFactory, svgPainter, navigation, galleryService)
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
        else if (Type > 0) // Only for floors above ground floor
        {
            DrawWindows();
        }

        if (floor.IsArtworksFloor)
        {

            var exhibition = _galleryService.GetExhibition(floor.FloorNumber);
            if (exhibition is not null && exhibition.Artworks.Count > 0)
            {
                DrawExhibitionInfoAndArtworksLink(exhibition);
            }
            else
            {
                DrawFloorCaption();
            }
        }
        else
        {
            DrawFloorCaption();
        }

        DrawLiftLink();

        void DrawOuterWalls()
        {
            _svgPainter.Area(0, 0, SvgWidth, SvgHeight, Colours.LightGray, Colours.Black);
            _svgPainter.Area(wall, wall, SvgWidth - 2 * wall, SvgHeight - 2 * wall, Colours.White, Colours.DarkGray);
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

            _svgPainter.Area(x, y - 1, doorWidth, wall + 1, Colours.Gray, Colours.Black);

            _svgPainter.VerticalLine(_width50, y, wall, Colours.DarkGray, 2);
        }

        void DrawWindow(int x, int y) =>
            _svgPainter.Area(x, y - 1, _windowWidth, wall + 1, Colours.White, Colours.Black);

        void DrawFloorCaption() =>
            _svgPainter.Text(SvgWidth / 2, SvgHeight / 2, floor.LiftLabel, _largeFontSize * 2, Colours.LightGray);

        void DrawLiftLink()
        {
            int doorWidth = 3 * _windowWidth + 2 * _gap;
            var x = (SvgWidth - (3 * _windowWidth + 2 * _gap)) / 2;

            ClickableAreas.Add(new ClickableArea(x, 0, doorWidth, wall + _largeFontSize * 4, "/Lift"));
            _svgPainter.TextLink(_width50, wall + _largeFontSize * 2, "LIFT", _fontSize);
        }

        void DrawExhibitionInfoAndArtworksLink(ExhibitionDto exhibition)
        {
            var gap125 = (int)(_largeFontSize * 1.25);

            _svgPainter.Text(_width50, _height66 - gap125, floor.LiftLabel, _largeFontSize * 2, Colours.LightGray);
            _svgPainter.Text(_width50, _height66 + gap125, exhibition.LiftLabel, _largeFontSize * 2, exhibition.LabelColour);
            _svgPainter.TextLink(_width50, _height33, "ARTWORKS", _fontSize);

            ClickableAreas.Add(new ClickableArea(0, _height25, SvgWidth, _height50, $"/Artwork/0"));
        }
    }
}