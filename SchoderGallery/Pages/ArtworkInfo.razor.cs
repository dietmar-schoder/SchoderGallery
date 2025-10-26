using Microsoft.AspNetCore.Components;
using SchoderGallery.Builders;
using SchoderGallery.DTOs;
using SchoderGallery.Shared;

namespace SchoderGallery.Pages;

public partial class ArtworkInfo : SvgComponentBase
{
    [Parameter] public int ArtworkId { get; set; }
    [Inject] private IArtworkInfoBuilder ArtworkInfoBuilder { get; set; } = default!;

    protected override string PageTitle => $"Schoder Gallery - Artwork Info {ArtworkId}";

    protected override async Task<string> GetSvgContentAsync(SizeDto size) =>
        await ArtworkInfoBuilder.GetHtmlAsync(size.Width, size.Height, ArtworkId);

    protected override int GetInterval() => ArtworkInfoBuilder.Interval;
}