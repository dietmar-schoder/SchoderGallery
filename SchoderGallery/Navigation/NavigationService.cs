using SchoderGallery.DTOs;
using SchoderGallery.Services;
using System.Net.Http.Json;

namespace SchoderGallery.Navigation;

public class NavigationService(ClientFactory http)
{
    private Visitor _visitor;

    public async Task<Visitor> GetInitVisitorAsync()
    {
        _visitor ??= new Visitor
        {
            Locale = await http.Backend.GetFromJsonAsync<LocaleDto>("/api/countries")
        };

        if (_visitor.Locale is null)
        {
            // Alert (and later disallow use of the app)
        }

        return _visitor;
    }

    private readonly IReadOnlyDictionary<FloorType, FloorInfo> _floors = new Dictionary<FloorType, FloorInfo>
    {
        { FloorType.Atelier, new FloorInfo(FloorType.Atelier, 1, 0, "Atelier", "/Atelier") },
        { FloorType.Floor6, new FloorInfo(FloorType.Floor6, 0, 0, "Floor 6", "/Floor") },
        { FloorType.Floor5, new FloorInfo(FloorType.Floor5, 1, 1, "Floor 5", "/Floor") },
        { FloorType.Floor4, new FloorInfo(FloorType.Floor4, 0, 1, "Floor 4", "/Floor") },
        { FloorType.Floor3, new FloorInfo(FloorType.Floor3, 1, 2, "Floor 3", "/Floor") },
        { FloorType.Floor2, new FloorInfo(FloorType.Floor2, 0, 2, "Floor 2", "/Floor") },
        { FloorType.Floor1, new FloorInfo(FloorType.Floor1, 1, 3, "Floor 1", "/Floor") },
        { FloorType.GroundFloor, new FloorInfo(FloorType.GroundFloor, 0, 3, "Ground Floor", "/GroundFloor") },
        { FloorType.Basement1, new FloorInfo(FloorType.Basement1, 0, 4, "Basement 1", "/Basement") },
        { FloorType.Basement2, new FloorInfo(FloorType.Basement2, 1, 4, "Basement 2", "/Basement") },
        { FloorType.Depot, new FloorInfo(FloorType.Depot, 0, 5, "Depot", "/Depot") },
        { FloorType.Operations, new FloorInfo(FloorType.Operations, 1, 5, "Operations", "/Operations") },

        { FloorType.Lift, new FloorInfo(FloorType.Lift, -1, -1, "Lift", "/Lift") }
    };

    public IEnumerable<FloorInfo> GetFloors() =>
        _floors.Values.Where(f => f.LiftColumn > -1);

    public FloorInfo GetFloor(FloorType type) =>
        _floors.TryGetValue(type, out var floor) ? floor : _floors[FloorType.GroundFloor];

    public FloorInfo GetFloor(int floorNumber) =>
        GetFloor(GetBuilderType(floorNumber));

    public FloorInfo GetFloor(string floorNumberString) =>
        GetFloor(GetBuilderType(int.TryParse(floorNumberString, out var floorNumber) ? floorNumber : 0));

    public async Task<LocaleDto> GetVisitorLocaleAsync() =>
        (await GetInitVisitorAsync()).Locale;

    public async Task<FloorInfo> GetVisitorFloorAsync() =>
        GetFloor(await GetVisitorFloorTypeAsync());

    public FloorType GetBuilderType(int floorNumber) =>
        Enum.TryParse<FloorType>(floorNumber.ToString(), out var result) ? result : FloorType.GroundFloor;

    public async Task<FloorType> GetVisitorFloorTypeAsync() =>
        (await GetInitVisitorAsync()).CurrentFloorType;

    public async Task SetVisitorFloorAsync(FloorType floor)
    {
        await GetInitVisitorAsync();
        if (GetFloor(floor)?.IsFloor ?? false)
        {
            _visitor.MoveToFloor(floor);
        }
    }

    public int GetArtworkIdOrLatestArtworkId(FloorType floorType, int artworkId) =>
        artworkId < 1 ? _visitor.LatestArtworkId(floorType) : artworkId;

    public ArtworkDto GetLatestFloorArtwork(FloorType floorType, ExhibitionDto exhibition)
    {
        var latestArtworkId = _visitor.LatestArtworkId(floorType);
        return latestArtworkId > 0
            ? exhibition.Artworks.FirstOrDefault(a => a.Id == latestArtworkId)
            : null;
    }

    public void SetLatestArtworkId(FloorType floorType, int artworkId) =>
        _visitor.ViewArtwork(floorType, artworkId);
}