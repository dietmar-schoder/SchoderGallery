using SchoderGallery.Navigation;
using SchoderGallery.Painters;
using SchoderGallery.Services;
using SchoderGallery.Settings;

namespace SchoderGallery.Builders;

public interface IArtworkBuilder : IBuilder
{
    string GetArtworkHtml(int screenWidth, int screenHeight, int artworkId);
}

public class ArtworkBuilder(
    SettingsFactory settingsFactory,
    SvgPainter svgPainter,
    NavigationService navigation,
    IGalleryService galleryService)
    : BaseBuilder(settingsFactory, svgPainter, navigation), IArtworkBuilder, IBuilder
{
    public override BuilderType Type => BuilderType.Artwork;

    public int Interval => 0;

    public string GetArtworkHtml(int screenWidth, int screenHeight, int artworkId)
    {
        _settings = _settingsFactory.GetSettings(screenWidth, screenHeight);
        SvgWidth = Math.Max(240, screenWidth - _settings.ScreenMargin * 2);
        SvgHeight = Math.Max(240, screenHeight - _settings.ScreenMargin * 2);
        _svg.Clear();
        ClickableAreas.Clear();

        _rowsColumns = _settings.RowsColumns;
        _gap = (int)(ShortSize / _rowsColumns * _settings.GapToRowColumnWidthRatio);

        var widthHalf = SvgWidth / 2;
        var height3rd = SvgHeight / 3;

        var floor = _navigation.GetVisitorFloor();
        var floorNumber = (int)floor.FloorType;

        var artwork = galleryService.GetArtworkAsync(floorNumber, artworkId);

        ClickableAreas.Add(new ClickableArea(0, 0, widthHalf - 2, height3rd - 2, floor.PageAndParam()));

        if (artwork.PreviousId > -1)
        {
            ClickableAreas.Add(new ClickableArea(0, height3rd + 2, widthHalf - 2, height3rd - 2, $"/Artwork/{artwork.PreviousId}"));
        }

        if (artwork.NextId > -1)
        {
            ClickableAreas.Add(new ClickableArea(widthHalf + 2, height3rd + 2, widthHalf - 2, height3rd - 2, $"/Artwork/{artwork.NextId}"));
        }

        _svg.Text(widthHalf, height3rd, artwork.Title, _gap, _settings.Black, 0);

        return _svg.SvgContent();
    }
}