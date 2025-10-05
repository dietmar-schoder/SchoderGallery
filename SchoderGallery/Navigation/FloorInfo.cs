using SchoderGallery.Builders;

namespace SchoderGallery.Navigation;

public record FloorInfo(BuilderType FloorType, int LiftColumn, int LiftRow, string LiftLabel, string Page)
{
    public bool IsFloor =>
        LiftColumn > -1;

    // Replace with record field IsArtworksFloor
    public bool IsArtworksFloor =>
        IsFloor
        && FloorType != BuilderType.Atelier
        && FloorType != BuilderType.GroundFloor
        && FloorType != BuilderType.Depot
        && FloorType != BuilderType.Operations;

    public string PageAndParam() =>
        IsArtworksFloor ? $"{Page}/{(int)FloorType}" : Page;
}