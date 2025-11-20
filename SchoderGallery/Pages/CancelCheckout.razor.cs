using Microsoft.AspNetCore.Components;
using SchoderGallery.Builders;
using SchoderGallery.DTOs;
using SchoderGallery.Navigation;
using SchoderGallery.Services;
using SchoderGallery.Shared;

namespace SchoderGallery.Pages;

public partial class CancelCheckout : SvgComponentBase
{
    [Parameter] public Guid ArtworkId { get; set; }
    [Inject] private IArtworkInfoBuilder ArtworkInfoBuilder { get; set; } = default!;
    [Inject] private GalleryService GalleryService { get; set; } = default!;
    [Inject] private NavigationService NavigationService { get; set; } = default!;
    [Inject] private ILocalStorageService LocalStorageService { get; set; } = default!;

    protected override string PageTitle => $"Schoder Gallery - Cancel Checkout";

    protected override async Task<string> GetSvgContentAsync(SizeDto size)
    {
        ArtworkInfoBuilder.Init(size.Width, size.Height);
        var visitor = await NavigationService.GetInitVisitorAsync();
        var floor = await NavigationService.GetVisitorFloorAsync();
        var artwork = await GalleryService.GetArtworkAsync(visitor, floor.FloorNumber, ArtworkId);
        var reservedArtwork = await LocalStorageService.GetItemAsync<ReservedArtworkDto>("reservedArtwork");
        if (reservedArtwork.Id == artwork.Id)
        {
            await GalleryService.CancelCheckoutAsync(visitor.Id, reservedArtwork.Id);
        }
        await LocalStorageService.RemoveItemAsync("reservedArtwork");
        Nav.NavigateTo($"/ArtworkInfo/{artwork.Id}");
        return string.Empty;
    }
}
