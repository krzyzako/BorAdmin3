using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Bormech.Data.DTOs;
using Bormech.Data.Entities;
using Bormech.Data.Responses;
using Bormech.Server.Liblary.Data;
using Bormech.Server.Liblary.Helpers;
using Bormech.Server.Liblary.Reporitories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Bormech.Server.Liblary.Reporitories.Implementations;

public class UserAccountRepository(IOptions<JwtSection> config, AppDbContext appDbContext) : IUserAccount
{
    public async Task<GeneralResonse> CreateAsync(Register? user)
    {
        if (user is null) return new GeneralResonse(false, "User is null");
        var checkUser = await FindUserByEmail(user.Email!);
        if (checkUser != null) return new GeneralResonse(false, "User already exist");

        // Add user to database
        var applicationUser = await AddToDatabase(new ApplicationUser
        {
            FullName = user.FullName,
            Email = user.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(user.Password)
        });

        // check, created and assigned role
        var checkAdminRole = await appDbContext.SystemRoles.FirstOrDefaultAsync(r => r.Name!.Equals(Constants.Admin));
        if (checkAdminRole is null)
        {
            var createAdminRole = await AddToDatabase(new SystemRole { Name = Constants.Admin });
            await AddToDatabase(new UserRole { RoleId = createAdminRole.Id, UserId = applicationUser.Id });
            return new GeneralResonse(true, "User created successfully");
        }

        var checkUserRole = await appDbContext.SystemRoles.FirstOrDefaultAsync(r => r.Name!.Equals(Constants.User));
        if (checkUserRole is null)
        {
            var respons = await AddToDatabase(new SystemRole { Name = Constants.User });
            await AddToDatabase(new UserRole { RoleId = respons.Id, UserId = applicationUser.Id });
        }
        else
        {
            await AddToDatabase(new UserRole { RoleId = checkUserRole.Id, UserId = applicationUser.Id });
        }

        return new GeneralResonse(true, "User created successfully");
    }

    public async Task<LoginResponse> SingInAsync(Login? user)
    {
        if (user is null) return new LoginResponse(false, "User is null");

        var applicationUser = await FindUserByEmail(user.Email!);
        if (applicationUser is null) return new LoginResponse(false, "User not found");

        // sprawdzanie hasła
        if (!BCrypt.Net.BCrypt.Verify(user.Password, applicationUser.Password))
            return new LoginResponse(false, "Invalid password");

        //pobieranie roli
        var getUserRole = await FindUserRole(applicationUser.Id);
        if (getUserRole is null) return new LoginResponse(false, "User role not found");

        var getRoleName = await FindRoleName(getUserRole.RoleId);

        var jwtToken = GenerateToken(applicationUser, getRoleName!.Name!);
        var refreshToken = GenerateRefreshToken();

        // zapisanie tokena odświeżającego
        var findUser = await appDbContext.RefreshTokenInfos.FirstOrDefaultAsync(x => x.UserId == applicationUser.Id);
        if (findUser is not null)
        {
            findUser.Token = refreshToken;
            await appDbContext.SaveChangesAsync();
        }
        else
        {
            await AddToDatabase(new RefreshTokenInfo { Token = refreshToken, UserId = applicationUser.Id });
        }

        return new LoginResponse(true, "Login successful", jwtToken, refreshToken);
    }

    public async Task<LoginResponse> RefreshTokenAsync(RefreshToken? token)
    {
        if (token is null) return new LoginResponse(false, "Token is null");

        var findToken = await appDbContext.RefreshTokenInfos.FirstOrDefaultAsync(x => x.Token!.Equals(token.Token));
        if (findToken is null) return new LoginResponse(false, "Token not found");

        var user = await appDbContext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == findToken.UserId);
        if (user is null)
            return new LoginResponse(false, "Refresh token could not be generated because the user was not found");

        var userRole = await FindUserRole(user.Id);
        var roleName = await FindRoleName(userRole!.RoleId);
        var jwtToken = GenerateToken(user, roleName!.Name!);
        var refreshToken = GenerateRefreshToken();

        var updateRefreshToken = await appDbContext.RefreshTokenInfos.FirstOrDefaultAsync(r => r.UserId == user.Id);
        if (updateRefreshToken is null)
            return new LoginResponse(false, "Refresh token could not be generated because the user has not signed in");

        updateRefreshToken.Token = refreshToken;
        await appDbContext.SaveChangesAsync();
        return new LoginResponse(true, "Token refreshed successfully", jwtToken, refreshToken);
    }

    private async Task<UserRole?> FindUserRole(int userId)
    {
        return await appDbContext.UserRoles.FirstOrDefaultAsync(x => x.UserId == userId)!;
    }

    private async Task<SystemRole?> FindRoleName(int roleId)
    {
        return await appDbContext.SystemRoles.FirstOrDefaultAsync(x => x.Id == roleId)!;
    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    private string GenerateToken(ApplicationUser user, string role)
    {
        //generowanie tokena
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Value.Key!));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var userClaims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Role, role!)
        };
        var token = new JwtSecurityToken(
            config.Value.Issuer,
            config.Value.Audience,
            userClaims,
            expires: DateTime.Now.AddSeconds(30),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<T> AddToDatabase<T>(T model)
    {
        var reult = appDbContext.Add(model!);
        await appDbContext.SaveChangesAsync();
        return (T)reult.Entity;
    }

    private async Task<ApplicationUser?> FindUserByEmail(string userEmail)
    {
        return await appDbContext.ApplicationUsers.FirstOrDefaultAsync(x =>
            x.Email!.ToLower()!.Equals(userEmail!.ToLower()))!;
    }
}