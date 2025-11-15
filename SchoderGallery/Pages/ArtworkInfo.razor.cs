using Microsoft.AspNetCore.Components;
using SchoderGallery.Builders;
using SchoderGallery.DTOs;
using SchoderGallery.Navigation;
using SchoderGallery.Services;
using SchoderGallery.Shared;

namespace SchoderGallery.Pages;

public partial class ArtworkInfo : SvgComponentBase
{
    [Parameter] public int ArtworkId { get; set; }
    [Inject] private IArtworkInfoBuilder ArtworkInfoBuilder { get; set; } = default!;
    [Inject] private IGalleryService GalleryService { get; set; } = default!;
    [Inject] private NavigationService NavigationService { get; set; } = default!;
    [Inject] private ILocalStorageService LocalStorageService { get; set; } = default!;

    protected override string PageTitle => $"Schoder Gallery - Artwork Info {ArtworkId}";

    protected override async Task<string> GetSvgContentAsync(SizeDto size) =>
        await ArtworkInfoBuilder.GetHtmlAsync(size.Width, size.Height, ArtworkId);

    private async Task HandleBuy()
    {
        _isLoading = true;
        await InvokeAsync(StateHasChanged);
        await Task.Yield();
        
        var visitor = await NavigationService.GetInitVisitorAsync();
        var artwork = ArtworkInfoBuilder.Artwork;
        var checkout = await GalleryService.BuyArtworkAsync(visitor.Id, artwork);
        if (!string.IsNullOrEmpty(checkout.ErrorMessage))
        {
            // Error = checkout.ErrorMessage
            return;
        }
        var reservedArtwork = new ReservedArtworkDto(artwork.Id, artwork.Number);
        await LocalStorageService.SetItemAsync("reservedArtwork", reservedArtwork);
        Nav.NavigateTo(checkout.PaymentUrl);
    }
}
