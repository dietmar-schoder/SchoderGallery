using SchoderGallery.DTOs;

namespace SchoderGallery.Helpers;

public class FixedPortLandSizeHelper : ISizeHelper
{
    public SizeType SizeType => SizeType.PortraitLandscape;

    public SizeDto GetArtworkSize(ArtworkDto artwork, int screenWidth, int screenHeight, bool isMobile = false)
    {
        double scale = 1.0;

        var artworkWidth = screenWidth > screenHeight ? artwork.Width : artwork.Height;
        var artworkHeight = screenWidth > screenHeight ? artwork.Height : artwork.Width;

        if (artworkWidth > screenWidth || artworkHeight > screenHeight)
        {
            double scaleW = (double)screenWidth / artworkWidth;
            double scaleH = (double)screenHeight / artworkHeight;
            scale = Math.Min(scaleW, scaleH);
        }

        int finalW = (int)Math.Round(artworkWidth * scale);
        int finalH = (int)Math.Round(artworkHeight * scale);
        return new SizeDto(finalW, finalH);
    }
}
