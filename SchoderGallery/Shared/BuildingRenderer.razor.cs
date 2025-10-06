using Microsoft.AspNetCore.Components;
using SchoderGallery.Builders;
using SchoderGallery.Navigation;
using SchoderGallery.Settings;

namespace SchoderGallery.Shared;

public partial class BuildingRenderer : SvgComponentBase
{
    [Parameter] public BuilderType BuilderType { get; set; }

    [Inject] private BuilderFactory BuilderFactory { get; set; }
    [Inject] private NavigationService Navigation { get; set; }

    private IBuilder _builder;

    protected override async Task OnInitializedAsync()
    {
        _builder = BuilderFactory.GetBuilder(BuilderType);
        await base.OnInitializedAsync();
    }

    protected override string PageTitle =>
        $"Schoder Gallery{(BuilderType == BuilderType.Facade ? string.Empty : $" - {Navigation.GetFloor(BuilderType).LiftLabel}")}";

    protected override Task<string> GetSvgContentAsync(ScreenSize size) =>
        Task.FromResult(_builder.GetSvgContent(size.Width, size.Height));

    protected override int GetInterval() => _builder.Interval;
}