using Microsoft.AspNetCore.Components;
using SchoderGallery.DTOs;
using SchoderGallery.Navigation;
using SchoderGallery.Services;
using System.Net.Http.Json;

namespace SchoderGallery.Pages;

public partial class Home
{
    [Inject] private NavigationService Navigation{ get; set; }

    protected override async Task OnInitializedAsync()
    {
        await Navigation.GetInitVisitorAsync();
    }
}
