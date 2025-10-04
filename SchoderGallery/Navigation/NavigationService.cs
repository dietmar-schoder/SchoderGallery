using SchoderGallery.Builders;

namespace SchoderGallery.Navigation;

public class NavigationService
{
    public IReadOnlyDictionary<BuilderType, FloorInfo> Floors { get; } = new Dictionary<BuilderType, FloorInfo>
    {
        { BuilderType.Atelier, new FloorInfo(BuilderType.Atelier, 1, 0, 7, "Atelier", "/Atelier") },
        { BuilderType.Floor6, new FloorInfo(BuilderType.Floor6, 0, 0, 6, "Floor 6", "/Floor6") },
        { BuilderType.Floor5, new FloorInfo(BuilderType.Floor5, 1, 1, 5, "Floor 5", "/Floor5") },
        { BuilderType.Floor4, new FloorInfo(BuilderType.Floor4, 0, 1, 4, "Floor 4", "/Floor4") },
        { BuilderType.Floor3, new FloorInfo(BuilderType.Floor3, 1, 2, 3, "Floor 3", "/Floor3") },
        { BuilderType.Floor2, new FloorInfo(BuilderType.Floor2, 0, 2, 2, "Floor 2", "/Floor2") },
        { BuilderType.Floor1, new FloorInfo(BuilderType.Floor1, 1, 3, 1, "Floor 1", "/Floor1") },
        { BuilderType.GroundFloor, new FloorInfo(BuilderType.GroundFloor, 0, 3, 0, "Ground Floor", "/GroundFloor") },
        { BuilderType.Basement1, new FloorInfo(BuilderType.Basement1, 0, 4, -1, "Basement 1", "/Basement1") },
        { BuilderType.Basement2, new FloorInfo(BuilderType.Basement2, 1, 4, -2, "Basement 2", "/Basement2") },
        { BuilderType.Depot, new FloorInfo(BuilderType.Depot, 0, 5, -3, "Depot", "/Depot") },
        { BuilderType.SiteManagement, new FloorInfo(BuilderType.SiteManagement, 1, 5, -4, "Site Management", "/SiteManagement") }
    };

    public IEnumerable<FloorInfo> GetFloors() => Floors.Values;
}