using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Bormech.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaintenanceController : ControllerBase
{
    [HttpPost("restart")]
    public IActionResult RestartApplication()
    {
        try
        {
            // Wywołanie polecenia restartu w systemctl
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "systemctl",
                    Arguments = "status your-service-name",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            process.WaitForExit();

            if (process.ExitCode == 0)
            {
                return Ok("Application restart initiated successfully.");
            }
            else
            {
                string error = process.StandardError.ReadToEnd();
                return StatusCode(500, $"Failed to restart application: {error}");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error occurred: {ex.Message}");
        }
    }
}