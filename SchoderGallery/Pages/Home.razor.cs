using Microsoft.AspNetCore.Components;
using SchoderGallery.Navigation;

namespace SchoderGallery.Pages;

public partial class Home
{
    [Inject] private NavigationService Navigation{ get; set; }

    protected override async Task OnInitializedAsync() =>
        await Navigation.GetInitVisitorAsync();
}
