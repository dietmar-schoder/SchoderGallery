using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SchoderGallery.DTOs;

namespace SchoderGallery.Shared;

public abstract class SvgComponentBase : ComponentBase, IDisposable
{
    [Inject] protected IJSRuntime JS { get; set; } = default!;

    private DotNetObjectReference<SvgComponentBase> _dotNetRef;
    private bool _isFirstRender = true;

    protected string _screenHeightPx = "100vh";
    protected string _svgContent;
    protected bool _isLoading = false;

    protected abstract Task<string> GetSvgContentAsync(SizeDto size);
    protected abstract string PageTitle { get; }

    protected override async Task OnInitializedAsync()
    {
        var size = await GetScreenSizeAsync();
        SetScreenHeightPx(size);

        _svgContent = await GetSvgContentAsync(size);
        _isLoading = false;

        _dotNetRef = DotNetObjectReference.Create(this);
        await JS.InvokeVoidAsync("initResizeHandler", _dotNetRef, GetInterval());
    }

    protected override async Task OnParametersSetAsync()
    {
        if (_isFirstRender)
        {
            _isFirstRender = false;
            return;
        }

        _isLoading = true;
        await InvokeAsync(StateHasChanged);
        await Task.Yield();
        await RenderPageAsync(await GetScreenSizeAsync());
    }

    [JSInvokable]
    public async Task OnResizeAsync(SizeDto size)
    {
        SetScreenHeightPx(size);
        await RenderPageAsync(size);
    }

    private async Task<SizeDto> GetScreenSizeAsync() =>
        await JS.InvokeAsync<SizeDto>("getScreenSize");

    protected void SetScreenHeightPx(SizeDto size) =>
        _screenHeightPx = $"{size.Height}px";

    private async Task RenderPageAsync(SizeDto size)
    {
        _svgContent = await GetSvgContentAsync(size);
        _isLoading = false;
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
