using System.Net;
using System.Net.Http.Headers;
using Bormech.Client.Liblary.Services.Contracts;
using Bormech.Data.DTOs;

namespace Bormech.Client.Liblary.Helpers;

public class CustomHttpHandler(
    GetHttpClient getHttpClient,
    LocalStorageService localStorageService,
    IUserAccountService accountService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        Console.WriteLine("CustomHttpHandler");
        var loginUrl = request.RequestUri!.AbsolutePath.Contains("login");
        var registerUrl = request.RequestUri!.AbsolutePath.Contains("register");
        var refreshTokenUrl = request.RequestUri!.AbsolutePath.Contains("refresh-token");
        Console.WriteLine($"{loginUrl} {registerUrl} {refreshTokenUrl}");
        if (loginUrl || registerUrl || refreshTokenUrl) return await base.SendAsync(request, cancellationToken);
        var result = await base.SendAsync(request, cancellationToken);
        Console.WriteLine(result.StatusCode);
        if (result.StatusCode == HttpStatusCode.Unauthorized)
        {
            var stringToken = await localStorageService.GetToken();
            if (stringToken == null) return result;

            var token = string.Empty;
            try
            {
                token = request.Headers.Authorization!.Parameter!;
            }
            catch (Exception e)
            {
            }

            var deserializeToken = Serializations.DeserializeObj<UserSession>(stringToken);
            Console.WriteLine(deserializeToken);
            if (deserializeToken is null) return result;
            if (string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", deserializeToken.Token);
                return await base.SendAsync(request, cancellationToken);
            }

            var newJwtToken = await GetRefreshToken(deserializeToken.RefreshToken);
            if (string.IsNullOrEmpty(newJwtToken)) return result;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newJwtToken);
            return await base.SendAsync(request, cancellationToken);
        }

        return result;
    }

    private async Task<string> GetRefreshToken(string? refreshToken)
    {
        var result = await accountService.RefreshTokenAsync(new RefreshToken { Token = refreshToken });
        var serializeToken = Serializations.SerializeObj(new UserSession
            { Token = result!.Token, RefreshToken = result!.RefreshToken });
        await localStorageService.SetToken(serializeToken);
        return result!.Token;
    }
}