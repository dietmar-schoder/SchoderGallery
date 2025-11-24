using SchoderGallery.DTOs;

namespace SchoderGallery.Navigation;

public class Visitor
{
    public Guid Id { get; set; }

    public LocaleDto Locale { get; set; }

    public FloorType CurrentFloorType { get; set; }

    public Dictionary<FloorType, Guid> LatestArtworkIds { get; set; }

    public static Visitor Create() =>
        Create(Guid.NewGuid());

    public static Visitor Create(Guid id) => new()
    {
        Id = id,
        CurrentFloorType = FloorType.GroundFloor,
        LatestArtworkIds = new Dictionary<FloorType, Guid>
        {
            { FloorType.Cafe, Guid.Empty },
            { FloorType.Info, Guid.Empty },
            { FloorType.Shop, Guid.Empty },
            { FloorType.Toilets, Guid.Empty },

            { FloorType.Atelier, Guid.Empty },
            { FloorType.Floor6, Guid.Empty },
            { FloorType.Floor5, Guid.Empty },
            { FloorType.Floor4, Guid.Empty },
            { FloorType.Floor3, Guid.Empty },
            { FloorType.Floor2, Guid.Empty },
            { FloorType.Floor1, Guid.Empty },
            { FloorType.Basement1, Guid.Empty },
            { FloorType.Basement2, Guid.Empty },
            { FloorType.Depot, Guid.Empty },
            { FloorType.MyCollection, Guid.Empty }
        }
    };

    public void MoveToFloor(FloorType newFloor) =>
        CurrentFloorType = newFloor;

    public Guid GetLatestArtworkId(FloorType floorType)
    {
        if (LatestArtworkIds.TryGetValue(floorType, out var artworkId))
        {
            return artworkId;
        }
        InitArtworkIds();
        return Guid.Empty;
    }

    public void ViewArtwork(FloorType floorType, Guid artworkId)
    {
        GetLatestArtworkId(floorType);
        LatestArtworkIds[floorType] = artworkId;
    }

    private void InitArtworkIds() =>
        LatestArtworkIds = new Dictionary<FloorType, Guid>
        {
            { FloorType.Cafe, Guid.Empty },
            { FloorType.Info, Guid.Empty },
            { FloorType.Shop, Guid.Empty },
            { FloorType.Toilets, Guid.Empty },

            { FloorType.Atelier, Guid.Empty },
            { FloorType.Floor6, Guid.Empty },
            { FloorType.Floor5, Guid.Empty },
            { FloorType.Floor4, Guid.Empty },
            { FloorType.Floor3, Guid.Empty },
            { FloorType.Floor2, Guid.Empty },
            { FloorType.Floor1, Guid.Empty },
            { FloorType.Basement1, Guid.Empty },
            { FloorType.Basement2, Guid.Empty },
            { FloorType.Depot, Guid.Empty },
            { FloorType.MyCollection, Guid.Empty }
        };
}
