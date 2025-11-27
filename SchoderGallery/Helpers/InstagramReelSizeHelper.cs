using SchoderGallery.DTOs;

namespace SchoderGallery.Helpers;

public class InstagramReelSizeHelper : ISizeHelper
{
    public SizeType SizeType => SizeType.InstagramReel;

    public SizeDto GetArtworkSize(ArtworkDto artwork, int screenWidth, int screenHeight, bool isMobile = false)
        => new(Math.Min(screenWidth * 80 / 100, 540), 0);
}
