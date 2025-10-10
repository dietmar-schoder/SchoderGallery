using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SchoderGallery.DTOs;

namespace SchoderGallery.Shared;

public abstract class SvgComponentBase : ComponentBase, IDisposable
{
    [Inject] protected IJSRuntime JS { get; set; } = default!;

    protected string _screenHeightPx = "100vh";
    protected string _svgContent;
    private DotNetObjectReference<SvgComponentBase> _dotNetRef;

    protected abstract Task<string> GetSvgContentAsync(SizeDto size);
    protected abstract string PageTitle { get; }

    protected override async Task OnInitializedAsync()
    {
        var size = await GetScreenSize();
        SetScreenHeightPx(size);

        _svgContent = await GetSvgContentAsync(size);

        _dotNetRef = DotNetObjectReference.Create(this);
        await JS.InvokeVoidAsync("initResizeHandler", _dotNetRef, GetInterval());
    }

    protected override async Task OnParametersSetAsync() =>
        await RenderPageAsync(await GetScreenSize());

    public async Task RefreshAsync() =>
        await OnParametersSetAsync();

    [JSInvokable]
    public async Task OnResize(SizeDto size)
    {
        SetScreenHeightPx(size);
        await RenderPageAsync(size);
    }

    private async Task<SizeDto> GetScreenSize() =>
        await JS.InvokeAsync<SizeDto>("getScreenSize");

    protected void SetScreenHeightPx(SizeDto size) =>
        _screenHeightPx = $"{size.Height}px";

    private async Task RenderPageAsync(SizeDto size)
    {
        _svgContent = await GetSvgContentAsync(size);
        await InvokeAsync(StateHasChanged);
    }

    protected abstract int GetInterval();

    public void Dispose()
    {
        _dotNetRef?.Dispose();
        _dotNetRef = null;
        GC.SuppressFinalize(this);
    }
}