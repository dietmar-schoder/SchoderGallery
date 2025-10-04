using SchoderGallery.Builders;

namespace SchoderGallery.Navigation;

public class Visitor
{
    public Guid Id { get; } = Guid.NewGuid();

    public BuilderType CurrentFloor { get; private set; }

    public Visitor()
    {
        CurrentFloor = BuilderType.GroundFloor;
    }

    public void MoveToFloor(BuilderType newFloor)
    {
        CurrentFloor = newFloor;
    }
}