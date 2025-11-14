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
       
        [HttpGet("prenumerant/{preNr}", Name = "GetPrenumerant")]
        public IActionResult GetPrenumerant(int preNr)
        {
            var prenumerant = prenumeranterMethods.GetPrenumerant(preNr, out string errormsg);

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

        [HttpPut("prenumerant/{preNr}", Name = "EditPrenumerant")]
        public IActionResult EditPrenumerant(int preNr, PrenumeranterDetails prenumerant)
        {
            if (preNr != prenumerant.pr_preNr)
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
