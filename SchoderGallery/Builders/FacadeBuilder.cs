using SchoderGallery.Constants;

namespace SchoderGallery.Builders;

public class FacadeBuilder(IConstantsFactory constantsFactory)
    : BaseBuilder(constantsFactory), IFacadeBuilder
{
    protected override void Draw()
    {
        int x = 0;
        int y = 0;

        int columns = _constants.NumberOfWindowsPerFloor;
        int gap = (int)(_width / _constants.NumberOfWindowsPerFloor * _constants.GapToColumnWidthRatio);
        int margin = (int)(gap * _constants.WindowMarginToGapRatio);

        int availableWidthForWindows = _width - (2 * margin) - ((columns - 1) * gap);
        int windowWidth = availableWidthForWindows / columns;
        int remainder = availableWidthForWindows - (windowWidth * columns);
        int rightOuterBorder = Math.Max(2, remainder);

        _width -= rightOuterBorder;

        int windowHeight = (int)(windowWidth * _constants.WindowHeightToWidthRatio);

        int windowY = y + margin;

        Svg($"<rect x='0' y='0' width='{_width}' height='{_height}' fill='{_constants.LightGray}' stroke='none' />");

        for (int i = 0; i < columns; i++)
        {
            int posX = x + margin + i * (windowWidth + gap);
            Svg($"<rect x='{posX}' y='{windowY}' width='{windowWidth}' height='{windowHeight}' fill='blue' />");
        }
    }
}