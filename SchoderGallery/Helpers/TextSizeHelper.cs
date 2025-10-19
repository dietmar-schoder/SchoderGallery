using SchoderGallery.DTOs;

namespace SchoderGallery.Helpers;

public class TextSizeHelper : ISizeHelper
{
    public SizeType SizeType => SizeType.Text;

    public SizeDto GetArtworkSize(ArtworkDto artwork, int screenWidth, int screenHeight) =>
        screenWidth > screenHeight
            ? new SizeDto(screenWidth * 33 / 100, 0)
            : new SizeDto(screenWidth * 67 / 100, 0);
}
