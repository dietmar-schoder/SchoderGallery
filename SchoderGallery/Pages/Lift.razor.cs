using Microsoft.AspNetCore.Components;
using SchoderGallery.Builders;
using SchoderGallery.DTOs;
using SchoderGallery.Navigation;
using SchoderGallery.Shared;

namespace SchoderGallery.Pages;

public partial class Lift : SvgComponentBase
{
    [Inject] private BuilderFactory BuilderFactory { get; set; }
    [Inject] private NavigationService Navigation { get; set; }

    private IBuilder _builder;
    private bool _isMovingUp = false;
    private bool _isMovingDown = false;

    protected override async Task OnInitializedAsync()
    {
        _builder = BuilderFactory.GetBuilder(BuilderType.Lift);
        await base.OnInitializedAsync();
    }
    private async Task OnLiftClick(string floorNumber)
    {
        var currentFloor = Navigation.GetVisitorFloor();
        var newFloor = Navigation.GetFloor(floorNumber);

        _isMovingUp = newFloor.FloorNumber > currentFloor.FloorNumber;
        _isMovingDown = newFloor.FloorNumber < currentFloor.FloorNumber;
        StateHasChanged();
        await Task.Yield();

        //var artworks = await ArtworkService.GetArtworksAsync(floorId);
        //ArtworkState.CurrentArtworks = artworks;
        await Task.Delay(1000);

        _isMovingUp = _isMovingDown = false;
        Nav.NavigateTo(newFloor.PageAndParam());
    }

    protected override string PageTitle =>
        "Schoder Gallery - Lift";

    protected override Task<string> GetSvgContentAsync(SizeDto size) =>
        Task.FromResult(_builder.GetSvgContent(size.Width, size.Height));

    protected override int GetInterval() => _builder.Interval;
}