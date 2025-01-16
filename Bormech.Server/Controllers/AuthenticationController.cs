using Bormech.Data.DTOs;
using Bormech.Server.Liblary.Reporitories.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Bormech.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IUserAccount accountInterface) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> CreateAsync(Register? user)
        {
            if (user == null) return BadRequest("User is null");
            var result = await accountInterface.CreateAsync(user);
            return Ok(result);
        }
        /// <summary>
        /// Logs in a user, returning a new access token
        /// </summary>
        /// <param name="user">The user to log in</param>
        /// <returns>A JSON response with the new access token</returns>
        [HttpPost("login")]
        public async Task<IActionResult> SingInAsync(Login? user)
        {
            if (user == null) return BadRequest("User is null");
            var result = await accountInterface.SingInAsync(user);
            return Ok(result);
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync(RefreshToken? token)
        {
            if (token == null) return BadRequest("Token is null");
            var result = await accountInterface.RefreshTokenAsync(token);
            return Ok(result);
        }
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePasswordAsync(ChangePassword? changePassword)
        {
            if (changePassword == null) return BadRequest("ChangePassword is null");
            var result = await accountInterface.ChangePasswordAsync(changePassword);
            return Ok(result);
        }
    }
}
