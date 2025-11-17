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
    GalleryService galleryService)
    : BaseBuilder(settingsFactory, svgPainter, navigation, galleryService)
{
    protected override async Task DrawAsync()
    {
        var wall = _settings.WallThickness;
        var floor = _navigation.GetFloor(FloorType);
        var exhibition = floor.IsArtworksFloor
            ? await _galleryService.GetExhibitionArtworksAsync(Visitor, floor.FloorNumber)
            : null;

        await _navigation.SetVisitorFloorAsync(FloorType);

        DrawOuterWalls();
        DrawFloorPattern();

        if (FloorType == FloorType.Atelier || floor.IsGroundFloorRoom)
        {
        }
        else if (FloorType == FloorType.GroundFloor)
        {
            DrawWindowsAndDoor();
        }
        else if (FloorType > 0)
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

        if (floor.IsGroundFloorRoom)
        {
            DrawExitToGroundFloorLink();
        }
        else
        {
            DrawLiftLink();
        }

        DrawVisitor(exhibition);

        void DrawOuterWalls()
        {
            _svgPainter.Area(0, 0, SvgWidth, SvgHeight, Colours.LightGray, Colours.Black);
            _svgPainter.Area(wall, wall, SvgWidth - 2 * wall, SvgHeight - 2 * wall, Colours.Background, Colours.DarkGray);
        }

        void DrawFloorPattern()
        {
            if (FloorType == FloorType.Atelier)
            {
                _svgPainter.FloorPattern3(wall + 4, wall + 4, SvgWidth - 2 * wall - 8, SvgHeight - 2 * wall - 8, _gap * 2);
            }
            else if (FloorType < 0)
            {
                _svgPainter.FloorPattern2(wall + 4, wall + 4, SvgWidth - 2 * wall - 8, SvgHeight - 2 * wall - 8, _gap * 2);
            }
            else
            {
                _svgPainter.FloorPattern1(wall + 4, wall + 4, SvgWidth - 2 * wall - 8, SvgHeight - 2 * wall - 8, _gap * 2);
            }
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
                _svgPainter.Area(artwork.WallX, artwork.WallY, artwork.ThumbnailSize, artwork.WallWidth, Colours.White, Colours.Black);
                var thumbnailFileName = $"images/floor{floor.FloorNumber}/{artwork.Number:D6}.jpg";

                _svgPainter.Thumbnail(artwork.WallX + 1, artwork.WallY + 1, artwork.ThumbnailSize - 2, artwork.WallWidth - 2, thumbnailFileName);
                
                var tooltip = artwork.SizeType == SizeType.Text ? "Info" : artwork.Title; // Shorter tooltip?????

                if (artwork.IsLeftWall)
                {
                    ClickableAreas.Add(new ClickableArea(0, artwork.WallY, _width20, artwork.WallWidth + 4, $"/Artwork/{artwork.Number}", tooltip));
                }
                else
                {
                    ClickableAreas.Add(new ClickableArea(_width80, artwork.WallY, _width20, artwork.WallWidth + 4, $"/Artwork/{artwork.Number}", tooltip));
                }
            }
        }

        void DrawFloorCaption() =>
            _svgPainter.Text(_width50, _height50, floor.LiftLabel, _largeFontSize * 2, Colours.LightGray);

        void DrawLiftLink()
        {
            ClickableAreas.Add(new ClickableArea(_width33 + 2, 0, _width33 - 4, _height25 - 2, "/Lift", "Enter lift"));
            _svgPainter.TextLink(_width50, wall + _largeFontSize * 2, "LIFT", _fontSize * 3 / 2);
        }

        void DrawExitToGroundFloorLink()
        {
            ClickableAreas.Add(new ClickableArea(_width33 + 2, 0, _width33 - 4, _height25 - 2, "/GroundFloor", "Back to ground floor"));
            _svgPainter.TextLink(_width50, wall + _largeFontSize * 2, "EXIT", _fontSize * 3 / 2);
        }

        void DrawExhibitionInfoAndArtworksLink(ExhibitionDto exhibition)
        {
            var gap125 = (int)(_largeFontSize * 1.25);

            _svgPainter.Text(_width50, _height66 - gap125, floor.LiftLabel, _largeFontSize * 2, Colours.LightGray);
            _svgPainter.Text(_width50, _height66 + gap125, exhibition.Title, _largeFontSize * 2, exhibition.Colour);
            _svgPainter.TextLink(_width50, _height33, "ARTWORKS", _fontSize * 3 / 2);

            ClickableAreas.Add(new ClickableArea(0, _height25 + 2, SvgWidth, SvgHeight - _height25 - 2, $"/Artwork/0", "Look at artworks"));
        }

        void DrawVisitor(ExhibitionDto exhibition)
        {
            var visitorX = _width50;
            var visitorY = _height50 - 75;

            if (exhibition is not null && exhibition.Artworks.Count > 0)
            {
                if (_navigation.GetLatestFloorArtwork(FloorType, exhibition) is { } artwork)
                {
                    var distance = artwork.ThumbnailSize + _gap * 2;
                    visitorX = artwork.IsLeftWall ? distance : SvgWidth - distance;
                    visitorY = artwork.WallY + artwork.WallWidth / 2;
                }
            }

            _svgPainter.Circle(visitorX - _fontSize / 2, visitorY - _fontSize / 2, _fontSize, Colours.Blue, 1, Colours.DeepBlue);
        }
    }

    private static List<ArtworkDto> HangArtworks(List<ArtworkDto> artworks, int innerRoomWidth, int innerRoomHeight, int wall)
    {
        var artworkGap = 4;

        var rightCount = (artworks.Count + 1) / 2;
        double rightArtWorkSpace = Math.Min((innerRoomHeight - artworkGap) / (double)rightCount, 100);
        for (int i = 0; i < rightCount; i++)
        {
            var artwork = artworks[i];
            artwork.IsRightWall = true;
            artwork.WallWidth = (int)rightArtWorkSpace - 4;
            artwork.ThumbnailSize = ThumbnailSize(artwork.SizeType, artwork.WallWidth);
            artwork.WallX = wall + innerRoomWidth - artwork.ThumbnailSize - artworkGap;
            artwork.WallY = wall + artworkGap + (int)(i * rightArtWorkSpace);
        }

        var leftCount = artworks.Count - rightCount;
        double leftArtWorkSpace = Math.Min((innerRoomHeight - artworkGap) / (double)leftCount, 100);
        for (int i = 0; i < leftCount; i++)
        {
            var artwork = artworks[rightCount + i];
            artwork.IsRightWall = false;
            artwork.WallWidth = (int)leftArtWorkSpace - 4;
            artwork.ThumbnailSize = ThumbnailSize(artwork.SizeType, artwork.WallWidth);
            artwork.WallX = wall + artworkGap;
            artwork.WallY = wall + artworkGap + (int)((leftCount - 1 - i) * leftArtWorkSpace);
        }

        return artworks;

        static int ThumbnailSize(SizeType sizeType, int wallWidth) =>
            sizeType == SizeType.Fixed || sizeType == SizeType.PortraitLandscape ? wallWidth : 4;
    }
}
