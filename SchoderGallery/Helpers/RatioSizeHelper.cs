using SchoderGallery.DTOs;

namespace SchoderGallery.Helpers;

public class RatioSizeHelper : ISizeHelper
{
    public SizeType SizeType => SizeType.Ratio;

    public SizeDto GetArtworkSize(ArtworkDto artwork, int screenWidth, int screenHeight)
    {
        double ratioW = artwork.Width;
        double ratioH = artwork.Height;

        if (ratioW <= 0 || ratioH <= 0)
            return new SizeDto(screenWidth, screenHeight);

        double screenRatio = (double)screenWidth / screenHeight;
        double artworkRatio = ratioW / ratioH;

        return artworkRatio > screenRatio
            ? new SizeDto(screenWidth, (int)Math.Round(screenWidth / artworkRatio))
            : new SizeDto((int)Math.Round(screenHeight * artworkRatio), screenHeight);
    }
}