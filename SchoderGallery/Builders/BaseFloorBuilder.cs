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
    private readonly Dictionary<FloorType, string> _groundFloorRoomActions = new()
    {
        { FloorType.Cafe, "MENU" },
        { FloorType.Shop, "PRODUCTS" },
        { FloorType.Toilets, "SHIT" },
        { FloorType.Info, "MAKE MONEY" }
    };

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
                if (floor.FloorType != FloorType.MyCollection)
                {
                    DrawSoldDots(exhibition.Artworks);
                }
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
            var doorsize = (int)(_windowHeight * 1.5);
            DrawRoomDoor(_height50 - doorsize / 2, doorsize, floor);
            DrawExitToGroundFloorLink(floor);
            // draw two windows
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
                    DrawMainDoor((SvgWidth - (3 * _windowWidth + 2 * _gap)) / 2, SvgHeight - wall);
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

        void DrawRoomDoor(int y, int width, FloorInfo floor)
        {
            var x = floor.IsLeftGroundFloorRoom ? SvgWidth - wall : 0;
            _svgPainter.Area(x, y, wall, width, Colours.Background, Colours.Black);
            _svgPainter.Area(x - 1, y + 1, wall + 2, width - 2, Colours.Background, Colours.Background);
        }

        void DrawMainDoor(double x, double y)
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
                _svgPainter.Area(artwork.WallX, artwork.WallY, artwork.WidthOnWall, artwork.WidthOnWall, Colours.White, Colours.Black);

                var thumbnailFileName = artwork.SizeType == SizeType.Text
                    ? $"images/info-thumbnail2.png"
                    : $"images/floor{artwork.FloorNumber}/{artwork.Number:D6}.jpg";
                _svgPainter.Thumbnail(artwork.WallX + 1, artwork.WallY + 1, artwork.WidthOnWall - 2, artwork.WidthOnWall - 2, thumbnailFileName);

                var tooltip = artwork.SizeType == SizeType.Text ? "Information" : artwork.Title; // Shorter tooltip?????

                if (artwork.IsLeftWall)
                {
                    ClickableAreas.Add(new ClickableArea(0, artwork.WallY, _width20, artwork.WidthOnWall + 4, $"/Artwork/{artwork.Id}", tooltip));
                }
                else
                {
                    ClickableAreas.Add(new ClickableArea(_width80, artwork.WallY, _width20, artwork.WidthOnWall + 4, $"/Artwork/{artwork.Id}", tooltip));
                }
            }
        }

        void DrawSoldDots(List<ArtworkDto> artworks)
        {
            foreach (var artwork in artworks.Where(a => a.HasOwner))
            {
                if (artwork.IsLeftWall)
                {
                    _svgPainter.Circle(artwork.WallX + artwork.WidthOnWall + 4, artwork.WallY + 2, 6, Colours.WarmAccentOrange, 1, Colours.WarmAccentRed);
                }
                else
                {
                    _svgPainter.Circle(artwork.WallX - 10, artwork.WallY + 2, 6, Colours.WarmAccentOrange, 1, Colours.WarmAccentRed);
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

        void DrawExitToGroundFloorLink(FloorInfo floor)
        {
            if (floor.FloorType == FloorType.Shop
                || floor.FloorType == FloorType.Info)
            {
                ClickableAreas.Add(new ClickableArea(0, _height33 + 2, _width33 - 2, _height33 - 4, "/GroundFloor", "Back to ground floor"));
                _svgPainter.TextLink(wall + _largeFontSize * 3, _height50, "EXIT", _fontSize * 3 / 2);
            }
            else
            {
                ClickableAreas.Add(new ClickableArea(_width66 + 2, _height33 + 2, _width33 - 2, _height33 - 4, "/GroundFloor", "Back to ground floor"));
                _svgPainter.TextLink(SvgWidth - wall - _largeFontSize * 3, _height50, "EXIT", _fontSize * 3 / 2);
            }
        }

        void DrawExhibitionInfoAndArtworksLink(ExhibitionDto exhibition)
        {
            var gap125 = (int)(_largeFontSize * 1.25);

            _svgPainter.Text(_width50, _height66 - gap125, floor.LiftLabel, _largeFontSize * 2, Colours.LightGray);
            if (!string.IsNullOrEmpty(exhibition.Title))
            {
                _svgPainter.Text(_width50, _height66 + gap125, exhibition.Title, _largeFontSize * 2, exhibition.Colour);
            }

            if (floor.IsGroundFloorRoom)
            {
                _svgPainter.TextLink(_width50, _height33, _groundFloorRoomActions[floor.FloorType], _fontSize * 3 / 2);
            }
            else
            {
                _svgPainter.TextLink(_width50, _height33, "ARTWORKS", _fontSize * 3 / 2);
            }

            ClickableAreas.Add(new ClickableArea(0, _height25 + 2, SvgWidth, SvgHeight - _height25 - 2, $"/Artwork/{Guid.Empty}", "Look at artworks"));
        }

        void DrawVisitor(ExhibitionDto exhibition)
        {
            var visitorX = _width50;
            var visitorY = _height50 - 75;

            if (exhibition is not null && exhibition.Artworks.Count > 0)
            {
                if (_navigation.GetLatestFloorArtwork(FloorType, exhibition) is { } artwork)
                {
                    var distance = artwork.WidthOnWall + _gap * 2;
                    visitorX = artwork.IsLeftWall ? distance : SvgWidth - distance;
                    visitorY = artwork.WallY + artwork.WidthOnWall / 2;
                }
            }

            _svgPainter.Circle(visitorX - _fontSize / 2, visitorY - _fontSize / 2, _fontSize, Colours.Blue, 1, Colours.DeepBlue);
        }

        void HangArtworks(List<ArtworkDto> artworks, int innerRoomWidth, int innerRoomHeight, int wall)
        {
            if (floor.IsLiftFloor)
            {
                var (rightArtworks, leftArtworks) = SplitArtworks(artworks);
                HangRightWallArtworks(rightArtworks, innerRoomWidth, innerRoomHeight, wall);
                HangLeftWallArtworks(leftArtworks, innerRoomWidth, innerRoomHeight, wall);
            }
            else if (floor.IsLeftGroundFloorRoom)
            {
                HangLeftWallArtworks(artworks, innerRoomWidth, innerRoomHeight, wall);
            }
            else if (floor.IsRightGroundFloorRoom)
            {
                HangRightWallArtworks(artworks, innerRoomWidth, innerRoomHeight, wall);
            }
        }
    }

    private static (List<ArtworkDto> rightArtworks, List<ArtworkDto> leftArtworks) SplitArtworks(List<ArtworkDto> artworks)
    {
        var rightCount = (artworks.Count + 1) / 2;
        var rightArtworks = artworks.Take(rightCount).ToList();
        var leftArtworks = artworks.Skip(rightCount).ToList();

        return (rightArtworks, leftArtworks);
    }

    private static void HangRightWallArtworks(List<ArtworkDto> rightArtworks, int innerRoomWidth, int innerRoomHeight, int wall)
    {
        var artworkGap = 4;
        double rightArtWorkSpace = Math.Min((innerRoomHeight - artworkGap) / (double)rightArtworks.Count, 100);

        for (int i = 0; i < rightArtworks.Count; i++)
        {
            var artwork = rightArtworks[i];
            artwork.IsRightWall = true;
            artwork.WidthOnWall = (int)rightArtWorkSpace - 4;
            artwork.WallX = wall + innerRoomWidth - artwork.WidthOnWall - artworkGap;
            artwork.WallY = wall + artworkGap + (int)(i * rightArtWorkSpace);
        }
    }

    private static void HangLeftWallArtworks(List<ArtworkDto> leftArtworks, int innerRoomWidth, int innerRoomHeight, int wall)
    {
        var artworkGap = 4;
        double leftArtWorkSpace = Math.Min((innerRoomHeight - artworkGap) / (double)leftArtworks.Count, 100);

        for (int i = 0; i < leftArtworks.Count; i++)
        {
            var artwork = leftArtworks[i];
            artwork.IsRightWall = false;
            artwork.WidthOnWall = (int)leftArtWorkSpace - 4;
            artwork.WallX = wall + artworkGap;
            artwork.WallY = wall + artworkGap + (int)((leftArtworks.Count - 1 - i) * leftArtWorkSpace);
        }
    }
}
