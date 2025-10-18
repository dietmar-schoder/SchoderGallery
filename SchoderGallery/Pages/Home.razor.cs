using Microsoft.AspNetCore.Components;
using SchoderGallery.DTOs;
using System.Net.Http.Json;

namespace SchoderGallery.Pages;

public partial class Home
{
    [Inject] private HttpClient HttpClient { get; set; }

    private LocaleDto locale;

    protected override async Task OnInitializedAsync()
    {
        locale = await HttpClient.GetFromJsonAsync<LocaleDto>($"/api/countries");
    }
}
