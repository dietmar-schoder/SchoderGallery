using SchoderGallery.Painters;
using SchoderGallery.Settings;

namespace SchoderGallery.DTOs;

public class ArtworkDto(
    string title,
    int year,
    Func<ISettings, SvgPainter, int, int, int> renderAlgorithm,
    string artist,
    int id,
    int previousId,
    int nextId)
{
    public string Title { get; } = title;
    public int Year { get; } = year;
    public Func<ISettings, SvgPainter, int, int, int> RenderAlgorithm { get; } = renderAlgorithm;
    public string Artist { get; } = artist;
    public int Id { get; } = id;
    public int PreviousId { get; set; } = previousId;
    public int NextId { get; set; } = nextId;
}