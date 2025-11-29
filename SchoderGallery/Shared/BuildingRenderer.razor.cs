using Microsoft.AspNetCore.Components;
using SchoderGallery.Builders;
using SchoderGallery.DTOs;
using SchoderGallery.Navigation;
using SchoderGallery.Services;

namespace SchoderGallery.Shared;

public partial class BuildingRenderer : SvgComponentBase
{
    [Parameter] public FloorType BuilderType { get; set; }

    [Inject] private BuilderFactory BuilderFactory { get; set; }
    [Inject] private NavigationService Navigation { get; set; }
    [Inject] private GalleryService GalleryService { get; set; }


    private IBuilder _builder;

    protected override string PageTitle =>
        $"Schoder Gallery{(BuilderType == FloorType.Facade ? string.Empty : $" - {Navigation.GetFloor(BuilderType).LiftLabel}")}";

    protected override async Task OnInitializedAsync()
    {
        _builder = BuilderFactory.GetBuilder(BuilderType);
        await base.OnInitializedAsync();
    }

    protected override async Task<string> GetSvgContentAsync(SizeDto size) =>
        await _builder.GetSvgContentAsync(size.Width, size.Height);

    private async Task OnAreaClick(ClickableArea area)
    {
        _isLoading = true;
        await InvokeAsync(StateHasChanged);
        await Task.Yield();

        await GalleryService.LoadExhibitionsAsync();

        if (area.FloorType is not null)
        {
            var floor = Navigation.GetFloor((FloorType)area.FloorType);
            if (floor.IsArtworksFloor)
            {
                var visitor = await Navigation.GetInitVisitorAsync();
                await GalleryService.GetExhibitionArtworksAsync(visitor, floor.FloorNumber);
            };
        }

        Nav.NavigateTo(area.Page);
    }
}
