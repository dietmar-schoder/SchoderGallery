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
        var floor = _navigation.GetVisitorFloor();
        var artwork = galleryService.GetArtworkAsync(_settings, floor.FloorNumber, artworkId);
        var sizeHelper = sizeHelperFactory.GetHelper(artwork.SizeType);

        _svgPainter.Clear();
        _settings = _settingsFactory.GetSettings(screenWidth, screenHeight);
        ClickableAreas.Clear();

        SvgWidth = Math.Max(320, screenWidth - _settings.OuterMargin * 2);
        SvgHeight = Math.Max(320, screenHeight - _settings.OuterMargin * 2);

        var tinyMargin = _settings.TinyMargin;
        var iconSize = IsMobile ? _settings.IconSizeMobile : _settings.IconDesktop;
        var fontSize = IsMobile ? _settings.FontSizeMobile : _settings.FontSizeDesktop;
        var iconSizePlus = iconSize + tinyMargin;
        var topMargin = iconSize + 2 * tinyMargin;
        var availableArtworkWidth = SvgWidth - tinyMargin * 2;
        var availableArtworkHeight = SvgHeight - topMargin * 2 - tinyMargin * 2;
        var artworkSize = sizeHelper.GetArtworkSize(artwork, availableArtworkWidth, availableArtworkHeight);
        var artworkLeftMargin = (SvgWidth - artworkSize.Width) / 2;
        var artworkTopMargin = (SvgHeight - artworkSize.Height) / 2;

        var widthHalf = SvgWidth / 2;
        var width3rd = SvgWidth / 3;
        var heightHalf = SvgHeight / 2;

        // Back to floor (top left)
        _svgPainter.IconLeftArrow(tinyMargin, tinyMargin, iconSize, _settings);
        ClickableAreas.Add(new ClickableArea(0, 0, width3rd - 2, heightHalf - 2, floor.PageAndParam(), "Back"));

        // Comments (top middle)

        // Buy (top right)

        // Previous artwork (bottom left)
        if (artwork.PreviousId > -1)
        {
            _svgPainter.IconLeft(tinyMargin, SvgHeight - iconSizePlus, iconSize, _settings);
            ClickableAreas.Add(new ClickableArea(0, heightHalf + 2, width3rd - 2, heightHalf - 2, $"/Artwork/{artwork.PreviousId}", "Previous artwork"));
        }

        // Refresh (bottom middle)
        _svgPainter.IconRefresh(widthHalf - iconSize / 2 - tinyMargin, SvgHeight - iconSizePlus, iconSize, _settings);
        ClickableAreas.Add(new ClickableArea(width3rd + 2, heightHalf + 2, width3rd - 4, heightHalf - 2, ReRender: true));

        // Next artwork (bottom right)
        if (artwork.NextId > -1)
        {
            _svgPainter.IconRight(SvgWidth - iconSizePlus, SvgHeight - iconSizePlus, iconSize, _settings);
            ClickableAreas.Add(new ClickableArea(width3rd * 2 + 2, heightHalf + 2, width3rd - 2, heightHalf - 2, $"/Artwork/{artwork.NextId}", "Next artwork"));
        }

        // Frame
        _svgPainter.Border(artworkLeftMargin - 1, artworkTopMargin - 1, artworkSize.Width + 2, artworkSize.Height + 2, _settings.Gray);

        // Title, Year, Artist
        var titleYearArtist = $"{artwork.Title} ({artwork.Year}) - {artwork.Artist}";
        _svgPainter.TextRight(artworkLeftMargin + artworkSize.Width - iconSize, artworkTopMargin + artworkSize.Height + topMargin / 2, titleYearArtist, fontSize, _settings.LightGray, 0);

        _svgPainter.Append($"<g transform='translate({artworkLeftMargin},{artworkTopMargin})'>");
        artwork.RenderAlgorithm(_settings, _svgPainter, artworkSize.Width, artworkSize.Height);
        _svgPainter.Append("</g>");

        return _svgPainter.SvgContent();
    }
}