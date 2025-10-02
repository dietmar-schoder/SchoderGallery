using SchoderGallery.Constants;

namespace SchoderGallery.Builders;

public class FacadeBuilder(IConstantsFactory constantsFactory)
    : BaseBuilder(constantsFactory), IFacadeBuilder
{
    protected override void Draw()
    {
        int columns = _constants.RowsColumns;
        int gap = (int)((ScreenMode == ScreenMode.Landscape ? _height : _width) / columns * _constants.GapToRowColumnWidthRatio);
        int margin = (int)(gap * _constants.WindowMarginToGapRatio);

        int windowWidth = WindowWidth();
        int windowHeight = WindowHeight();

        Svg($"<rect x='0' y='0' width='{_width}' height='{_height}' fill='{_constants.LightGray}' stroke='none' />");

        for (int y = 0; y < columns; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                int posX = margin + x * (windowWidth + gap);
                int posY = margin + y * (windowHeight + gap);
                Svg($"<rect x='{posX}' y='{posY}' width='{windowWidth}' height='{windowHeight}' fill='blue' />");
            }
        }

        int WindowWidth()
        {
            int availableWidthForWindows = _width - (2 * margin) - ((columns - 1) * gap);
            int windowWidth = availableWidthForWindows / columns;
            int remainder = availableWidthForWindows - (windowWidth * columns);
            int rightOuterBorder = remainder;
            _width -= rightOuterBorder;
            return windowWidth;
        }

        int WindowHeight()
        {
            int availableHeightForWindows = _height - (2 * margin) - ((columns - 1) * gap);
            int windowHeight = availableHeightForWindows / columns;
            int remainder = availableHeightForWindows - (windowHeight * columns);
            int rightOuterBorder = remainder;
            _height -= rightOuterBorder;
            return windowHeight;
        }
    }
}