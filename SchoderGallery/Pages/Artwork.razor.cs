using Microsoft.AspNetCore.Components;
using SchoderGallery.Builders;
using SchoderGallery.DTOs;
using SchoderGallery.Shared;

namespace SchoderGallery.Pages;

public partial class Artwork : ArtworkBase
{
    [Parameter] public Guid ArtworkId { get; set; }
    [Inject] private IArtworkBuilder ArtworkBuilder { get; set; } = default!;

    protected override string PageTitle => $"Schoder Gallery - Artwork {ArtworkId}";

    protected override ArtworkDto GetArtwork() => ArtworkBuilder.Artwork;

    protected override void NavigateTo(string url) => Nav.NavigateTo(url);

    protected override async Task<string> GetSvgContentAsync(SizeDto size) =>
        await ArtworkBuilder.GetHtmlAsync(size.Width, size.Height, ArtworkId);
}
