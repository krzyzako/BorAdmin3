using System.Net.Http.Json;
using Bormech.Client.Liblary.Helpers;
using Bormech.Client.Liblary.Services.Contracts;
using Bormech.Data.DTOs;
using Bormech.Data.Responses;

namespace Bormech.Client.Liblary.Services.Implementations;

public class UserAccountService(GetHttpClient getHttpClient) : IUserAccountService
{
    private const string AuthUrl = "api/authentication";

    public async Task<GeneralResonse?> CreateAsync(Register user)
    {
        var httpClient = getHttpClient.GetPublicHttpClient();
        var response = await httpClient.PostAsJsonAsync(AuthUrl + "/register", user);
        if (!response.IsSuccessStatusCode) return new GeneralResonse(false, "Error occured");
        return await response.Content.ReadFromJsonAsync<GeneralResonse>()!;
    }

    public async Task<LoginResponse?> SingInAsync(Login user)
    {
        var httpClient = getHttpClient.GetPublicHttpClient();
        var result = await httpClient.PostAsJsonAsync(AuthUrl + "/login", user);
        if (!result.IsSuccessStatusCode) return new LoginResponse(false, "Error occured");
        return await result.Content.ReadFromJsonAsync<LoginResponse>()!;
    }

    public async Task<LoginResponse?> RefreshTokenAsync(RefreshToken token)
    {
        var httpClient = getHttpClient.GetPublicHttpClient();
        var response = await httpClient.PostAsJsonAsync(AuthUrl + "/refresh-token", token);
        if (!response.IsSuccessStatusCode) return new LoginResponse(false, "Error occured");
        return await response.Content.ReadFromJsonAsync<LoginResponse>()!;
    }

    public async Task<WeatherForecast[]?> GetWeatherForecastAsync()
    {
        var httpClient = await getHttpClient.GetPrivateHttpClient();
        var result = httpClient.GetFromJsonAsync<WeatherForecast[]>("api/WeatherForecast");
        return await result!;
    }
}