using Microsoft.AspNetCore.Components;
using SchoderGallery.Builders;
using SchoderGallery.DTOs;
using SchoderGallery.Navigation;

namespace SchoderGallery.Shared;

public partial class BuildingRenderer : SvgComponentBase
{
    [Parameter] public FloorType BuilderType { get; set; }

    [Inject] private BuilderFactory BuilderFactory { get; set; }
    [Inject] private NavigationService Navigation { get; set; }

    private IBuilder _builder;

    protected override async Task OnInitializedAsync()
    {
        _builder = BuilderFactory.GetBuilder(BuilderType);
        await base.OnInitializedAsync();
    }

    protected override string PageTitle =>
        $"Schoder Gallery{(BuilderType == FloorType.Facade ? string.Empty : $" - {Navigation.GetFloor(BuilderType).LiftLabel}")}";

    protected override async Task<string> GetSvgContentAsync(SizeDto size) =>
        await _builder.GetSvgContentAsync(size.Width, size.Height);

    protected override int GetInterval() => _builder.Interval;
}