using SchoderGallery.DTOs;
using SchoderGallery.Painters;
using SchoderGallery.Settings;

namespace SchoderGallery.Algorithms;

public class Image(SvgPainter svgPainter) : IAlgorithm
{
    public AlgorithmType AlgorithmType => AlgorithmType.Image;

    public ArtworkType Jpg(ISettings settings, int width, int height, string filename)
    {
        svgPainter.Image(width, height, $"/images/{filename}");
        return ArtworkType.Static;
    }
}