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
    }
}
