using SchoderGallery.Builders;

namespace SchoderGallery.Navigation;

public record FloorInfo(BuilderType FloorType, int LiftColumn, int LiftRow, int LiftButtonCaption, string LiftLabel, string Page);