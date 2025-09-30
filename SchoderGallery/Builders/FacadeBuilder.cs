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
        return $"<svg width='{width}' height='{height}'>{Windows(constants, width)}</svg>";
    }

    private static string Windows(IConstants constants, int totalWidth)
    {
        var windowsSvg = new StringBuilder();
        int n = constants.NumberOfWindowsPerFloor;
        int windowWidth = 8;
        int windowHeight = 8;
        int y = 10;

        double spacing = (double)(totalWidth - windowWidth) / (n - 1);
        for (int i = 0; i < n; i++)
        {
            double x = i * spacing;
            windowsSvg.Append($"<rect x='{x:F2}' y='{y}' width='{windowWidth}' height='{windowHeight}' fill='blue' />");
        }
        return windowsSvg.ToString();
    }
}