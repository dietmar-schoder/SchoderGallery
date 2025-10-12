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
    : BaseBuilder(settingsFactory, svgPainter, navigation, galleryService), IArtworkBuilder, IBuilder
{
    public override BuilderType Type => BuilderType.Artwork;

    public int Interval => 0;

    public string GetArtworkHtml(int screenWidth, int screenHeight, int artworkId)
    {
        Init(screenWidth, screenHeight);

        var floor = _navigation.GetVisitorFloor();
        var artwork = _galleryService.GetArtworkAsync(floor.FloorNumber, artworkId);
        var sizeHelper = sizeHelperFactory.GetHelper(artwork.SizeType);

        var tinyMargin = _settings.TinyMargin;
        var iconSize = IsMobile ? _settings.IconSizeMobile : _settings.IconSizeDesktop;
        var iconSizePlus = iconSize + tinyMargin;
        var topMargin = iconSize + 2 * tinyMargin;
        var availableArtworkWidth = SvgWidth - tinyMargin * 2;
        var availableArtworkHeight = SvgHeight - topMargin * 2 - tinyMargin * 2;
        var artworkSize = sizeHelper.GetArtworkSize(artwork, availableArtworkWidth, availableArtworkHeight);
        var artworkLeftMargin = (SvgWidth - artworkSize.Width) / 2;
        var artworkTopMargin = (SvgHeight - artworkSize.Height) / 2;

        // Back to floor (top left)
        _svgPainter.IconLeftArrow(tinyMargin, tinyMargin, iconSize);
        ClickableAreas.Add(new ClickableArea(0, 0, _width33 - 2, _height50 - 2, floor.PageAndParam(), "Back"));

        // Comments (top middle)

        // Buy (top right)

        // Previous artwork (bottom left)
        if (artwork.PreviousId > -1)
        {
            _svgPainter.IconLeft(tinyMargin, SvgHeight - iconSizePlus, iconSize);
            ClickableAreas.Add(new ClickableArea(0, _height50 + 2, _width33 - 2, _height50 - 2, $"/Artwork/{artwork.PreviousId}", "Previous artwork"));
        }

        // Refresh (bottom middle)
        _svgPainter.IconRefresh(_width50 - iconSize / 2 - tinyMargin, SvgHeight - iconSizePlus, iconSize);
        ClickableAreas.Add(new ClickableArea(_width33 + 2, _height50 + 2, _width33 - 4, _height50 - 2, ReRender: true));

        // Next artwork (bottom right)
        if (artwork.NextId > -1)
        {
            _svgPainter.IconRight(SvgWidth - iconSizePlus, SvgHeight - iconSizePlus, iconSize);
            ClickableAreas.Add(new ClickableArea(_width33 * 2 + 2, _height50 + 2, _width33 - 2, _height50 - 2, $"/Artwork/{artwork.NextId}", "Next artwork"));
        }

        // Frame
        _svgPainter.Border(artworkLeftMargin - 1, artworkTopMargin - 1, artworkSize.Width + 2, artworkSize.Height + 2, Colours.Gray);

        // Title, Year, Artist
        var titleYearArtist = $"{artwork.Title} ({artwork.Year}) - {artwork.Artist}";
        _svgPainter.TextRight(artworkLeftMargin + artworkSize.Width - iconSize, artworkTopMargin + artworkSize.Height + _smallFontSize * 2 / 3 + 2, titleYearArtist, _smallFontSize, Colours.LightGray, 0);

        _svgPainter.Append($"<g transform='translate({artworkLeftMargin},{artworkTopMargin})'>");
        artwork.RenderAlgorithm(_settings, _svgPainter, artworkSize.Width, artworkSize.Height);
        _svgPainter.Append("</g>");

        return _svgPainter.SvgContent();
    }
}