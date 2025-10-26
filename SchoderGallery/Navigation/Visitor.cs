using SchoderGallery.DTOs;

namespace SchoderGallery.Navigation;

public class Visitor
{
    public Guid Id { get; } = Guid.NewGuid();

    public FloorType CurrentFloorType { get; private set; }

    private Dictionary<FloorType, int> LatestArtworkIds { get; } = new Dictionary<FloorType, int>()
    {
        { FloorType.Atelier, 0 },
        { FloorType.Floor6, 0 },
        { FloorType.Floor5, 0 },
        { FloorType.Floor4, 0 },
        { FloorType.Floor3, 0 },
        { FloorType.Floor2, 0 },
        { FloorType.Floor1, 0 },
        { FloorType.Basement1, 0 },
        { FloorType.Basement2, 0 },
        { FloorType.Depot, 0 },
    };

    public LocaleDto Locale { get; set; }

    public Visitor() =>
        CurrentFloorType = FloorType.GroundFloor;

    public void MoveToFloor(FloorType newFloor) =>
        CurrentFloorType = newFloor;

    public int LatestArtworkId(FloorType floorType) =>
        LatestArtworkIds[floorType];

    public void ViewArtwork(FloorType floorType, int artworkId) =>
        LatestArtworkIds[floorType] = artworkId;
}
