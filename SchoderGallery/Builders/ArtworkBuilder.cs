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
    Task<string> GetArtworkHtmlAsync(int screenWidth, int screenHeight, int artworkId);
    public string Html { get; set; }
    public int HtmlWidth { get; set; }
    public int HtmlFontSize { get; set; }
    public string HtmlColor { get; set; }
}

public class ArtworkBuilder(
    SettingsFactory settingsFactory,
    SvgPainter svgPainter,
    NavigationService navigation,
    IGalleryService galleryService,
    SizeHelperFactory sizeHelperFactory,
    Image image)
    : BaseBuilder(settingsFactory, svgPainter, navigation, galleryService), IArtworkBuilder, IBuilder
{
    public override BuilderType Type => BuilderType.Artwork;

    public int Interval => 0;

    public string Html { get; set; }
    public int HtmlWidth { get; set; }
    public int HtmlFontSize { get; set; }
    public string HtmlColor { get; set; }

    public async Task<string> GetArtworkHtmlAsync(int screenWidth, int screenHeight, int artworkId)
    {
        Init(screenWidth, screenHeight);
        Html = string.Empty;

        var floor = await _navigation.GetVisitorFloorAsync();
        artworkId = _navigation.GetArtworkIdOrLatestArtworkId(artworkId);
        var artwork = await _galleryService.GetArtworkAsync(floor.FloorNumber, artworkId);

        // If no artwork found, clear latest artwork id and go back to the floor

        _navigation.SetLatestArtworkId(artwork.Id);

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
        var artworkType = ArtworkType.Static;

        // Images
        if (artwork.SizeType != SizeType.Text)
        {
            _svgPainter.Append($"<g transform='translate({artworkLeftMargin},{artworkTopMargin})'>");

            // Jpg, png
            if (artwork.SizeType == SizeType.Fixed)
            {
                var fileName = $"images/floor{floor.FloorNumber}/{artwork.FileName}";
                if (ScreenMode == ScreenMode.Portrait)
                {
                    fileName = fileName.Replace("1920-1080", "1080-1920");
                }
                artworkType = image.JpgPng(_settings, artworkSize.Width, artworkSize.Height, fileName);
            }
            // Generative
            else if (artwork.RenderAlgorithm != default)
            {
                artworkType = artwork.RenderAlgorithm(_settings, artworkSize.Width, artworkSize.Height);
            }
            _svgPainter.Append("</g>");
        }

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
        if (artworkType == ArtworkType.Generative)
        {
            _svgPainter.IconRefresh(_width50 - iconSize / 2 - tinyMargin, SvgHeight - iconSizePlus, iconSize);
            ClickableAreas.Add(new ClickableArea(_width33 + 2, _height50 + 2, _width33 - 4, _height50 - 2, ReRender: true));
        }

        // Next artwork (bottom right)
        if (artwork.NextId > -1)
        {
            _svgPainter.IconRight(SvgWidth - iconSizePlus, SvgHeight - iconSizePlus, iconSize);
            ClickableAreas.Add(new ClickableArea(_width33 * 2 + 2, _height50 + 2, _width33 - 2, _height50 - 2, $"/Artwork/{artwork.NextId}", "Next artwork"));
        }

        // HtmlText
        if (artwork.SizeType == SizeType.Text)
        {
            Html = ConvertToParagraphs(artwork.Title);
            HtmlWidth = artworkSize.Width;
            HtmlFontSize = _fontSize;
            HtmlColor = Colours.DarkGray;
        }
        else
        {
            // Frame
            _svgPainter.Border(artworkLeftMargin - 1, artworkTopMargin - 1, artworkSize.Width + 2, artworkSize.Height + 2, Colours.Gray);

            // Title, Year, Artist
            var titleYearArtist = $"{artwork.Title} ({artwork.Year}) - {artwork.Artist}";
            _svgPainter.TextRight(artworkLeftMargin + artworkSize.Width - iconSize, artworkTopMargin + artworkSize.Height + _smallFontSize * 2 / 3 + 2, titleYearArtist, _smallFontSize, Colours.Gray, 0);
        }

        return _svgPainter.SvgContent();
    }
    private static string ConvertToParagraphs(string text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        var paragraphs = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var paragraphTags = paragraphs.Select(p => $"<p>{p.Trim()}</p>");
        return string.Join("", paragraphTags);
    }
}