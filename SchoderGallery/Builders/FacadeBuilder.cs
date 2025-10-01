using SchoderGallery.Constants;
using System.Text;

namespace SchoderGallery.Builders;

public interface IFacadeBuilder
{
    string GetSvg(int width, int height);
}

public class FacadeBuilder(IConstantsFactory constantsFactory) : IFacadeBuilder
{
    public string GetSvg(int width, int height)
    {
        var screenMode = width > height ? ScreenMode.Landscape : ScreenMode.Portrait;
        var constants = constantsFactory.GetConstants(screenMode);
        return $"<svg width='{width - 8}' height='{height - 8}'>{DrawBuilding(constants, width - 8, height - 8)}</svg>";
    }

    private static string DrawBuilding(IConstants constants, int totalWidth, int totalHeight)
    {
        int x = 0;
        int y = 0;
        var svg = new StringBuilder();

        int columns = constants.NumberOfWindowsPerFloor;

        int gap = (int)(totalWidth / constants.NumberOfWindowsPerFloor * constants.GapToColumnWidthRatio);
        int margin = (int)(gap * constants.WindowMarginToGapRatio);

        int availableWidthForWindows = totalWidth - (2 * margin) - ((columns - 1) * gap);
        int windowWidth = availableWidthForWindows / columns;
        int remainder = availableWidthForWindows - (windowWidth * columns);
        int rightOuterBorder = Math.Max(2, remainder);

        int buildingWidth = totalWidth - (x + rightOuterBorder);
        int buildingHeight = totalHeight;

        int windowHeight = (int)(windowWidth * constants.WindowHeightToWidthRatio);

        int windowY = y + margin;

        svg.Append(
            $"<rect x='{x}' y='{y}' width='{buildingWidth}' height='{buildingHeight}' " +
            $"fill='none' stroke='black' stroke-width='1' />"
        );
        
        for (int i = 0; i < columns; i++)
        {
            int posX = x + margin + i * (windowWidth + gap);
            svg.Append($"<rect x='{posX}' y='{windowY}' width='{windowWidth}' height='{windowHeight}' fill='blue' />");
        }

        return svg.ToString();
    }
}
