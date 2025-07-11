using MagicVillaAPI.Models;
using MagicVillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MagicVillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaNumberAPIController : ControllerBase
    {
        private string controllerName = "VillaAPI";
        private readonly ILogger<VillaNumberAPIController> logger;
        private readonly IVillaNumberRepository villaNumberRepository;

        public VillaNumberAPIController(ILogger<VillaNumberAPIController> _logger, IVillaNumberRepository _villaNumberRepository)
        {
            logger = _logger;
            villaNumberRepository = _villaNumberRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VillaNumber>>> GetVillas()
        {
            logger.LogInformation($"{controllerName}: Getting all villas...");
            var villaNumbers = await villaNumberRepository.GetAllAsync();
            return Ok(villaNumbers);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<VillaNumber>> GetVilla(int id)
        {
            logger.LogInformation($"{controllerName}: Getting villa with ID {id}...");
            var villaNumber = await villaNumberRepository.GetAsync(v => v.VillaNo == id);
            if (villaNumber == null)
            {
                logger.LogWarning($"{controllerName}: Villa with ID {id} not found.");
                return NotFound();
            }
            return Ok(villaNumber);
        }

        [HttpPost]
        public async Task<ActionResult<VillaNumber>> PostVilla([FromBody] VillaNumber villaNumber)
        {
            var existingVillaNumber = await villaNumberRepository.GetAsync(v => v.VillaNo == villaNumber.VillaNo && v.VillaId == villaNumber.VillaId);
            if (existingVillaNumber != null)
            {
                logger.LogWarning($"{controllerName}: Villa number already exists.");
                return BadRequest();
            }

            await villaNumberRepository.CreateAsync(villaNumber);
            logger.LogInformation($"{controllerName}: Villa '{villaNumber.VillaNo}' added for villa {villaNumber.VillaId}.");
            return Created();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<VillaNumber>> EditVilla(int id, [FromBody] VillaNumber villaNumber)
        {
            logger.LogInformation($"{controllerName}: Attempting to update villa number with number {id}...");

            var existingVillaNumber = await villaNumberRepository.GetAsync(v => v.VillaNo == id);
            if (existingVillaNumber == null)
            {
                logger.LogWarning($"{controllerName}: Villa number with ID {id} not found for update.");
                return NotFound();
            }

            var existingVillaWithSameNumber = await villaNumberRepository.GetAsync(v => v.VillaNo == villaNumber.VillaNo && v.VillaId == villaNumber.VillaId);
            if (existingVillaWithSameNumber != null)
            {
                logger.LogWarning($"{controllerName}: Another villa with number '{villaNumber.VillaNo}' already exists.");
                return BadRequest("A villa with the same name already exists.");
            }

            existingVillaNumber.VillaId = villaNumber.VillaId;
            existingVillaNumber.SpecialDetails = villaNumber.SpecialDetails;

            await villaNumberRepository.UpdateAsync(existingVillaNumber);

            logger.LogInformation($"{controllerName}: Villa number with ID {id} updated successfully.");
            return NoContent();
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteVilla(int id)
        {
            logger.LogInformation($"{controllerName}: Attempting to delete villa number with ID {id}...");
            var villa = await villaNumberRepository.GetAsync(obj => obj.VillaNo == id);
            if (villa != null)
            {
                await villaNumberRepository.RemoveAsync(villa);
                logger.LogInformation($"{controllerName}: Villa number with ID {id} deleted successfully.");
            }
            else
            {
                logger.LogWarning($"{controllerName}: Villa number with ID {id} not found for deletion.");
            }

            return Ok();
        }
    }
}
