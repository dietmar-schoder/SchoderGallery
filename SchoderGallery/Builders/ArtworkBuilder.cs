using SchoderGallery.Algorithms;
using SchoderGallery.DTOs;
using SchoderGallery.Helpers;
using SchoderGallery.Navigation;
using SchoderGallery.Painters;
using SchoderGallery.Services;
using SchoderGallery.Settings;

namespace SchoderGallery.Builders;

public interface IArtworkBuilder : IBuilder
{
    Task<string> GetHtmlAsync(int screenWidth, int screenHeight, Guid artworkId);
    public string Html { get; set; }
    public int HtmlWidth { get; set; }
    public int HtmlFontSize { get; set; }
    public string HtmlColor { get; set; }
    public string InstagramId { get; set; }
    public ArtworkDto Artwork { get; set; }
    public bool ShowBuyButton => Artwork.IsForSale;
}

public class ArtworkBuilder(
    SettingsFactory settingsFactory,
    SvgPainter svgPainter,
    NavigationService navigation,
    GalleryService galleryService,
    SizeHelperFactory sizeHelperFactory,
    Image image)
    : BaseBuilder(settingsFactory, svgPainter, navigation, galleryService), IArtworkBuilder, IBuilder
{
    public override FloorType FloorType => FloorType.Artwork;
    public string Html { get; set; }
    public int HtmlWidth { get; set; }
    public int HtmlFontSize { get; set; }
    public string HtmlColor { get; set; }
    public string InstagramId { get; set; }
    public ArtworkDto Artwork { get; set; }

    public async Task<string> GetHtmlAsync(int screenWidth, int screenHeight, Guid artworkId)
    {
        Init(screenWidth, screenHeight);
        Visitor = await _navigation.GetInitVisitorAsync();
        Html = string.Empty;

        var floor = await _navigation.GetVisitorFloorAsync();
        artworkId = _navigation.GetArtworkIdOrLatestArtworkId(floor.FloorType, artworkId);
        Artwork = await _galleryService.GetArtworkAsync(Visitor, floor.FloorNumber, artworkId);

        // Later: If no artwork found, clear latest artwork id and go back to the floor

        await _navigation.SetLatestIdAsync(floor.FloorType, Artwork.Id);

        var sizeHelper = sizeHelperFactory.GetHelper(Artwork.SizeType);
        var tinyMargin = _settings.TinyMargin;
        var iconSize = IsMobile ? _settings.IconSizeMobile : _settings.IconSizeDesktop;
        var iconSizePlus = iconSize + tinyMargin;
        var iconSizePlusx4 = iconSizePlus * 4;
        var topMargin = iconSize + 2 * tinyMargin;
        var availableArtworkWidth = SvgWidth - tinyMargin * 2;
        var availableArtworkHeight = SvgHeight - topMargin * 2 - tinyMargin * 2;
        var artworkSize = sizeHelper.GetArtworkSize(Artwork, availableArtworkWidth, availableArtworkHeight, IsMobile);
        var artworkLeftMargin = (SvgWidth - artworkSize.Width) / 2;
        var artworkTopMargin = (SvgHeight - artworkSize.Height) / 2;
        var artworkType = ArtworkType.Static;

        // Images
        if (Artwork.SizeType != SizeType.Text)
        {
            _svgPainter.Append($"<g transform='translate({artworkLeftMargin},{artworkTopMargin})'>");

            // Jpg, png fixed size or portrait/landscape
            if (Artwork.SizeType == SizeType.Fixed || Artwork.SizeType == SizeType.PortraitLandscape)
            {
                var fileName = $"images/floor{Artwork.FloorNumber}/{Artwork.FileName}";
                if (Artwork.SizeType == SizeType.PortraitLandscape && ScreenMode == ScreenMode.Portrait)
                {
                    fileName = fileName.Replace("1920-1080", "1080-1920");
                }
                artworkType = image.JpgPng(_settings, artworkSize.Width, artworkSize.Height, fileName);
            }
            // Generative
            else if (Artwork.RenderAlgorithm != default)
            {
                artworkType = Artwork.RenderAlgorithm(_settings, artworkSize.Width, artworkSize.Height);
            }
            _svgPainter.Append("</g>");
        }

        // Back to floor (top left)
        _svgPainter.IconLeftArrow(tinyMargin, tinyMargin, iconSize);
        ClickableAreas.Add(new ClickableArea(0, 0, _width33 - 2, iconSizePlusx4, floor.PageAndParam(), "Back"));

        // Info (top middle)
        if (Artwork.SizeType != SizeType.Text)
        {
            _svgPainter.IconQuestionMark(_width50 - iconSize / 2 - tinyMargin, tinyMargin, iconSize);
            ClickableAreas.Add(new ClickableArea(_width33 + 2, 0, _width33 - 4, iconSizePlusx4, $"/ArtworkInfo/{artworkId}", "What is this?"));
        }

        // Previous artwork, if previous exists (bottom left)
        if (Artwork.PreviousId != Guid.Empty)
        {
            _svgPainter.IconLeft(tinyMargin, SvgHeight - iconSizePlus, iconSize);
            ClickableAreas.Add(new ClickableArea(0, SvgHeight - iconSizePlusx4, _width33 - 2, iconSizePlusx4, $"/Artwork/{Artwork.PreviousId}", "Previous artwork"));
        }

        // Refresh (bottom middle)
        if (artworkType == ArtworkType.Generative)
        {
            _svgPainter.IconRefresh(_width50 - iconSize / 2 - tinyMargin, SvgHeight - iconSizePlus, iconSize);
            ClickableAreas.Add(new ClickableArea(_width33 + 2, SvgHeight - iconSizePlusx4, _width33 - 4, iconSizePlusx4, ReRender: true));
        }

        // Next artwork, if next exists (bottom right)
        if (Artwork.NextId != Guid.Empty)
        {
            _svgPainter.IconRight(SvgWidth - iconSizePlus, SvgHeight - iconSizePlus, iconSize);
            ClickableAreas.Add(new ClickableArea(_width33 * 2 + 2, SvgHeight - iconSizePlusx4, _width33 - 2, iconSizePlusx4, $"/Artwork/{Artwork.NextId}", "Next artwork"));
        }

        InstagramId = null;
        // HtmlText
        if (Artwork.SizeType == SizeType.Text)
        {
            Html = ConvertToParagraphs(Artwork.Title);
            HtmlWidth = artworkSize.Width;
            HtmlFontSize = _fontSize;
            HtmlColor = Colours.DarkGray;
        }
        // Instagram Reel
        else if (Artwork.SizeType == SizeType.InstagramReel)
        {
            HtmlWidth = artworkSize.Width;
            InstagramId = Artwork.FileName;
        }
        else
        {
            // Frame
            _svgPainter.Border(artworkLeftMargin - 1, artworkTopMargin - 1, artworkSize.Width + 2, artworkSize.Height + 2, Colours.Gray);

            // Title, Year, Artist
            var titleYearArtist = $"{Artwork.Title} ({Artwork.Year})"; //  - {artwork.Artist}
            _svgPainter.TextRight(artworkLeftMargin + artworkSize.Width - iconSize, artworkTopMargin + artworkSize.Height + _smallFontSize * 2 / 3 + 2, titleYearArtist, _smallFontSize, Colours.Gray, 0);
        }

        return _svgPainter.SvgContent();
    }
}
