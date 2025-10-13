using SchoderGallery.Algorithms;
using SchoderGallery.DTOs;
using SchoderGallery.Painters;
using SchoderGallery.Settings;

namespace SchoderGallery.Services;

public interface IGalleryService
{
    List<TodoDto> GetTodosAsync();
    ExhibitionDto GetExhibition(int floorNumber);
    List<ArtworkDto> GetArtworksAsync(int floorNumber);
    ArtworkDto GetArtworkAsync(int floorNumber, int id);
}

public class GalleryService : IGalleryService
{
    private const string Dietmar = "Dietmar Schoder";
    private readonly Colours _colours;
    private readonly FourColours _fourColours;
    private readonly Image _image;
    private readonly TurtleGraphics _turtleGraphics;
    private readonly Dictionary<int, ExhibitionDto> _exhibitionCache;

    private List<ArtworkDto> CreateFloor1Artworks() => LinkArtworks(
    [
        NewArtwork("Adventure 1/5", 2025, 1, (s, w, h) => _turtleGraphics.Turtle1(s, w, h, 8, 4, closePath: true), Dietmar),
        NewArtwork("Adventure 2/5", 2025, 9, (s, w, h) => _turtleGraphics.Turtle1Smooth(s, w, h, 8, 4, closePath : true), Dietmar),
        NewArtwork("Adventure 3/5", 2025, 2, (s, w, h) => _turtleGraphics.Turtle1(s, w, h, 16, 9), Dietmar),
        NewArtwork("Adventure 4/5", 2025, 3, (s, w, h) => _turtleGraphics.Turtle2(s, w, h, 13, 7), Dietmar),
        NewArtwork("Adventure 5/5", 2025, 4, (s, w, h) => _turtleGraphics.Turtle2(s, w, h, 32, 18, 1), Dietmar),
    ]);

    private List<ArtworkDto> CreateFloor2Artworks() => LinkArtworks(
    [
        NewArtwork("Door No.1", 2025, 5, (s, w, h) => _fourColours.Pattern1(s, w, h, 10, 6, _colours.Blueish20Colours), Dietmar),
        NewArtwork("Door No.2", 2025, 6, (s, w, h) => _fourColours.Pattern1(s, w, h, 21, 13, _colours.Warm20AccentColours), Dietmar),
        NewArtwork("Door No.3", 2025, 10, (s, w, h) => _fourColours.Pattern1(s, w, h, 49, 37, _colours.MixedColoursBW), Dietmar),
    ]);

    private List<ArtworkDto> CreateAtelierArtworks() => LinkArtworks(
    [
        NewArtwork("Experiment #1", 2025, 7, (s, w, h) => _fourColours.Pattern2(s, w, h, 4, 4, _colours.Warm20AccentColours), Dietmar),
        NewArtwork("Experiment #2", 2025, 8, (s, w, h) => _fourColours.Pattern2(s, w, h, 8, 6, _colours.Blueish20Colours), Dietmar),
        NewArtwork("Experiment #3", 2025, 11, (s, w, h) => _image.Jpg(s, w, h, "000011.jpg"), Dietmar, SizeType.Fixed, 2160, 3820),
    ]);

    public GalleryService(AlgorithmFactory algorithmFactory, Colours colours)
    {
        _colours = colours;
        _fourColours = algorithmFactory.GetAlgorithm(AlgorithmType.FourColours) as FourColours;
        _image = algorithmFactory.GetAlgorithm(AlgorithmType.Image) as Image;
        _turtleGraphics = algorithmFactory.GetAlgorithm(AlgorithmType.TurtleGraphics) as TurtleGraphics;

        _exhibitionCache = new Dictionary<int, ExhibitionDto>
        {
            [1] = new ExhibitionDto("Erasures", Colours.WarmAccentOrange, CreateFloor1Artworks),
            [2] = new ExhibitionDto("Four Horsemen", Colours.WarmAccentMagenta, CreateFloor2Artworks),
            [7] = new ExhibitionDto("Atelier", Colours.WarmAccentYellow, CreateAtelierArtworks)
        };
    }

    public List<TodoDto> GetTodosAsync()
    {
        List<TodoDto> todos = [
            new("Jpg/Png images as artworks", TodoStatus.InProgress),
            new("OnMouseOver for links", TodoStatus.InProgress),
            new("Explanation pages", TodoStatus.InProgress),

            new("Artwork: comments", TodoStatus.Planned),
            new("Artwork: buy", TodoStatus.Planned),
            new("Artwork: sell", TodoStatus.Planned),
            new("Cafe with payment and coffee as artworks", TodoStatus.Planned),
            new("Launch the SCHODER GALLERY", TodoStatus.Planned),

            new("Exhibition \"Find Me!\"", TodoStatus.Planned),
            new("Exhibition \"Hitler Eats Beigel\"", TodoStatus.Planned),
            new("Exhibition \"Who Am I?\"", TodoStatus.Planned),

            new("You are here", TodoStatus.Finished, 13, 10, 2025),
            new("Mobile and desktop font sizes", TodoStatus.Finished, 12, 10, 2025),
            new("Exhibitions in floors + lift labels", TodoStatus.Finished, 12, 10, 2025),
            new("Hourglass", TodoStatus.Finished, 11, 10, 2025),
            new("Artwork: refresh", TodoStatus.Finished, 6, 10, 2025),
            new("Artwork: prev/next/back", TodoStatus.Finished, 6, 10, 2025),
            new("Floor: start viewing artworks", TodoStatus.Finished, 6, 10, 2025),
            new("Favicon Schoder Factory brick", TodoStatus.Finished, 5, 10, 2025),
            new("To do list", TodoStatus.Finished, 5, 10, 2025),
            new("Fix mobile margins", TodoStatus.Finished, 5, 10, 2025),
            new("Lift and floors", TodoStatus.Finished, 4, 10, 2025),
        ];

        var today = DateTime.UtcNow;
        var nextday = today;
        foreach (var todo in todos.Where(t => t.Date == default))
        {
            todo.Date = todo.Status == TodoStatus.InProgress ? today : nextday;
            nextday = nextday.AddDays(1);
        }

        return [.. todos.OrderBy(t => t.Status).ThenBy(t => t.Date)];
    }
    public ExhibitionDto GetExhibition(int floorNumber)
    {
        if (!_exhibitionCache.TryGetValue(floorNumber, out var exhibition))
            return null;

        if (exhibition.Artworks.Count == 0 && exhibition.ArtworkFactory is not null)
        {
            var artworks = exhibition.ArtworkFactory();
            exhibition.Artworks.AddRange(artworks);
        }

        return exhibition;
    }

    public List<ArtworkDto> GetArtworksAsync(int floorNumber)
    {
        if (!_exhibitionCache.TryGetValue(floorNumber, out var exhibition))
            return [];

        if (exhibition.Artworks.Count == 0 && exhibition.ArtworkFactory is not null)
        {
            var artworks = exhibition.ArtworkFactory();
            exhibition.Artworks.AddRange(artworks);
        }

        return exhibition.Artworks;
    }

    public ArtworkDto GetArtworkAsync(int floorNumber, int id)
    {
        var artworks = GetArtworksAsync(floorNumber);
        var artwork = id > 0
            ? artworks.FirstOrDefault(a => a.Id == id)
            : null;

        return artwork ?? artworks.FirstOrDefault(a => a.PreviousId == -1);
    }

    private static ArtworkDto NewArtwork(
        string title,
        int year,
        int id,
        Func<ISettings, int, int, int> renderAlgorithm,
        string artist,
        SizeType sizeType = SizeType.Dynamic,
        int fixedWidth = 0, int fixedHeight = 0)
        => new(title, year, renderAlgorithm, sizeType, fixedWidth, fixedHeight, artist, id);

    private static List<ArtworkDto> LinkArtworks(List<ArtworkDto> artworks)
    {
        if (artworks.Count == 0) { return artworks; }

        artworks.First().PreviousId = -1;
        artworks.Last().NextId = -1;

        for (int j = 0; j < artworks.Count - 1; j++)
        {
            artworks[j].NextId = artworks[j + 1].Id;
            artworks[j + 1].PreviousId = artworks[j].Id;
        }

        return artworks;
    }
}