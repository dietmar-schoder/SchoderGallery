using SchoderGallery.Settings;

namespace SchoderGallery.DTOs;

public class ArtworkDto
{
    // "000051-Weather Forecast Bright-2025-Dietmar Schoder-0-1920-1080.png"
    // "Id-Title-Year-Artist-SizeType-Width-Height.FileExtension"
    public Guid Id { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
    public Func<ISettings, int, int, ArtworkType> RenderAlgorithm { get; set; }
    public SizeType SizeType { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string Artist { get; set; }
    public int Number { get; set; }
    public string FileName { get; set; }
    public int PreviousId { get; set; }
    public int NextId { get; set; }
    public bool IsRightWall { get; set; }
    public bool IsLeftWall => !IsRightWall;
    public int WallX { get; set; }
    public int WallY { get; set; }
    public int WallWidth { get; set; }
    public int ThumbnailSize { get; set; }
    public string Info { get; set; }

    public ArtworkDto() { }

    public ArtworkDto(
        string title,
        int year,
        Func<ISettings, int, int, ArtworkType> renderAlgorithm,
        SizeType sizeType,
        int width,
        int height,
        string artist,
        int id,
        string fileName = default,
        string info = default)
    {
        Title = title;
        Year = year;
        RenderAlgorithm = renderAlgorithm;
        SizeType = sizeType;
        Width = width;
        Height = height;
        Artist = artist;
        Number = id;
        FileName = fileName;
        Info = info;
    }
}
