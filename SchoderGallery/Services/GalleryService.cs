using SchoderGallery.DTOs;
using SchoderGallery.Navigation;
using SchoderGallery.Painters;
using SchoderGalleryServer.DTOs;
using System.Net.Http.Json;

namespace SchoderGallery.Services;

//        // https://learn.microsoft.com/en-us/azure/azure-functions/migrate-dotnet-to-isolated-model?tabs=net8
//        new("Automatic register", TodoStatus.InProgress),
//        new("Subscribe to invitations", TodoStatus.InProgress),
//        new("Center vistior point", TodoStatus.InProgress),
//        new("Artwork: comments", TodoStatus.Planned),
//        new("Artwork: sell", TodoStatus.Planned),
//        new("Cafe with payment and coffee as artworks", TodoStatus.Planned),
//        new("Launch the SCHODER GALLERY", TodoStatus.Planned),
//        new("Buy membership", TodoStatus.InProgress),

//        new("Exhibition \"Tomato\"", TodoStatus.Planned),
//        new("Exhibition \"Judgement Day\"", TodoStatus.Planned),
//        new("Exhibition \"Self Portraits\"", TodoStatus.Planned),
//        new("Exhibition \"Evolution\"", TodoStatus.Planned),
//        new("Exhibition \"Find Me!\"", TodoStatus.Planned),
//        new("Exhibition \"Morning Walk\"", TodoStatus.Planned),

//        new("Exhibition \"Advent Calendar\"", TodoStatus.Planned),

//        new("Exhibition \"New Home\"", TodoStatus.Planned),
//        new("Exhibition \"Book: Civilized\"", TodoStatus.Planned),
//        new("Exhibition \"Book: Innovation\"", TodoStatus.Planned),
//        new("Exhibition \"Who Am I?\"", TodoStatus.Planned),
//        new("Exhibition \"Book: Goettlich?\"", TodoStatus.Planned),
//        new("Exhibition \"Music 2\"", TodoStatus.Planned),
//        new("Exhibition \"Boris Bike\"", TodoStatus.Planned),
//        new("Exhibition \"Hitler Eats Beigel\"", TodoStatus.Planned),
//        new("Exhibition \"Digital 2\"", TodoStatus.Planned),
//        new("Exhibition \"Cafe Weidinger\"", TodoStatus.Planned),
//        new("Exhibition \"Giordano Bruno\"", TodoStatus.Planned),
//        new("Exhibition \"Digital 1\"", TodoStatus.Planned),

public class GalleryService(ClientFactory http)
{
    private readonly ClientFactory _http = http;
    //private readonly Colours _colours = colours;
    //private readonly FourColours _fourColours = algorithmFactory.GetAlgorithm(AlgorithmType.FourColours) as FourColours;
    //private readonly TurtleGraphics _turtleGraphics = algorithmFactory.GetAlgorithm(AlgorithmType.TurtleGraphics) as TurtleGraphics;

    private Dictionary<int, ExhibitionDto> _exhibitions;
    private DateTime _exhibitionsLastLoadedDate = DateTime.MinValue;

    private bool LoadExhibitionsNeeded =>
        _exhibitions is null || _exhibitions.Count == 0 || _exhibitionsLastLoadedDate.Date < DateTime.UtcNow.Date;

    public async Task<ArtworkDto> GetArtworkAsync(Visitor collector, int floorNumber, Guid id)
    {
        var artworks = (await GetExhibitionArtworksAsync(collector, floorNumber)).Artworks;
        var artwork = id == Guid.Empty
            ? null
            : artworks.FirstOrDefault(a => a.Id == id);

        return artwork ?? artworks.FirstOrDefault(a => a.PreviousId == Guid.Empty);
    }

    public async Task<ExhibitionDto> GetExhibitionAsync(int floorNumber)
    {
        await LoadExhibitionsAsync();
        return _exhibitions.TryGetValue(floorNumber, out var exhibition) ? exhibition : null;
    }

    public async Task LoadExhibitionsAsync()
    {
        if (LoadExhibitionsNeeded)
        {
            await LoadExhibitionsFromBackendAsync();
        }
    }

    public async Task<ExhibitionDto> GetExhibitionArtworksAsync(Visitor collector, int floorNumber)
    {
        var exhibition = await GetExhibitionAsync(floorNumber);
        if (exhibition is null) { return null; }

        if (exhibition.LoadArtworksNeeded)
        {
            await LoadArtworksFromBackendAsync(collector, floorNumber, exhibition);
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

    public async Task CancelCheckoutAsync(Guid collectorId, Guid artworkId)
    {
        var purchaseDto = new PurchaseDto(collectorId, artworkId, null);
        await _http.Backend.PostAsJsonAsync("/api/checkouts/cancel", purchaseDto);
    }

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

    private async Task LoadArtworksFromBackendAsync(Visitor collector, int floorNumber, ExhibitionDto exhibition)
    {
        var response = floorNumber == (int)FloorType.MyCollection
            ? await MyCollectionArtworksFromBackendAsync(collector.Id)
            : await ExhibitionArtworksFromBackendAsync(exhibition.Id, collector.Locale.CountryIsoCode);

        if (!response.IsSuccessStatusCode)
        {
            //alert
            return;
        }

        exhibition.ArtworksLastLoadedDateTime = DateTime.UtcNow;
        var artworks = await response.Content.ReadFromJsonAsync<List<ArtworkDto>>();
        exhibition.Artworks = LinkArtworks(artworks);
    }

    private async Task<HttpResponseMessage> MyCollectionArtworksFromBackendAsync(Guid collectorId) =>
        await _http.Backend.GetAsync($"/api/collectors/{collectorId}/artworks");

    private async Task<HttpResponseMessage> ExhibitionArtworksFromBackendAsync(Guid exhibitionId, string country) =>
        await _http.Backend.GetAsync($"/api/exhibitions/{exhibitionId}/artworks?loc={country}");

    private static List<ArtworkDto> LinkArtworks(List<ArtworkDto> artworks)
    {
        if (artworks.Count == 0) { return artworks; }

        artworks = [.. artworks.OrderBy(a => a.Number)];
        artworks.First().PreviousId = Guid.Empty;
        artworks.Last().NextId = Guid.Empty;

        for (int j = 0; j < artworks.Count - 1; j++)
        {
            artworks[j].NextId = artworks[j + 1].Id;
            artworks[j + 1].PreviousId = artworks[j].Id;
        }

        return artworks;
    }
}
