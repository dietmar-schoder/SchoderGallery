namespace SchoderGallery.Navigation;

public record FloorInfo(FloorType FloorType, int LiftColumn, int LiftRow, string LiftLabel, string Page)
{
    public bool IsFloor =>
        LiftColumn > -1;

    public int FloorNumber =>
        (int)FloorType;

    public bool IsArtworksFloor =>
        IsFloor
        && FloorType != FloorType.GroundFloor
        && FloorType != FloorType.MyCollection;

    public bool HasFloorParam =>
        IsFloor
        && FloorType != FloorType.Atelier
        && FloorType != FloorType.GroundFloor
        && FloorType != FloorType.Depot
        && FloorType != FloorType.MyCollection;

    public string PageAndParam() =>
        HasFloorParam ? $"{Page}/{(int)FloorType}" : Page;
}