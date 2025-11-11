using SchoderGallery.Helpers;

namespace SchoderGallery.DTOs;

public record ExhibitionDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Colour { get; set; }
    public int Floor { get; set; }
    public List<ArtworkDto> Artworks { get; set; }
    public DateTime ArtworksLastLoadedDateTime { get; set; } = DateTime.MinValue;
    public bool LoadArtworksNeeded =>
        Artworks is null
        || Artworks.Count == 0
        || ArtworksLastLoadedDateTime < DateTime.UtcNow.AddMinutes(-Const.ARTWORKS_CACHE_TIMEOUT_MINUTES);

    public ExhibitionDto(string title, string colour)
    {
        Title = title;
        Colour = colour;
        Artworks = [];
    }
}
