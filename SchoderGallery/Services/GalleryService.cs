using SchoderGallery.DTOs;

namespace SchoderGallery.Services;

public interface IGalleryService
{
    List<TodoDto> GetTodosAsync();
    List<ArtworkDto> GetArtworksAsync(int floor);
    ArtworkDto GetArtworkAsync(int floorNumber, int id);
}

public class GalleryService : IGalleryService
{
    public List<TodoDto> GetTodosAsync()
    {
        List<TodoDto> todos = [
            new("Display artworks in floors", TodoStatus.InProgress),
            new("Floor: start viewing artworks", TodoStatus.InProgress),
            new("Artwork: prev/next/back", TodoStatus.InProgress),

            new("Artwork: refresh", TodoStatus.Planned),
            new("Artwork: comments", TodoStatus.Planned),
            new("Artwork: buy", TodoStatus.Planned),
            new("Artwork: sell", TodoStatus.Planned),
            new("Cafe with payment and coffee as artworks", TodoStatus.Planned),
            new("Launch the SCHODER GALLERY", TodoStatus.Planned),

            new("Exhibition \"Find Me!\"", TodoStatus.Planned),
            new("Exhibition \"Hitler Eats Beigel\"", TodoStatus.Planned),
            new("Exhibition \"Who Am I?\"", TodoStatus.Planned),

            new("Favicon Schoder Factory brick", TodoStatus.Finished, 5, 10, 2025),
            new("To do list", TodoStatus.Finished, 5, 10, 2025),
            new("Fix mobile margins", TodoStatus.Finished, 5, 10, 2025),
            new("Lift and floors", TodoStatus.Finished, 4, 10, 2025),
        ];

        var today = DateTime.UtcNow;
        var nextday = today;
        foreach (var todo in todos.Where(t => t.Date == default))
        {
            todo.Date = todo.Status == TodoStatus.InProgress ? today : nextday;
            nextday = nextday.AddDays(1);
        }

        return [.. todos.OrderBy(t => t.Status).ThenBy(t => t.Date)];
    }

    // Read them from the server when the list is outdated, else from cache
    public List<ArtworkDto> GetArtworksAsync(int floorNumber) =>
        [
            new ArtworkDto(1, "Mona Lisa", -1, 2),
            new ArtworkDto(2, "Starry Night", 1, 3),
            new ArtworkDto(3, "The Scream", 2, -1),
        ];

    public ArtworkDto GetArtworkAsync(int floorNumber, int id) =>
        id < 1
            ? GetArtworksAsync(floorNumber).FirstOrDefault(a => a.PreviousId == -1)
            : GetArtworksAsync(floorNumber).FirstOrDefault(a => a.Id == id);
}