using SchoderGallery.DTOs;

namespace SchoderGallery.Helpers;

public interface ISizeHelper
{
    SizeDto GetArtworkSize(ArtworkDto artwork, int screenWidth, int screenHeight);
    SizeType SizeType { get; }
}