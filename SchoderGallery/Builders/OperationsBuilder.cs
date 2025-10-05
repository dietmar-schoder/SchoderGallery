using SchoderGallery.DTOs;
using SchoderGallery.Navigation;
using SchoderGallery.Painters;
using SchoderGallery.Services;
using SchoderGallery.Settings;

namespace SchoderGallery.Builders;

public class OperationsBuilder(
    SettingsFactory settingsFactory,
    SvgPainter svgPainter,
    NavigationService navigation,
    IGalleryService galleryService)
    : BaseFloorBuilder(settingsFactory, svgPainter, navigation), IBuilder
{
    public override BuilderType Type => BuilderType.Operations;
    public int Interval => 0;
    protected override void Draw()
    {
        base.Draw();

        var wall = _settings.WallThickness;

        var todos =  galleryService.GetTodosAsync();

        foreach (var (todo, i) in todos.Select((value, i) => (value, i)))
        {
            _svg.TextLeft(wall + _gap, wall + (i + 4) * _gap, TodoLine(todo, i + 1), (int)(_gap * 0.4), StatusColour(todo.Status));
        }

        string TodoLine(TodoDto todo, int number)
        {
            return todo.Status switch
            {
                TodoStatus.InProgress => $"{number}. {todo.Text} ({todo.Date:dd-MM-yy})",
                TodoStatus.Planned => $"{number}. {todo.Text} ({todo.Date:dd-MM-yy})",
                _ => $"{todo.Text} ({todo.Date:dd-MM-yy})"
            };
        }

        string StatusColour(TodoStatus status) =>
            status switch
            {
                TodoStatus.InProgress => _settings.Pink,
                TodoStatus.Planned => _settings.Blue,
                _ => _settings.DarkGray
            };
    }
}