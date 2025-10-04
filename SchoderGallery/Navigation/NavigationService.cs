using SchoderGallery.Builders;

namespace SchoderGallery.Navigation;

public class NavigationService
{
    private Visitor _visitor;
    
    public IReadOnlyDictionary<BuilderType, FloorInfo> Floors { get; } = new Dictionary<BuilderType, FloorInfo>
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
        { BuilderType.SiteManagement, new FloorInfo(BuilderType.SiteManagement, 1, 5, "Site Management", "/SiteManagement") },

        { BuilderType.Lift, new FloorInfo(BuilderType.Lift, -1, -1, "Lift", "/Lift") }
    };

    public IEnumerable<FloorInfo> GetFloors() => Floors.Values.Where(f => f.LiftColumn > -1);

    public FloorInfo GetFloor(BuilderType type) => Floors.TryGetValue(type, out var floor) ? floor : null;

    public BuilderType GetBuilderType(int floorNumber) =>
        Enum.TryParse<BuilderType>(floorNumber.ToString(), out var result) ? result : BuilderType.GroundFloor;

    public void SetVisitorFloor(BuilderType floor)
    {
        EnsureVisitor();
        if (GetFloor(floor)?.LiftColumn > -1)
        {
            _visitor.MoveToFloor(floor);
        }
    }

    public BuilderType GetVisitorFloor()
    {
        EnsureVisitor();
        return _visitor.CurrentFloor;
    }

    private void EnsureVisitor() => _visitor ??= new Visitor();
}