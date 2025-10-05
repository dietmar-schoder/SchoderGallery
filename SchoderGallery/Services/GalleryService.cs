using SchoderGallery.DTOs;

namespace SchoderGallery.Services;

public interface IGalleryService
{
    List<TodoDto> GetTodosAsync();
}

public class GalleryService : IGalleryService
{
    public List<TodoDto> GetTodosAsync() =>
        [
            new("Display artworks in floors", new DateTime(2025, 10, 05), TodoStatus.InProgress),
            new("Floor: start viewing artworks", new DateTime(2025, 10, 05), TodoStatus.InProgress),
            new("Artwork: prev/next/back", new DateTime(2025, 10, 05), TodoStatus.InProgress),
            new("Favicon Schoder Factory brick", new DateTime(2025, 10, 05), TodoStatus.InProgress),

            new("Artwork: refresh", new DateTime(2025, 10, 06), TodoStatus.Planned),
            new("Artwork: comments", new DateTime(2025, 10, 06), TodoStatus.Planned),
            new("Artwork: buy", new DateTime(2025, 10, 06), TodoStatus.Planned),
            new("Artwork: sell", new DateTime(2025, 10, 06), TodoStatus.Planned),
            new("Cafe with payment and coffee as artworks", new DateTime(2025, 10, 06), TodoStatus.Planned),
            new("Launch the SCHODER GALLERY", new DateTime(2025, 10, 30), TodoStatus.Planned),

            new("Exhibition \"Find Me!\"", new DateTime(2025, 11, 07), TodoStatus.Planned),
            new("Exhibition \"Hitler Eats Beigel\"", new DateTime(2025, 11, 08), TodoStatus.Planned),
            new("Exhibition \"Who Am I?\"", new DateTime(2025, 11, 09), TodoStatus.Planned),

            new("To do list", new DateTime(2025, 10, 05), TodoStatus.Finished),
            new("Fix mobile margins", new DateTime(2025, 10, 05), TodoStatus.Finished),
            new("Lift and floors", new DateTime(2025, 10, 04), TodoStatus.Finished),
        ];
}