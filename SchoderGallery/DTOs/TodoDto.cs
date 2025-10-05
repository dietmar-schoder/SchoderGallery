namespace SchoderGallery.DTOs;

public class TodoDto(string text, TodoStatus status, int day = 0, int month = 0, int year = 00)
{
    public string Text { get; set; } = text;
    public TodoStatus Status { get; set; } = status;
    public DateTime Date { get; set; } = day == 0 ? default : new DateTime(year, month, day);
}