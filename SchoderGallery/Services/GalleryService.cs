using SchoderGallery.Algorithms;
using SchoderGallery.DTOs;
using SchoderGallery.Painters;
using SchoderGallery.Settings;
using System.Net.Http.Json;

namespace SchoderGallery.Services;

public interface IGalleryService
{
    List<TodoDto> GetTodosAsync();
    Task<ExhibitionDto> GetExhibitionAsync(int floorNumber);
    Task<ArtworkDto> GetArtworkAsync(int floorNumber, int id);
}

public class GalleryService : IGalleryService
{
    private const string Dietmar = "Dietmar Schoder";
    private readonly ClientFactory _http;
    private readonly Colours _colours;
    private readonly FourColours _fourColours;
    private readonly Image _image;
    private readonly TurtleGraphics _turtleGraphics;
    private readonly Dictionary<int, ExhibitionDto> _exhibitions;

    private Dictionary<int, ExhibitionDto> CreateExhibitions => new()
    {
        [1] = new ExhibitionDto("New Home", Colours.WarmAccentOrange, default),
        [7] = new ExhibitionDto("Atelier", Colours.DeepBlue, CreateAtelierArtworks)
    };

    private List<ArtworkDto> CreateAtelierArtworks(int floorNumber) => LinkArtworks(
    [
        NewArtwork("Attempt #1", 2025, 102, (s, w, h) => _turtleGraphics.Turtle1(s, w, h, 8, 4, closePath: true), Dietmar),
        NewArtwork("Attempt #2", 2025, 103, (s, w, h) => _turtleGraphics.Turtle1Smooth(s, w, h, 8, 4, closePath : true), Dietmar),
        NewArtwork("Attempt #3", 2025, 104, (s, w, h) => _turtleGraphics.Turtle1(s, w, h, 16, 9), Dietmar),
        NewArtwork("Attempt #4", 2025, 105, (s, w, h) => _turtleGraphics.Turtle2(s, w, h, 13, 7), Dietmar),
        NewArtwork("Attempt #5", 2025, 106, (s, w, h) => _turtleGraphics.Turtle2(s, w, h, 32, 18, 1), Dietmar),
        NewArtwork("Attempt #6", 2025, 107, (s, w, h) => _turtleGraphics.Turtle1Smooth(s, w, h, 12, 6, closePath : true), Dietmar),
        NewArtwork("Attempt #7", 2025, 108, (s, w, h) => _fourColours.Pattern1(s, w, h, 10, 6, _colours.Blueish20Colours), Dietmar),
        NewArtwork("Attempt #8", 2025, 109, (s, w, h) => _fourColours.Pattern1(s, w, h, 21, 13, _colours.Warm20AccentColours), Dietmar),
        NewArtwork("Attempt #9", 2025, 110, (s, w, h) => _fourColours.Pattern1(s, w, h, 49, 37, _colours.MixedColoursBW), Dietmar),
        NewArtwork("Attempt #10", 2025, 111, (s, w, h) => _fourColours.Pattern2(s, w, h, 4, 4, _colours.Warm20AccentColours), Dietmar),
        NewArtwork("Attempt #11", 2025, 112, (s, w, h) => _fourColours.Pattern2(s, w, h, 8, 6, _colours.Blueish20Colours), Dietmar),
    ]);

    public GalleryService(ClientFactory http, AlgorithmFactory algorithmFactory, Colours colours)
    {
        _http = http;
        _colours = colours;
        _fourColours = algorithmFactory.GetAlgorithm(AlgorithmType.FourColours) as FourColours;
        _image = algorithmFactory.GetAlgorithm(AlgorithmType.Image) as Image;
        _turtleGraphics = algorithmFactory.GetAlgorithm(AlgorithmType.TurtleGraphics) as TurtleGraphics;
        _exhibitions = CreateExhibitions;
    }

    public List<TodoDto> GetTodosAsync()
    {
        List<TodoDto> todos = [
            new("Buy membership", TodoStatus.InProgress),
            new("Subscribe to invitations", TodoStatus.InProgress),
            new("Center vistior point", TodoStatus.InProgress),
            new("Make artworks clickable", TodoStatus.InProgress),

            new("Artwork: comments", TodoStatus.Planned),
            new("Artwork: buy", TodoStatus.Planned),
            new("Artwork: sell", TodoStatus.Planned),
            new("Cafe with payment and coffee as artworks", TodoStatus.Planned),
            new("Launch the SCHODER GALLERY", TodoStatus.Planned),

            new("Exhibition \"Morning Walk\"", TodoStatus.Planned),
            new("Exhibition \"Find Me!\"", TodoStatus.Planned),
            new("Exhibition \"Hitler Eats Beigel\"", TodoStatus.Planned),
            new("Exhibition \"Who Am I?\"", TodoStatus.Planned),
            new("Exhibition \"Boris Bike\"", TodoStatus.Planned),

            new("Remember artwork id per floor", TodoStatus.Finished, 26, 10, 2025),
            new("Explanation pages", TodoStatus.Finished, 26, 10, 2025),
            new("Jpg/Png images as artworks", TodoStatus.Finished, 20, 10, 2025),
            new("OnMouseOver for links", TodoStatus.Finished, 18, 10, 2025),
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

    public async Task<ArtworkDto> GetArtworkAsync(int floorNumber, int id)
    {
        var artworks = await GetArtworksAsync(floorNumber);
        var artwork = id > 0
            ? artworks.FirstOrDefault(a => a.Id == id)
            : null;

        return artwork ?? artworks.FirstOrDefault(a => a.PreviousId == -1);
    }

    public async Task<ExhibitionDto> GetExhibitionAsync(int floorNumber)
    {
        if (!_exhibitions.TryGetValue(floorNumber, out var exhibition))
            return null;

        if (exhibition.Artworks.Count == 0)
        {
            await GetArtworksAsync(floorNumber);
        }

        return exhibition;
    }

    private async Task<List<ArtworkDto>> GetArtworksAsync(int floorNumber)
    {
        if (!_exhibitions.TryGetValue(floorNumber, out var exhibition))
            return [];

        if (exhibition.Artworks.Count == 0)
        {
            var artworks = exhibition.ReadFromArtworksJson
                ? await GetArtworksFromJson(floorNumber)
                : exhibition.ArtworkFactory(floorNumber); // Later: Get artworks for ALL floors from their JSON file

            exhibition.Artworks.AddRange(LinkArtworks(artworks));
        }

        return exhibition.Artworks;
    }

    private async Task<List<ArtworkDto>> GetArtworksFromJson(int floorNumber) =>
        await _http.Frontend.GetFromJsonAsync<List<ArtworkDto>>(ArtworksJson(floorNumber));

    private static string ArtworksJson(int floorNumber) =>
        $"images/floor{floorNumber}/_artworks.json";

    private static ArtworkDto NewArtwork(
        string title,
        int year,
        int id,
        Func<ISettings, int, int, ArtworkType> renderAlgorithm,
        string artist,
        SizeType sizeType = SizeType.Dynamic,
        int fixedWidth = 0, int fixedHeight = 0)
        => new(title, year, renderAlgorithm, sizeType, fixedWidth, fixedHeight, artist, id);

    private static List<ArtworkDto> LinkArtworks(List<ArtworkDto> artworks)
    {
        if (artworks.Count == 0) { return artworks; }

        artworks = [.. artworks.OrderBy(a => a.Id)];
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
