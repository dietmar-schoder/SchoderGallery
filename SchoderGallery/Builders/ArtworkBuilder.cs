using SchoderGallery.Helpers;
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
    IGalleryService galleryService,
    SizeHelperFactory sizeHelperFactory)
    : BaseBuilder(settingsFactory, svgPainter, navigation), IArtworkBuilder, IBuilder
{
    public override BuilderType Type => BuilderType.Artwork;

    public int Interval => 0;

    public string GetArtworkHtml(int screenWidth, int screenHeight, int artworkId)
    {
        _svgPainter.Clear();
        _settings = _settingsFactory.GetSettings(screenWidth, screenHeight);
        ClickableAreas.Clear();

        var floor = _navigation.GetVisitorFloor();
        var artwork = galleryService.GetArtworkAsync(_settings, floor.FloorNumber, artworkId);

        var sizeHelper = sizeHelperFactory.GetHelper(artwork.SizeType);
        (int artworkWidth, int artworkHeight) = sizeHelper.GetArtworkSize(artwork, screenWidth, screenHeight);


        //var artworkWidth = SvgWidth - margin * 2;
        //var artworkHeight = SvgHeight - margin * 2;
        SvgWidth = Math.Max(240, screenWidth - _settings.ScreenMargin * 2);
        SvgHeight = Math.Max(240, screenHeight - _settings.ScreenMargin * 2);
        _rowsColumns = _settings.RowsColumns;
        _gap = (int)(ShortSize / _rowsColumns * _settings.GapToRowColumnWidthRatio);
        var margin = _gap / 2 + 4;
        var halfMargin = margin / 2;
        var widthHalf = SvgWidth / 2;
        var height3rd = SvgHeight / 3;





        // Back to floor (left top)
        _svgPainter.IconLeftArrow(0, 0, margin, _settings);
        ClickableAreas.Add(new ClickableArea(0, 0, widthHalf - 2, height3rd - 2, floor.PageAndParam(), "Back"));

        // Comments (right top)

        // Previous artwork
        if (artwork.PreviousId > -1)
        {
            _svgPainter.IconLeft(-2, SvgHeight / 2 - margin / 2, margin, _settings);
            ClickableAreas.Add(new ClickableArea(0, height3rd + 2, widthHalf - 2, height3rd - 2, $"/Artwork/{artwork.PreviousId}", "Previous artwork"));
        }

        // Next artwork
        if (artwork.NextId > -1)
        {
            _svgPainter.IconRight(2 + SvgWidth - margin, SvgHeight / 2 - margin / 2, margin, _settings);
            ClickableAreas.Add(new ClickableArea(widthHalf + 2, height3rd + 2, widthHalf - 2, height3rd - 2, $"/Artwork/{artwork.NextId}", "Next artwork"));
        }

        // Refresh (left bottom)
        _svgPainter.IconRefresh(0, SvgHeight - margin, margin, _settings);
        ClickableAreas.Add(new ClickableArea(0, height3rd * 2 + 2, widthHalf - 2, height3rd - 2, ReRender: true));

        // Buy (right bottom)

        // Frame
        _svgPainter.Border(margin - 1, margin - 1, SvgWidth - margin * 2 + 2, SvgHeight - margin * 2 + 2, _settings.Gray);

        // Title, Year, Artist
        var titleYearArtist = $"{artwork.Title} ({artwork.Year}) by {artwork.Artist}";
        _svgPainter.TextRight(SvgWidth - margin * 2, SvgHeight - margin / 2 + 2, titleYearArtist, (int)(margin * 0.6), _settings.Gray, 0);

        _svgPainter.Append("<g transform='translate(" + margin + "," + margin + ")'>");
        artwork.RenderAlgorithm(_settings, _svgPainter, artworkWidth, artworkHeight);
        _svgPainter.Append("</g>");

        return _svgPainter.SvgContent();
    }
}