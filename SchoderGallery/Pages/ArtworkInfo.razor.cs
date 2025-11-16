using Microsoft.AspNetCore.Components;
using SchoderGallery.Builders;
using SchoderGallery.DTOs;
using SchoderGallery.Shared;

namespace SchoderGallery.Pages;

public partial class ArtworkInfo : ArtworkBase
{
    [Parameter] public int ArtworkId { get; set; }
    [Inject] private IArtworkInfoBuilder ArtworkInfoBuilder { get; set; } = default!;

    protected override string PageTitle => $"Schoder Gallery - Artwork Info {ArtworkId}";

    protected override ArtworkDto GetArtwork() => ArtworkInfoBuilder.Artwork;

    protected override void NavigateTo(string url) => Nav.NavigateTo(url);

    protected override async Task<string> GetSvgContentAsync(SizeDto size) =>
        await ArtworkInfoBuilder.GetHtmlAsync(size.Width, size.Height, ArtworkId);
}
