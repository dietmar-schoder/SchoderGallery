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

    private LiftBuilder _builder;
    private bool _isMovingUp = false;
    private bool _isMovingDown = false;
    private int _currentFloorNumber;

    private bool IsMoving => _isMovingUp || _isMovingDown;

    protected override string PageTitle => "Schoder Gallery - Lift";

    protected override async Task OnInitializedAsync()
    {
        _builder = BuilderFactory.GetBuilder(FloorType.Lift) as LiftBuilder;
        await base.OnInitializedAsync();
        _currentFloorNumber = _builder.CurrentFloor.FloorNumber;
    }

    protected override async Task<string> GetSvgContentAsync(SizeDto size) =>
        await _builder.GetSvgContentAsync(size.Width, size.Height);

    private async Task OnLiftClick(string floorNumber)
    {
        var currentFloor = await Navigation.GetVisitorFloorAsync();
        var newFloor = Navigation.GetFloor(floorNumber);

        _isMovingUp = newFloor.FloorNumber > currentFloor.FloorNumber;
        _isMovingDown = newFloor.FloorNumber < currentFloor.FloorNumber;

        if (IsMoving)
        {
            StateHasChanged();
            await Task.Yield();
        }

        _isMovingUp = _isMovingDown = false;
        Nav.NavigateTo(newFloor.PageAndParam());
    }
}
