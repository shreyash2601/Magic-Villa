using MagicVillaAPI.Data;
using MagicVillaAPI.Models;
using MagicVillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVillaAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private string controllerName = "VillaAPI";
        private readonly ILogger<VillaAPIController> logger;
        private readonly IVillaRepository villaRepository;

        public VillaAPIController(ILogger<VillaAPIController> _logger, IVillaRepository _villaRepository)
        {
            logger = _logger;
            villaRepository = _villaRepository;
        }

        [HttpGet("get-all-villas")]
        public async Task<ActionResult<IEnumerable<Villa>>> GetVillas()
        {
            logger.LogInformation($"{controllerName}: Getting all villas...");
            var villas = await villaRepository.GetAllAsync();
            return Ok(villas);
        }

        [HttpGet("get-villa/{id:int}")]
        public async Task<ActionResult<Villa>> GetVilla(int id)
        {
            logger.LogInformation($"{controllerName}: Getting villa with ID {id}...");

            var villa = await villaRepository.GetAsync(v => v.Id == id);
            if (villa == null)
            {
                logger.LogWarning($"{controllerName}: Villa with ID {id} not found.");
                return NotFound();
            }
            return Ok(villa);
        }

        [HttpPost("post-villa")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Villa>> PostVilla([FromBody] Villa villa)
        {
            logger.LogInformation($"{controllerName}: Attempting to add new villa '{villa.Name}'...");
            var existingVilla = await villaRepository.GetAsync(v => v.Name == villa.Name);
            if (existingVilla != null && existingVilla.Name == villa.Name)
            {
                logger.LogWarning($"{controllerName}: Villa with name '{villa.Name}' already exists.");
                return BadRequest();
            }

            await villaRepository.CreateAsync(villa);
            logger.LogInformation($"{controllerName}: Villa '{villa.Name}' added with ID {villa.Id}.");
            return Created();
        }

        [HttpPut("update-villa/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Villa>> EditVilla(int id, [FromBody] Villa villa)
        {
            logger.LogInformation($"{controllerName}: Attempting to update villa with ID {id}...");

            var existingVilla = await villaRepository.GetAsync(v => v.Id == id);
            if (existingVilla == null)
            {
                logger.LogWarning($"{controllerName}: Villa with ID {id} not found for update.");
                return NotFound();
            }

            var existingVillaWithSameName = await villaRepository.GetAsync(v => v.Name == villa.Name && v.Id != id);
            if (existingVillaWithSameName != null)
            {
                logger.LogWarning($"{controllerName}: Another villa with name '{villa.Name}' already exists.");
                return BadRequest("A villa with the same name already exists.");
            }

            existingVilla.Name = villa.Name;
            existingVilla.Details = villa.Details;
            existingVilla.ImageURL = villa.ImageURL;
            existingVilla.Occupancy = villa.Occupancy;
            existingVilla.Rate = villa.Rate;
            existingVilla.Sqft = villa.Sqft;

            await villaRepository.UpdateAsync(existingVilla);

            logger.LogInformation($"{controllerName}: Villa with ID {id} updated successfully.");
            return NoContent();
        }


        [HttpDelete("delete-villa/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteVilla(int id)
        {
            logger.LogInformation($"{controllerName}: Attempting to delete villa with ID {id}...");
            var villa = await villaRepository.GetAsync(obj => obj.Id == id);
            if (villa != null)
            {
                await villaRepository.RemoveAsync(villa);
                logger.LogInformation($"{controllerName}: Villa with ID {id} deleted successfully.");
            }
            else
            {
                logger.LogWarning($"{controllerName}: Villa with ID {id} not found for deletion.");
            }

            return Ok();
        }
    }
}
