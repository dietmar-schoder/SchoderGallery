using SchoderGallery.DTOs;

namespace SchoderGallery.Helpers;

public class DynamicSizeHelper : ISizeHelper
{
    public SizeType SizeType => SizeType.Dynamic;

    public SizeDto GetArtworkSize(ArtworkDto artwork, int screenWidth, int screenHeight)
    {
        const double maxRatioW = 16.0;
        const double maxRatioH = 9.0;
        double maxRatio = maxRatioW / maxRatioH;
        double screenRatio = (double)screenWidth / screenHeight;

        int finalW, finalH;

        if (screenRatio > maxRatio)
        {
            finalH = screenHeight;
            finalW = (int)Math.Round(finalH * maxRatio);
        }
        else if (screenRatio < 1 / maxRatio)
        {
            finalW = screenWidth;
            finalH = (int)Math.Round(finalW * maxRatio);
        }
        else
        {
            finalW = screenWidth;
            finalH = screenHeight;
        }

        return new SizeDto(finalW, finalH);
    }
}