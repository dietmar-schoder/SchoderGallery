using SchoderGallery.DTOs;

namespace SchoderGallery.Helpers;

public class FixedSizeHelper : ISizeHelper
{
    public SizeType SizeType => SizeType.Fixed;

    public SizeDto GetArtworkSize(ArtworkDto artwork, int screenWidth, int screenHeight)
    {
        double scale = 1.0;

        if (artwork.Width > screenWidth || artwork.Height > screenHeight)
        {
            double scaleW = (double)screenWidth / artwork.Width;
            double scaleH = (double)screenHeight / artwork.Height;
            scale = Math.Min(scaleW, scaleH);
        }

        int finalW = (int)Math.Round(artwork.Width * scale);
        int finalH = (int)Math.Round(artwork.Height * scale);
        return new SizeDto(finalW, finalH);
    }
}