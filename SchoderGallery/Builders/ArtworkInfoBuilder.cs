using SchoderGallery.DTOs;
using SchoderGallery.Helpers;
using SchoderGallery.Navigation;
using SchoderGallery.Painters;
using SchoderGallery.Services;
using SchoderGallery.Settings;

namespace SchoderGallery.Builders;

public interface IArtworkInfoBuilder : IBuilder
{
    void Init(int screenWidth, int screenHeight);
    Task<string> GetHtmlAsync(int screenWidth, int screenHeight, Guid artworkId);
    public string Html { get; set; }
    public int HtmlWidth { get; set; }
    public int HtmlFontSize { get; set; }
    public string HtmlColor { get; set; }
    public ArtworkDto Artwork { get; set; }
    public bool ShowBuyButton => Artwork.IsForSale;
}

public class ArtworkInfoBuilder(
    SettingsFactory settingsFactory,
    SvgPainter svgPainter,
    NavigationService navigation,
    GalleryService galleryService,
    SizeHelperFactory sizeHelperFactory)
    : BaseBuilder(settingsFactory, svgPainter, navigation, galleryService), IArtworkInfoBuilder, IBuilder
{
    public override FloorType FloorType => FloorType.ArtworkInfo;
    public string Html { get; set; }
    public int HtmlWidth { get; set; }
    public int HtmlFontSize { get; set; }
    public string HtmlColor { get; set; }
    public ArtworkDto Artwork { get; set; }

    public async Task<string> GetHtmlAsync(int screenWidth, int screenHeight, Guid artworkId)
    {
        Init(screenWidth, screenHeight);
        Html = string.Empty;

        var floor = await _navigation.GetVisitorFloorAsync();
        artworkId = _navigation.GetArtworkIdOrLatestArtworkId(floor.FloorType, artworkId);
        Artwork = await _galleryService.GetArtworkAsync(Visitor, floor.FloorNumber, artworkId);

        // Later: If no artwork found, clear latest artwork id and go back to the floor

        await _navigation.SetLatestIdAsync(floor.FloorType, Artwork.Id);

        var sizeHelper = sizeHelperFactory.GetHelper(SizeType.Text);
        var tinyMargin = _settings.TinyMargin;
        var iconSize = IsMobile ? _settings.IconSizeMobile : _settings.IconSizeDesktop;
        var iconSizePlus = iconSize + tinyMargin;
        var iconSizePlusx4 = iconSizePlus * 4;
        var topMargin = iconSize + 2 * tinyMargin;
        var availableArtworkWidth = SvgWidth - tinyMargin * 2;
        var availableArtworkHeight = SvgHeight - topMargin * 2 - tinyMargin * 2;
        var artworkSize = sizeHelper.GetArtworkSize(Artwork, availableArtworkWidth, availableArtworkHeight, IsMobile);

        // Close -> back to artwork (top right)
        _svgPainter.IconClose(_width50 - iconSize / 2 - tinyMargin, tinyMargin, iconSize);
        ClickableAreas.Add(new ClickableArea(_width33 + 2, 0, _width33 - 4, iconSizePlusx4, $"/Artwork/{artworkId}", "Close"));

        // Previous artwork, if previous exists (bottom left)
        if (Artwork.PreviousId != Guid.Empty)
        {
            _svgPainter.IconLeft(tinyMargin, SvgHeight - iconSizePlus, iconSize);
            ClickableAreas.Add(new ClickableArea(0, SvgHeight - iconSizePlusx4, _width33 - 2, iconSizePlusx4, $"/Artwork/{Artwork.PreviousId}", "Previous artwork"));
        }

        // Next artwork, if next exists (bottom right)
        if (Artwork.NextId != Guid.Empty)
        {
            _svgPainter.IconRight(SvgWidth - iconSizePlus, SvgHeight - iconSizePlus, iconSize);
            ClickableAreas.Add(new ClickableArea(_width33 * 2 + 2, SvgHeight - iconSizePlusx4, _width33 - 2, iconSizePlusx4, $"/Artwork/{Artwork.NextId}", "Next artwork"));
        }

        var size = string.Empty;
        if (Artwork.Width > 0)
        {
            size = Artwork.SizeType == SizeType.PortraitLandscape
                ? "Up to 411 trillion px (~1 square mile)"
                : $"{Artwork.Width}x{Artwork.Height} px";
            size = $"\nSize: {size}";
        }

        string owner;
        string info;
        if (Artwork.SizeType == SizeType.InstagramReel)
        {
            info = "\nInstagram Reel";
            owner = string.IsNullOrEmpty(Artwork.Info) ? "" : Artwork.Info;
        }
        else
        {
            info = string.IsNullOrEmpty(Artwork.Info) ? "" : Artwork.Info;
            owner = Artwork.IsForSale ? $"\nPrice: {Artwork.PriceFormatted}" : string.Empty;
        }

        Html = ConvertToParagraphs(
            info +
            $"\nTitle: <b>{Artwork.Title}</b>" +
            $"\nYear: {Artwork.Year}" +
            size +
            $"\nMaterial: Pixels" +
            $"\nArtist: Schoder Factory Ltd" +
            $"\nOwner: {(Artwork.HasOwner ? "Private Collector" : "Schoder Factory Ltd")}" +
            owner);
        HtmlWidth = artworkSize.Width;
        HtmlFontSize = _fontSize;
        HtmlColor = Colours.DarkGray;

        return _svgPainter.SvgContent();
    }
}
