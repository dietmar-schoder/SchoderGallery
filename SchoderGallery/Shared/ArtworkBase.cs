using Microsoft.AspNetCore.Components;
using SchoderGallery.DTOs;
using SchoderGallery.Navigation;
using SchoderGallery.Services;

namespace SchoderGallery.Shared;

public abstract class ArtworkBase : SvgComponentBase
{
    [Inject] private GalleryService GalleryService { get; set; } = default!;
    [Inject] private NavigationService NavigationService { get; set; } = default!;
    [Inject] private ILocalStorageService LocalStorageService { get; set; } = default!;

    protected string _errorMessage;

    protected abstract ArtworkDto GetArtwork();
 
    protected abstract void NavigateTo(string url);

    protected async Task HandleBuy()
    {
        _isLoading = true;
        _errorMessage = string.Empty;
        await InvokeAsync(StateHasChanged);
        await Task.Yield();

        var visitor = await NavigationService.GetInitVisitorAsync();
        var artwork = GetArtwork();
        var checkout = await GalleryService.BuyArtworkAsync(visitor.Id, artwork);
        if (!string.IsNullOrEmpty(checkout.ErrorMessage))
        {
            _errorMessage = checkout.ErrorMessage;
            StateHasChanged();
            return;
        }
        var reservedArtwork = new ReservedArtworkDto(artwork.Id, artwork.Number);
        await LocalStorageService.SetItemAsync("reservedArtwork", reservedArtwork);
        NavigateTo(checkout.PaymentUrl);
    }

    protected void ClearError()
    {
        _errorMessage = string.Empty;
        _isLoading = false;
        StateHasChanged();
    }
}
