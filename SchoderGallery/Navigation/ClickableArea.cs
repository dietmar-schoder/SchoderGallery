namespace SchoderGallery.Navigation;

public record ClickableArea(int X, int Y, int Width, int Height, string Page = default, string Tooltip = default, bool ReRender = false);