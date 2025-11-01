using SchoderGallery.DTOs;

namespace SchoderGallery.Navigation;

public class Visitor
{
    public Guid Id { get; set; }

    public LocaleDto Locale { get; set; }

    public FloorType CurrentFloorType { get; set; }

    public Dictionary<FloorType, int> LatestArtworkIds { get; set; }

    public static Visitor Create() => new()
    {
        Id = Guid.NewGuid(),
        CurrentFloorType = FloorType.GroundFloor,
        LatestArtworkIds = new Dictionary<FloorType, int>
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
        }
    };

    public void MoveToFloor(FloorType newFloor) =>
        CurrentFloorType = newFloor;

    public int LatestArtworkId(FloorType floorType) =>
        LatestArtworkIds[floorType];

    public void ViewArtwork(FloorType floorType, int artworkId) =>
        LatestArtworkIds[floorType] = artworkId;
}
