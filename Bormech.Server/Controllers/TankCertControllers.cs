using Bormech.Data.DTOs.TankCertification;
using Bormech.Data.Entities.Approvals;
using Bormech.Server.Liblary.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Bormech.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TankCertControllers : ControllerBase
    {
        [HttpGet("types")]
        public IActionResult GetTypes()
        {
            // var type = Enum.GetNames(typeof(TankCertificationType));
            // return Ok(type); 
            
            var types = Enum.GetValues(typeof(TankCertificationType))
                .Cast<TankCertificationType>()
                .Select<TankCertificationType, object>(r => new
                {
                    Value = r,
                    DisplayName = EnumHelper.GetDisplayName(r)
                });

            return Ok(types);
        }


        [HttpPost("test")]
        public IActionResult Test(TankCertificationDto test)
        {
            Console.WriteLine(test);
            return Ok(); 
        }
    }
}
