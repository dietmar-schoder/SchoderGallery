using Microsoft.AspNetCore.Components;
using SchoderGallery.Navigation;

namespace SchoderGallery.Pages;

public partial class Login
{
    [Parameter] public Guid Id { get; set; }
    [Inject] private NavigationService NavigationService { get; set; } = default!;


    protected override async Task OnParametersSetAsync()
    {
        await NavigationService.LoginVisitorAsync(Id);
        Nav.NavigateTo($"/GroundFloor");
    }
}
