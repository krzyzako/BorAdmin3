using Bormech.Data.DTOs;
using Bormech.Data.Responses;

namespace Bormech.Client.Liblary.Services.Contracts;

public interface IUserAccountService
{
    Task<GeneralResonse?> CreateAsync(Register user);
    Task<LoginResponse?> SingInAsync(Login user);
    Task<LoginResponse?> RefreshTokenAsync(RefreshToken token);
    Task<GeneralResonse?> ChangePasswordAsync(ChangePassword? changePassword);
    Task<WeatherForecast[]?> GetWeatherForecastAsync();
}