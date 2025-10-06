using Microsoft.AspNetCore.Components;
using SchoderGallery.Builders;
using SchoderGallery.Settings;
using SchoderGallery.Shared;

namespace SchoderGallery.Pages;

public partial class Artwork : SvgComponentBase
{
    [Parameter] public int ArtworkId { get; set; }
    [Inject] private IArtworkBuilder ArtworkBuilder { get; set; } = default!;

    protected override string PageTitle => $"Schoder Gallery - Artwork {ArtworkId}";

    protected override Task<string> GetSvgContentAsync(ScreenSize size) =>
        Task.FromResult(ArtworkBuilder.GetArtworkHtml(size.Width, size.Height, ArtworkId));

    protected override int GetInterval() => ArtworkBuilder.Interval;
}