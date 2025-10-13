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
        var exhibition = floor.IsArtworksFloor ? _galleryService.GetExhibition(floor.FloorNumber) : null;

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

            if (exhibition is not null && exhibition.Artworks.Count > 0)
            {
                DrawExhibitionInfoAndArtworksLink(exhibition);
                HangArtworks(exhibition.Artworks, SvgWidth - 2* wall, SvgHeight - 2 * wall, wall);
                DrawArtworks(exhibition.Artworks);
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

        DrawVisitor(exhibition);

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

        void DrawArtworks(List<ArtworkDto> artworks)
        {
            foreach (var artwork in artworks)
            {
                _svgPainter.Area(artwork.WallX, artwork.WallY, 4, artwork.WallWidth, Colours.White, Colours.Black);
            }
        }

        void DrawFloorCaption() =>
            _svgPainter.Text(_width50, _height66, floor.LiftLabel, _largeFontSize * 2, Colours.LightGray);

        void DrawLiftLink()
        {
            ClickableAreas.Add(new ClickableArea(_width33 + 2, 0, _width33 - 4, _height25 - 2, "/Lift", "Enter lift"));
            _svgPainter.TextLink(_width50, wall + _largeFontSize * 2, "LIFT", _fontSize);
        }

        void DrawExhibitionInfoAndArtworksLink(ExhibitionDto exhibition)
        {
            var gap125 = (int)(_largeFontSize * 1.25);

            _svgPainter.Text(_width50, _height66 - gap125, floor.LiftLabel, _largeFontSize * 2, Colours.LightGray);
            _svgPainter.Text(_width50, _height66 + gap125, exhibition.LiftLabel, _largeFontSize * 2, exhibition.LabelColour);
            _svgPainter.TextLink(_width50, _height33, "ARTWORKS", _fontSize);

            ClickableAreas.Add(new ClickableArea(0, _height25 + 2, SvgWidth, SvgHeight - _height25 - 2, $"/Artwork/0", "Look at artworks"));
        }

        void DrawVisitor(ExhibitionDto exhibition)
        {
            var visitorX = _width50;
            var visitorY = _height50;

            if (exhibition is not null && exhibition.Artworks.Count > 0)
            {
                if (navigation.GetLatestFloorArtwork(exhibition) is { } artwork)
                {
                    visitorX = artwork.IsLeftWall ? _width20 : _width80;
                    visitorY = artwork.WallY + artwork.WallWidth / 2;
                }
            }

            _svgPainter.Circle(visitorX - _fontSize / 2, visitorY - _fontSize / 2, _fontSize, Colours.Blue, 1, Colours.DeepBlue);
        }
    }

    private static List<ArtworkDto> HangArtworks(List<ArtworkDto> artworks, int innerRoomWidth, int innerRoomHeight, int wall)
    {
        int artworkGap = 4;
        int artworkThickness = 4;
        int rightCount = (artworks.Count + 1) / 2;
        int leftCount = artworks.Count - rightCount;
        double rightArtWorkSpace = (innerRoomHeight - artworkGap) / (double)rightCount;
        double leftArtWorkSpace = (innerRoomHeight - artworkGap) / (double)leftCount;

        for (int i = 0; i < rightCount; i++)
        {
            var artwork = artworks[i];
            artwork.IsRightWall = true;
            artwork.WallX = wall + innerRoomWidth - artworkThickness - artworkGap;
            artwork.WallY = wall + artworkGap + (int)(i * rightArtWorkSpace);
            artwork.WallWidth = (int)rightArtWorkSpace - 4;
        }

        for (int i = 0; i < leftCount; i++)
        {
            var artwork = artworks[rightCount + i];
            artwork.IsRightWall = false;
            artwork.WallX = wall + artworkGap;
            artwork.WallY = wall + artworkGap + (int)((leftCount - 1 - i) * leftArtWorkSpace);
            artwork.WallWidth = (int)leftArtWorkSpace - 4;
        }

        return artworks;
    }
}