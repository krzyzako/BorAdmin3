using System.Net.Http.Headers;
using Bormech.Data.DTOs;

namespace Bormech.Client.Liblary.Helpers;

public class GetHttpClient(IHttpClientFactory httpClientFactory, LocalStorageService localStorage)
{
    private const string HeaderKey = "Authorization";

    public async Task<HttpClient> GetPrivateHttpClient()
    {
        var client = httpClientFactory.CreateClient("BormechApi");
        var stringToken = await localStorage.GetToken();
        if (string.IsNullOrEmpty(stringToken)) return client;

        var deserializedToken = Serializations.DeserializeObj<UserSession>(stringToken);
        if (deserializedToken == null) return client;

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", deserializedToken.Token);
        return client;
    }

    public HttpClient GetPublicHttpClient()
    {
        var client = httpClientFactory.CreateClient("BormechApi");
        client.DefaultRequestHeaders.Remove(HeaderKey);
        return client;
    }
}