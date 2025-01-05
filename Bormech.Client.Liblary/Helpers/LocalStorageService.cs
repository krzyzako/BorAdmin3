using Blazored.LocalStorage;

namespace Bormech.Client.Liblary.Helpers;

public class LocalStorageService(ILocalStorageService localStorageService)
{
    private const string StorageKey = "authentification-token";

    public async Task<string> GetToken()
    {
        return await localStorageService.GetItemAsync<string>(StorageKey);
    }

    public async Task SetToken(string item)
    {
        await localStorageService.SetItemAsync(StorageKey, item);
    }

    public async Task RemoveToken()
    {
        await localStorageService.RemoveItemAsync(StorageKey);
    }
}