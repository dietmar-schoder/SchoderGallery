using SchoderGallery.Builders;

namespace SchoderGallery.Navigation;

public record FloorInfo(BuilderType FloorType, int LiftColumn, int LiftRow, string LiftLabel, string Page)
{
    public bool IsFloor =>
        LiftColumn > -1;

    public int FloorNumber =>
        (int)FloorType;

    public bool IsArtworksFloor =>
        IsFloor
        && FloorType != BuilderType.GroundFloor
        && FloorType != BuilderType.Operations;

    public bool HasFloorParam =>
        IsFloor
        && FloorType != BuilderType.Atelier
        && FloorType != BuilderType.GroundFloor
        && FloorType != BuilderType.Depot
        && FloorType != BuilderType.Operations;

    public string PageAndParam() =>
        HasFloorParam ? $"{Page}/{(int)FloorType}" : Page;
}