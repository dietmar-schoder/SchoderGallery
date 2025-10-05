using SchoderGallery.Builders;

namespace SchoderGallery.Navigation;

public class Visitor
{
    public Guid Id { get; } = Guid.NewGuid();

    public BuilderType CurrentFloorType { get; private set; }

    public Visitor() =>
        CurrentFloorType = BuilderType.GroundFloor;

    public void MoveToFloor(BuilderType newFloor) =>
        CurrentFloorType = newFloor;
}