using SchoderGallery.Algorithms;
using SchoderGallery.DTOs;
using SchoderGallery.Painters;
using SchoderGallery.Settings;
using SchoderGalleryServer.DTOs;
using System.Net.Http.Json;

namespace SchoderGallery.Services;

public interface IGalleryService
{
    List<TodoDto> GetTodosAsync();
    Task<ExhibitionDto> GetExhibitionAsync(int floorNumber);
    Task<ExhibitionDto> GetExhibitionArtworksAsync(int floorNumber);
    Task<ArtworkDto> GetArtworkAsync(int floorNumber, int id);
    Task<CheckoutDto> BuyArtworkAsync(Guid collectorId, ArtworkDto artwork);
}

public class GalleryService : IGalleryService
{
    private const string Dietmar = "Dietmar Schoder";
    private readonly ClientFactory _http;
    private readonly Colours _colours;
    private readonly FourColours _fourColours;
    private readonly TurtleGraphics _turtleGraphics;

    private Dictionary<int, ExhibitionDto> _exhibitions;
    private DateTime _exhibitionsLastLoadedDate = DateTime.MinValue;

    private bool LoadExhibitionsNeeded =>
        _exhibitions is null || _exhibitions.Count == 0 || _exhibitionsLastLoadedDate.Date < DateTime.UtcNow.Date;

    private Dictionary<int, ExhibitionDto> CreateExhibitions => new()
    {
        [1] = new ExhibitionDto("New Home", Colours.WarmAccentOrange, default),
        [7] = new ExhibitionDto("Atelier", Colours.DeepBlue, CreateAtelierArtworks)
    };

    private List<ArtworkDto> CreateAtelierArtworks(int floorNumber) => LinkArtworks(
    [
        NewArtwork("In my line of work everything is true and false.", 2025, 111, default, Dietmar, SizeType.Text),
        NewArtwork("Attempt #1", 2025, 112, (s, w, h) => _turtleGraphics.Turtle1(s, w, h, 8, 4, closePath: true), Dietmar),
        NewArtwork("Attempt #2", 2025, 113, (s, w, h) => _turtleGraphics.Turtle1Smooth(s, w, h, 8, 4, closePath : true), Dietmar),
        NewArtwork("Attempt #3", 2025, 114, (s, w, h) => _turtleGraphics.Turtle1(s, w, h, 16, 9), Dietmar),
        NewArtwork("Attempt #4", 2025, 115, (s, w, h) => _turtleGraphics.Turtle2(s, w, h, 13, 7), Dietmar),
        NewArtwork("Attempt #5", 2025, 116, (s, w, h) => _turtleGraphics.Turtle2(s, w, h, 32, 18, 1), Dietmar),
        NewArtwork("Attempt #6", 2025, 117, (s, w, h) => _turtleGraphics.Turtle1Smooth(s, w, h, 12, 6, closePath : true), Dietmar),
        NewArtwork("Attempt #7", 2025, 118, (s, w, h) => _fourColours.Pattern1(s, w, h, 10, 6, _colours.Blueish20Colours), Dietmar),
        NewArtwork("Attempt #8", 2025, 119, (s, w, h) => _fourColours.Pattern1(s, w, h, 21, 13, _colours.Warm20AccentColours), Dietmar),
        NewArtwork("Attempt #9", 2025, 120, (s, w, h) => _fourColours.Pattern1(s, w, h, 49, 37, _colours.MixedColoursBW), Dietmar),
        NewArtwork("Attempt #10", 2025, 121, (s, w, h) => _fourColours.Pattern2(s, w, h, 4, 4, _colours.Warm20AccentColours), Dietmar),
        NewArtwork("Attempt #11", 2025, 122, (s, w, h) => _fourColours.Pattern2(s, w, h, 8, 6, _colours.Blueish20Colours), Dietmar),
    ]);

    public GalleryService(ClientFactory http, AlgorithmFactory algorithmFactory, Colours colours)
    {
        _http = http;
        _colours = colours;
        _fourColours = algorithmFactory.GetAlgorithm(AlgorithmType.FourColours) as FourColours;
        _turtleGraphics = algorithmFactory.GetAlgorithm(AlgorithmType.TurtleGraphics) as TurtleGraphics;
        _exhibitions = CreateExhibitions;
    }

    public List<TodoDto> GetTodosAsync()
    {
        List<TodoDto> todos = [
            // https://learn.microsoft.com/en-us/azure/azure-functions/migrate-dotnet-to-isolated-model?tabs=net8
            new("Automatic register", TodoStatus.InProgress),
            new("My Collection", TodoStatus.InProgress),
            new("Subscribe to invitations", TodoStatus.InProgress),
            new("Center vistior point", TodoStatus.InProgress),
            new("Artwork: comments", TodoStatus.Planned),
            new("Artwork: buy", TodoStatus.Planned),
            new("Artwork: sell", TodoStatus.Planned),
            new("Cafe with payment and coffee as artworks", TodoStatus.Planned),
            new("Launch the SCHODER GALLERY", TodoStatus.Planned),
            new("Buy membership", TodoStatus.InProgress),

            new("Exhibition \"Tomato\"", TodoStatus.Planned),
            new("Exhibition \"Judgement Day\"", TodoStatus.Planned),
            new("Exhibition \"Self Portraits\"", TodoStatus.Planned),
            new("Exhibition \"Evolution\"", TodoStatus.Planned),
            new("Exhibition \"Find Me!\"", TodoStatus.Planned),
            new("Exhibition \"Morning Walk\"", TodoStatus.Planned),

            new("Exhibition \"Advent Calendar\"", TodoStatus.Planned),

            new("Exhibition \"New Home\"", TodoStatus.Planned),
            new("Exhibition \"Book: Civilized\"", TodoStatus.Planned),
            new("Exhibition \"Book: Innovation\"", TodoStatus.Planned),
            new("Exhibition \"Who Am I?\"", TodoStatus.Planned),
            new("Exhibition \"Book: Goettlich?\"", TodoStatus.Planned),
            new("Exhibition \"Music 2\"", TodoStatus.Planned),
            new("Exhibition \"Boris Bike\"", TodoStatus.Planned),
            new("Exhibition \"Hitler Eats Beigel\"", TodoStatus.Planned),
            new("Exhibition \"Digital 2\"", TodoStatus.Planned),
            new("Exhibition \"Cafe Weidinger\"", TodoStatus.Planned),
            new("Exhibition \"Giordano Bruno\"", TodoStatus.Planned),
            new("Exhibition \"Digital 1\"", TodoStatus.Planned),

            new("Make artworks clickable", TodoStatus.InProgress),
            new("Thumbnails", TodoStatus.Finished, 30, 10, 2025),
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
        var artworks = (await GetExhibitionArtworksAsync(floorNumber)).Artworks;
        var artwork = id > 0
            ? artworks.FirstOrDefault(a => a.Number == id)
            : null;

        return artwork ?? artworks.FirstOrDefault(a => a.PreviousId == -1);
    }

    public async Task<ExhibitionDto> GetExhibitionAsync(int floorNumber)
    {
        if (LoadExhibitionsNeeded)
        {
            await LoadExhibitionsFromBackendAsync();
        }

        return _exhibitions.TryGetValue(floorNumber, out var exhibition) ? exhibition : null;
    }

    public async Task<ExhibitionDto> GetExhibitionArtworksAsync(int floorNumber)
    {
        var exhibition = await GetExhibitionAsync(floorNumber);
        if (exhibition is null) { return null; }

        if (exhibition.LoadArtworksNeeded)
        {
            await LoadArtworksFromBackendAsync(exhibition);
            //var artworks = exhibition.ReadFromArtworksJson
            //    ? await GetArtworksFromJson(floorNumber)
            //    : exhibition.ArtworkFactory(floorNumber);
            //exhibition.Artworks.AddRange(LinkArtworks(artworks));
        }

        return exhibition;
    }

    public async Task<CheckoutDto> BuyArtworkAsync(Guid collectorId, ArtworkDto artwork)
    {
        var purchaseDto = new PurchaseDto(collectorId, artwork.Id, null);
        var response = await _http.Backend.PostAsJsonAsync("/api/checkouts", purchaseDto);
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<CheckoutDto>()
            : new CheckoutDto(default, "Checkout failed.");
    }

    //private async Task<List<ArtworkDto>> GetArtworksFromJson(int floorNumber) =>
    //    await _http.Frontend.GetFromJsonAsync<List<ArtworkDto>>(ArtworksJson(floorNumber));

    //private static string ArtworksJson(int floorNumber) =>
    //    $"images/floor{floorNumber}/_artworks.json";

    private static ArtworkDto NewArtwork(
        string title,
        int year,
        int id,
        Func<ISettings, int, int, ArtworkType> renderAlgorithm,
        string artist,
        SizeType sizeType = SizeType.Dynamic,
        int fixedWidth = 0, int fixedHeight = 0)
        => new(title, year, renderAlgorithm, sizeType, fixedWidth, fixedHeight, artist, id);

    private async Task LoadExhibitionsFromBackendAsync()
    {
        var response = await _http.Backend.GetAsync("/api/exhibitions");

        if (!response.IsSuccessStatusCode)
        {
            //alert
            return;
        }

        var exhibitions = await response.Content.ReadFromJsonAsync<List<ExhibitionDto>>();
        _exhibitions = exhibitions.ToDictionary(e => e.Floor, e => e);
        _exhibitionsLastLoadedDate = DateTime.UtcNow.Date;
    }

    private async Task LoadArtworksFromBackendAsync(ExhibitionDto exhibition)
    {
        var response = await _http.Backend.GetAsync($"/api/exhibitions/{exhibition.Id}/artworks");

        if (!response.IsSuccessStatusCode)
        {
            //alert
            return;
        }

        exhibition.ArtworksLastLoadedDateTime = DateTime.UtcNow;
        var artworks = await response.Content.ReadFromJsonAsync<List<ArtworkDto>>();
        exhibition.Artworks = LinkArtworks(artworks);
    }

    private static List<ArtworkDto> LinkArtworks(List<ArtworkDto> artworks)
    {
        if (artworks.Count == 0) { return artworks; }

        artworks = [.. artworks.OrderBy(a => a.Number)];
        artworks.First().PreviousId = -1;
        artworks.Last().NextId = -1;

        for (int j = 0; j < artworks.Count - 1; j++)
        {
            artworks[j].NextId = artworks[j + 1].Number;
            artworks[j + 1].PreviousId = artworks[j].Number;
        }

        return artworks;
    }
}
