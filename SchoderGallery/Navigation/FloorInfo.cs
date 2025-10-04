using SchoderGallery.Builders;

namespace SchoderGallery.Navigation;

public record FloorInfo(BuilderType FloorType, int LiftColumn, int LiftRow, string LiftLabel, string Page);