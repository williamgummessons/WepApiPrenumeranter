using Microsoft.AspNetCore.Mvc;
using WepApiPrenumeranter.Models;

namespace WepApiPrenumeranter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrenumeranterController : ControllerBase
    {
        private readonly PrenumeranterMethods prenumeranterMethods;

        public PrenumeranterController(IConfiguration configuration)
        {
            prenumeranterMethods = new PrenumeranterMethods(configuration);
        }
        //Halloj
        [HttpGet("prenumerant/{prennr}", Name = "GetPrenumerant")]
        public IActionResult GetPrenumerant(int prennr)
        {
            var prenumerant = prenumeranterMethods.GetPrenumerant(prennr, out string errormsg);

            if (prenumerant == null)
            {
                if (!string.IsNullOrEmpty(errormsg) && errormsg.Contains("not found"))
                {
                    return NotFound(errormsg);
                }
                return StatusCode(500, errormsg);
            }

            return Ok(prenumerant);
        }

        [HttpPut("prenumerant/{prennr}", Name = "EditPrenumerant")]
        public IActionResult EditPrenumerant(int prennr, Prenumeranter prenumerant)
        {
            if (prennr != prenumerant.Prennr)
            {
                return BadRequest("Prennr in URL does not match Prennr in body.");
            }

            var updatedPrenumerant = prenumeranterMethods.EditPrenumerant(prenumerant, out string errormsg);

            if (updatedPrenumerant == null)
            {
                if (!string.IsNullOrEmpty(errormsg) && errormsg.Contains("not found"))
                {
                    return NotFound(errormsg);
                }
                return StatusCode(500, errormsg);
            }
            return Ok(updatedPrenumerant);
        }
    }
}
