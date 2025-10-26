using SchoderGallery.DTOs;

namespace SchoderGallery.Helpers;

public class TextSizeHelper : ISizeHelper
{
    public SizeType SizeType => SizeType.Text;

    public SizeDto GetArtworkSize(ArtworkDto artwork, int screenWidth, int screenHeight, bool isMobile = false)
    {
        if (isMobile)
        {
            return screenWidth > screenHeight
                ? new SizeDto(screenWidth * 70 / 100, 0) // Mobile landscape
                : new SizeDto(screenWidth * 80 / 100, 0); // Mobile portrait
        }

        return screenWidth > screenHeight
            ? new SizeDto(screenWidth * 33 / 100, 0) // Desktop landscape
            : new SizeDto(screenWidth * 67 / 100, 0); // Desktop portrait
    }
}
