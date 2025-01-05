using Bormech.Data.DTOs;
using Bormech.Data.Responses;

namespace Boremch.Server.Liblary.Reporitories.Contracts;

public interface IUserAccount
{
    Task<GeneralResonse> CreateAsync(Register? user);
    Task<LoginResponse> SingInAsync(Login user);
    Task<LoginResponse> RefreshTokenAsync(RefreshToken token);
}