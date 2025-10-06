using SchoderGallery.Algorithms;
using SchoderGallery.DTOs;

namespace SchoderGallery.Services;

public interface IGalleryService
{
    List<TodoDto> GetTodosAsync();
    List<ArtworkDto> GetArtworksAsync(int floor);
    ArtworkDto GetArtworkAsync(int floorNumber, int id);
}

public class GalleryService(AlgorithmFactory algorithmFactory) : IGalleryService
{
    public List<TodoDto> GetTodosAsync()
    {
        List<TodoDto> todos = [
            new("Exhibitions in floors + lift labels", TodoStatus.InProgress),
            new("Explanation pages", TodoStatus.InProgress),
            new("Hourglass", TodoStatus.InProgress),

            new("Artwork: comments", TodoStatus.Planned),
            new("Artwork: buy", TodoStatus.Planned),
            new("Artwork: sell", TodoStatus.Planned),
            new("Cafe with payment and coffee as artworks", TodoStatus.Planned),
            new("Launch the SCHODER GALLERY", TodoStatus.Planned),

            new("Exhibition \"Find Me!\"", TodoStatus.Planned),
            new("Exhibition \"Hitler Eats Beigel\"", TodoStatus.Planned),
            new("Exhibition \"Who Am I?\"", TodoStatus.Planned),

            new("Artwork: refresh", TodoStatus.Finished, 6, 10, 2025),
            new("Artwork: prev/next/back", TodoStatus.Finished, 6, 10, 2025),
            new("Floor: start viewing artworks", TodoStatus.Finished, 6, 10, 2025),
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
    public List<ArtworkDto> GetArtworksAsync(int floorNumber)
    {
        var turtleGraphics = algorithmFactory.GetAlgorithm(AlgorithmType.TurtleGraphics) as TurtleGraphics;
        var fourColours = algorithmFactory.GetAlgorithm(AlgorithmType.FourColours) as FourColours;
        var i = 0;

        return [
            new ArtworkDto(++i, "Adventure 1/4", -1, i + 1, (s, p, w, h) => turtleGraphics.Turtle1(s, p, w, h, 8, 4)),
            new ArtworkDto(++i, "Adventure 2/4", i - 1, i + 1, (s, p, w, h) => turtleGraphics.Turtle1(s, p, w, h, 16, 9)),
            new ArtworkDto(++i, "Adventure 3/4", i - 1, i + 1, (s, p, w, h) => turtleGraphics.Turtle2(s, p, w, h, 13, 7)),
            new ArtworkDto(++i, "Adventure 4/4", i - 1, i + 1, (s, p, w, h) => turtleGraphics.Turtle2(s, p, w, h, 32, 18, 1)),
            new ArtworkDto(++i, "The Entrance 1/2", i - 1, i + 1, (s, p, w, h) => fourColours.Pattern1(s, p, w, h, 10, 6)),
            new ArtworkDto(++i, "The Entrance 2/2", i - 1, -1, (s, p, w, h) => fourColours.Pattern1(s, p, w, h, 21, 13)),
        ];
}

    public ArtworkDto GetArtworkAsync(int floorNumber, int id) =>
        id < 1
            ? GetArtworksAsync(floorNumber).FirstOrDefault(a => a.PreviousId == -1)
            : GetArtworksAsync(floorNumber).FirstOrDefault(a => a.Id == id);
}