using Microsoft.JSInterop;
using SchoderGallery.Helpers;

namespace SchoderGallery.Services;

public interface ILocalStorageService
{
    Task<T> GetItemAsync<T>(string key);
    Task SetItemAsync<T>(string key, T value);
    Task RemoveItemAsync(string key);
}

public class LocalStorageService(IJSRuntime jsRuntime) : ILocalStorageService
{
    public async Task<T> GetItemAsync<T>(string key) =>
        (await jsRuntime.InvokeAsync<string>("localStorage.getItem", Const.StoragePrefix + key)).FromJson<T>();

    public async Task SetItemAsync<T>(string key, T value) =>
        await jsRuntime.InvokeVoidAsync("localStorage.setItem", Const.StoragePrefix + key, value.ToJson());

    public async Task RemoveItemAsync(string key) =>
        await jsRuntime.InvokeVoidAsync("localStorage.removeItem", Const.StoragePrefix + key);
}
