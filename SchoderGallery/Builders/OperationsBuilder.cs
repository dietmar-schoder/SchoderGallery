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
    : BaseFloorBuilder(settingsFactory, svgPainter, navigation, galleryService), IBuilder
{
    public override BuilderType Type => BuilderType.Operations;
    public int Interval => 0;
    protected override void Draw()
    {
        base.Draw();

        var wall = _settings.WallThickness;

        var todos =  _galleryService.GetTodosAsync();

        foreach (var (todo, i) in todos.Select((value, i) => (value, i)))
        {
            _svgPainter.TextLeft(wall + _largeFontSize, wall + (i + 4) * _largeFontSize, TodoLine(todo, i + 1), _fontSize, StatusColour(todo.Status));
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
                TodoStatus.InProgress => Colours.Pink,
                TodoStatus.Planned => Colours.Blue,
                _ => Colours.DarkGray
            };
    }
}