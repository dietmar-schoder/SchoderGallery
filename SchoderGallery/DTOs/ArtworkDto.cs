using SchoderGallery.Painters;
using SchoderGallery.Settings;

namespace SchoderGallery.DTOs;

public record ArtworkDto(
    int Id,
    string Title,
    int PreviousId,
    int NextId,
    Func<ISettings, SvgPainter, int, int, int> RenderAlgorithm);