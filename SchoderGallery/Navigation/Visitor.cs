using SchoderGallery.Builders;
using SchoderGallery.DTOs;

namespace SchoderGallery.Navigation;

public class Visitor
{
    public Guid Id { get; } = Guid.NewGuid();

    public BuilderType CurrentFloorType { get; private set; }

    public int LatestArtworkId { get; private set; }

    public LocaleDto Locale { get; set; }

    public Visitor() =>
        CurrentFloorType = BuilderType.GroundFloor;

    public void MoveToFloor(BuilderType newFloor) =>
        CurrentFloorType = newFloor;

    public void ViewArtwork(int artworkId) =>
        LatestArtworkId = artworkId;
}
