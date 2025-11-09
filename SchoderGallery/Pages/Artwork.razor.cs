using Microsoft.AspNetCore.Components;
using SchoderGallery.Builders;
using SchoderGallery.DTOs;
using SchoderGallery.Shared;

namespace SchoderGallery.Pages;

public partial class Artwork : SvgComponentBase
{
    [Parameter] public int ArtworkId { get; set; }
    [Inject] private IArtworkBuilder ArtworkBuilder { get; set; } = default!;

    protected override string PageTitle => $"Schoder Gallery - Artwork {ArtworkId}";

    protected override async Task<string> GetSvgContentAsync(SizeDto size) =>
        await ArtworkBuilder.GetHtmlAsync(size.Width, size.Height, ArtworkId);
}
