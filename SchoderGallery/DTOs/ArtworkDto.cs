using SchoderGallery.Settings;

namespace SchoderGallery.DTOs;

public class ArtworkDto(
    string title,
    int year,
    Func<ISettings, int, int, int> renderAlgorithm,
    SizeType sizeType,
    int width,
    int height,
    string artist,
    int id)
{
    public string Title { get; } = title;
    public int Year { get; } = year;
    public Func<ISettings, int, int, int> RenderAlgorithm { get; } = renderAlgorithm;
    public SizeType SizeType { get; } = sizeType;
    public int Width { get; } = width;
    public int Height { get; } = height;
    public string Artist { get; } = artist;
    public int Id { get; } = id;
    public int PreviousId { get; set; }
    public int NextId { get; set; }
    public bool IsRightWall { get; set; }
    public bool IsLeftWall => !IsRightWall;
    public int WallX { get; set; }
    public int WallY { get; set; }
    public int WallWidth { get; set; }
}