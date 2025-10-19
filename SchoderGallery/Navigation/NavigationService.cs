using SchoderGallery.Builders;
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

    private readonly IReadOnlyDictionary<BuilderType, FloorInfo> _floors = new Dictionary<BuilderType, FloorInfo>
    {
        { BuilderType.Atelier, new FloorInfo(BuilderType.Atelier, 1, 0, "Atelier", "/Atelier") },
        { BuilderType.Floor6, new FloorInfo(BuilderType.Floor6, 0, 0, "Floor 6", "/Floor") },
        { BuilderType.Floor5, new FloorInfo(BuilderType.Floor5, 1, 1, "Floor 5", "/Floor") },
        { BuilderType.Floor4, new FloorInfo(BuilderType.Floor4, 0, 1, "Floor 4", "/Floor") },
        { BuilderType.Floor3, new FloorInfo(BuilderType.Floor3, 1, 2, "Floor 3", "/Floor") },
        { BuilderType.Floor2, new FloorInfo(BuilderType.Floor2, 0, 2, "Floor 2", "/Floor") },
        { BuilderType.Floor1, new FloorInfo(BuilderType.Floor1, 1, 3, "Floor 1", "/Floor") },
        { BuilderType.GroundFloor, new FloorInfo(BuilderType.GroundFloor, 0, 3, "Ground Floor", "/GroundFloor") },
        { BuilderType.Basement1, new FloorInfo(BuilderType.Basement1, 0, 4, "Basement 1", "/Basement") },
        { BuilderType.Basement2, new FloorInfo(BuilderType.Basement2, 1, 4, "Basement 2", "/Basement") },
        { BuilderType.Depot, new FloorInfo(BuilderType.Depot, 0, 5, "Depot", "/Depot") },
        { BuilderType.Operations, new FloorInfo(BuilderType.Operations, 1, 5, "Operations", "/Operations") },

        { BuilderType.Lift, new FloorInfo(BuilderType.Lift, -1, -1, "Lift", "/Lift") }
    };

    public IEnumerable<FloorInfo> GetFloors() =>
        _floors.Values.Where(f => f.LiftColumn > -1);

    public FloorInfo GetFloor(BuilderType type) =>
        _floors.TryGetValue(type, out var floor) ? floor : _floors[BuilderType.GroundFloor];

    public FloorInfo GetFloor(int floorNumber) =>
        GetFloor(GetBuilderType(floorNumber));

    public FloorInfo GetFloor(string floorNumberString) =>
        GetFloor(GetBuilderType(int.TryParse(floorNumberString, out var floorNumber) ? floorNumber : 0));

    public async Task<LocaleDto> GetVisitorLocaleAsync() =>
        (await GetInitVisitorAsync()).Locale;

    public async Task<FloorInfo> GetVisitorFloorAsync() =>
        GetFloor(await GetVisitorFloorTypeAsync());

    public BuilderType GetBuilderType(int floorNumber) =>
        Enum.TryParse<BuilderType>(floorNumber.ToString(), out var result) ? result : BuilderType.GroundFloor;

    public async Task<BuilderType> GetVisitorFloorTypeAsync() =>
        (await GetInitVisitorAsync()).CurrentFloorType;

    public async Task SetVisitorFloorAsync(BuilderType floor)
    {
        await GetInitVisitorAsync();
        if (GetFloor(floor)?.IsFloor ?? false)
        {
            _visitor.MoveToFloor(floor);
        }
    }

    public int GetArtworkIdOrLatestArtworkId(int artworkId) =>
        artworkId < 1 ? _visitor.LatestArtworkId : artworkId;

    public ArtworkDto GetLatestFloorArtwork(ExhibitionDto exhibition) =>
        _visitor.LatestArtworkId > 0
            ? exhibition.Artworks.FirstOrDefault(a => a.Id == _visitor.LatestArtworkId)
            : null;

    public void SetLatestArtworkId(int artworkId) =>
        _visitor.ViewArtwork(artworkId);
}