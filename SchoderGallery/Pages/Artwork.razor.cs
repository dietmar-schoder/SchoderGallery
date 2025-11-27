using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SchoderGallery.Builders;
using SchoderGallery.DTOs;
using SchoderGallery.Shared;

namespace SchoderGallery.Pages;

public partial class Artwork : ArtworkBase
{
    [Parameter] public Guid ArtworkId { get; set; }
    [Inject] private IArtworkBuilder ArtworkBuilder { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    protected override string PageTitle => $"Schoder Gallery - Artwork {ArtworkId}";

    protected override ArtworkDto GetArtwork() => ArtworkBuilder.Artwork;

    protected override void NavigateTo(string url) => Nav.NavigateTo(url);

    protected override async Task<string> GetSvgContentAsync(SizeDto size) =>
        await ArtworkBuilder.GetHtmlAsync(size.Width, size.Height, ArtworkId);

    private bool _shouldLoadInstagramEmbed;

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (!string.IsNullOrEmpty(ArtworkBuilder.InstagramId))
        {
            _shouldLoadInstagramEmbed = true;
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (_shouldLoadInstagramEmbed)
        {
            _shouldLoadInstagramEmbed = false;
            await LoadInstagramEmbed();
        }
    }

    private async Task LoadInstagramEmbed()
    {
        await JSRuntime.InvokeVoidAsync("eval",
            @"
if (window.instgrm && window.instgrm.Embeds) {
    window.instgrm.Embeds.process();
}
            ");
    }
}
