using SchoderGallery.DTOs;

namespace SchoderGallery.Helpers;

public class InstagramReelSizeHelper : ISizeHelper
{
    public SizeType SizeType => SizeType.InstagramReel;

    public SizeDto GetArtworkSize(ArtworkDto artwork, int screenWidth, int screenHeight, bool isMobile = false)
    {
        int maxWidthFromHeight = (int)(screenHeight * 0.8 / 1.91);
        int maxWidthFromWidth = screenWidth * 80 / 100;
        int maxWidth = Math.Min(Math.Min(maxWidthFromHeight, maxWidthFromWidth), 540);
        return new SizeDto(maxWidth, 0);
    }
}
