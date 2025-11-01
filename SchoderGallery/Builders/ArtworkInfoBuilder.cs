using SchoderGallery.DTOs;
using SchoderGallery.Helpers;
using SchoderGallery.Navigation;
using SchoderGallery.Painters;
using SchoderGallery.Services;
using SchoderGallery.Settings;

namespace SchoderGallery.Builders;

public interface IArtworkInfoBuilder : IBuilder
{
    Task<string> GetHtmlAsync(int screenWidth, int screenHeight, int artworkId);
    public string Html { get; set; }
    public int HtmlWidth { get; set; }
    public int HtmlFontSize { get; set; }
    public string HtmlColor { get; set; }
}

public class ArtworkInfoBuilder(
    SettingsFactory settingsFactory,
    SvgPainter svgPainter,
    NavigationService navigation,
    IGalleryService galleryService,
    SizeHelperFactory sizeHelperFactory)
    : BaseBuilder(settingsFactory, svgPainter, navigation, galleryService), IArtworkInfoBuilder, IBuilder
{
    public override FloorType FloorType => FloorType.ArtworkInfo;

    public int Interval => 0;

    public string Html { get; set; }
    public int HtmlWidth { get; set; }
    public int HtmlFontSize { get; set; }
    public string HtmlColor { get; set; }

    public async Task<string> GetHtmlAsync(int screenWidth, int screenHeight, int artworkId)
    {
        Init(screenWidth, screenHeight);
        Html = string.Empty;

        var floor = await _navigation.GetVisitorFloorAsync();
        artworkId = _navigation.GetArtworkIdOrLatestArtworkId(floor.FloorType, artworkId);
        var artwork = await _galleryService.GetArtworkAsync(floor.FloorNumber, artworkId);

        // Later: If no artwork found, clear latest artwork id and go back to the floor

        await _navigation.SetLatestArtworkIdAsync(floor.FloorType, artwork.Id);

        var sizeHelper = sizeHelperFactory.GetHelper(SizeType.Text);
        var tinyMargin = _settings.TinyMargin;
        var iconSize = IsMobile ? _settings.IconSizeMobile : _settings.IconSizeDesktop;
        var iconSizePlus = iconSize + tinyMargin;
        var iconSizePlusx4 = iconSizePlus * 4;
        var topMargin = iconSize + 2 * tinyMargin;
        var availableArtworkWidth = SvgWidth - tinyMargin * 2;
        var availableArtworkHeight = SvgHeight - topMargin * 2 - tinyMargin * 2;
        var artworkSize = sizeHelper.GetArtworkSize(artwork, availableArtworkWidth, availableArtworkHeight, IsMobile);

        // Close -> back to artwork (top right)
        _svgPainter.IconClose(_width50 - iconSize / 2 - tinyMargin, tinyMargin, iconSize);
        ClickableAreas.Add(new ClickableArea(_width33 + 2, 0, _width33 - 4, iconSizePlusx4, $"/Artwork/{artworkId}", "Close"));

        // Buy (top right)
        if (artwork.SizeType != SizeType.Text)
        {
        }

        // Previous artwork (bottom left)
        _svgPainter.IconLeft(tinyMargin, SvgHeight - iconSizePlus, iconSize);
        if (artwork.PreviousId > -1)
        {
            ClickableAreas.Add(new ClickableArea(0, SvgHeight - iconSizePlusx4, _width33 - 2, iconSizePlusx4, $"/Artwork/{artwork.PreviousId}", "Previous artwork"));
        }
        else
        {
            ClickableAreas.Add(new ClickableArea(0, SvgHeight - iconSizePlusx4, _width33 - 2, iconSizePlusx4, floor.PageAndParam(), "Back"));
        }

        // Next artwork or back to floor (bottom right)
        _svgPainter.IconRight(SvgWidth - iconSizePlus, SvgHeight - iconSizePlus, iconSize);
        if (artwork.NextId > -1)
        {
            ClickableAreas.Add(new ClickableArea(_width33 * 2 + 2, SvgHeight - iconSizePlusx4, _width33 - 2, iconSizePlusx4, $"/Artwork/{artwork.NextId}", "Next artwork"));
        }
        else
        {
            ClickableAreas.Add(new ClickableArea(_width33 * 2 + 2, SvgHeight - iconSizePlusx4, _width33 - 2, iconSizePlusx4, floor.PageAndParam(), "Back"));
        }

        var size = artwork.SizeType == SizeType.PortraitLandscape
            ? "Up to 411 trillion px (~1 square mile)"
            : $"{artwork.Width}x{artwork.Height} px";

        Html = ConvertToParagraphs($"{artwork.Info ?? "Work in progress..."}\nTitle: {artwork.Title}\nYear: {artwork.Year}\nSize: {size}\nMaterial: Pixels\nArtist: {artwork.Artist}\nOwner: Schoder Factory Ltd");
        HtmlWidth = artworkSize.Width;
        HtmlFontSize = _fontSize;
        HtmlColor = Colours.DarkGray;

        return _svgPainter.SvgContent();
    }
}
