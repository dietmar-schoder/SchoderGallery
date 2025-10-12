namespace SchoderGallery.DTOs;

public record ExhibitionDto
{
    public string LiftLabel { get; init; }
    public string LabelColour { get; init; }
    public Func<List<ArtworkDto>> ArtworkFactory { get; init; }
    public List<ArtworkDto> Artworks { get; init; }

    public ExhibitionDto(string liftLabel, string labelColour, Func<List<ArtworkDto>> artworkFactory)
    {
        LiftLabel = liftLabel;
        LabelColour = labelColour;
        ArtworkFactory = artworkFactory;
        Artworks = [];
    }
}