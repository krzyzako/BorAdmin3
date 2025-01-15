using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Bormech.Data.DTOs;
using Microsoft.AspNetCore.Components.Authorization;

namespace Bormech.Client.Liblary.Helpers;

public class CustomAuthenticationStateProvider(LocalStorageService localStorageService) : AuthenticationStateProvider
{
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var stringToken = await localStorageService.GetToken();
        if (string.IsNullOrEmpty(stringToken)) return await Task.FromResult(new AuthenticationState(_anonymous));
        Console.WriteLine("String token: " + stringToken);
        
        var deserializeToken = Serializations.DeserializeObj<UserSession>(stringToken);
        if (deserializeToken == null) return await Task.FromResult(new AuthenticationState(_anonymous));
        Console.WriteLine("Deserialize token: " + Serializations.SerializeObj(deserializeToken));
        
        var getUserClaims = DecryptToken(deserializeToken.Token!);
        if (getUserClaims == null) return await Task.FromResult(new AuthenticationState(_anonymous));
        Console.WriteLine("GetUserClaims: " + Serializations.SerializeObj(getUserClaims));
        
        var claimsPrincipal = SetClaimsPrincipal(getUserClaims);
        return await Task.FromResult(new AuthenticationState(claimsPrincipal));
    }

    public async Task UpdateAuthenticationState(UserSession userSession)
    {
        var claimsPricipal = new ClaimsPrincipal();
        if (userSession.Token != null || userSession.Token != null)
        {
            var serializeSesion = Serializations.SerializeObj(userSession);
            await localStorageService.SetToken(serializeSesion);
            var getUserClaims = DecryptToken(userSession.Token!);
            claimsPricipal = SetClaimsPrincipal(getUserClaims);
        }
        else
        {
            await localStorageService.RemoveToken();
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPricipal)));
    }

    private ClaimsPrincipal SetClaimsPrincipal(CustomUserClaims claims)
    {
        if (claims.Email is null) return new ClaimsPrincipal();
        return new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, claims.Id!),
            new(ClaimTypes.Name, claims.Name!),
            new(ClaimTypes.Email, claims.Email),
            new(ClaimTypes.Role, claims.Role!)
        }, "JwtAuth"));
    }

    private static CustomUserClaims DecryptToken(string jwtToken)
    {
        if (string.IsNullOrEmpty(jwtToken)) return new CustomUserClaims();

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwtToken);
        var userId = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
        var name = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
        var email = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
        var role = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
        return new CustomUserClaims(userId?.Value, name?.Value, email?.Value, role?.Value);
    }
}