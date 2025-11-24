namespace SchoderGallery.Navigation;

public record FloorInfo(FloorType FloorType, int LiftColumn, int LiftRow, string LiftLabel, string Page)
{
    public bool IsLiftFloor =>
        LiftColumn > -1;

    public int FloorNumber =>
        (int)FloorType;

    public bool IsArtworksFloor =>
        (IsLiftFloor || IsGroundFloorRoom)
        && FloorType != FloorType.GroundFloor;

    public bool SaveVisitorFloor =>
        IsLiftFloor || IsGroundFloorRoom;

    public bool IsGroundFloorRoom =>
        FloorType == FloorType.Cafe
        || FloorType == FloorType.Shop
        || FloorType == FloorType.Toilets
        || FloorType == FloorType.Info;

    public bool IsLeftGroundFloorRoom =>
        FloorType == FloorType.Cafe
        || FloorType == FloorType.Toilets;

    public bool IsRightGroundFloorRoom =>
        FloorType == FloorType.Shop
        || FloorType == FloorType.Info;

    public bool HasFloorParam =>
        IsLiftFloor
        && FloorType != FloorType.Atelier
        && FloorType != FloorType.GroundFloor
        && FloorType != FloorType.Depot
        && FloorType != FloorType.MyCollection;

    public string PageAndParam() =>
        HasFloorParam ? $"{Page}/{(int)FloorType}" : Page;
}
