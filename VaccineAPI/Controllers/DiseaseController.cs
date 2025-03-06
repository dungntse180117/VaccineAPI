using Microsoft.AspNetCore.Mvc;
using VaccineAPI.BusinessLogic.Services.Interface;
using VaccineAPI.BusinessLogic.Services.Implement;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;

namespace VaccineAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiseaseController : ControllerBase
    {
        private readonly IDiseaseService _diseaseService;

        public DiseaseController(IDiseaseService diseaseService)
        {
            _diseaseService = diseaseService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiseaseResponse>>> GetAllDiseases()
        {
            var diseases = await _diseaseService.GetAllDiseases();
            return Ok(diseases);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DiseaseResponse>> GetDiseaseById(int id)
        {
            var disease = await _diseaseService.GetDiseaseById(id);

            if (disease == null)
            {
                return NotFound();
            }

            return Ok(disease);
        }

        [HttpPost]
        public async Task<ActionResult<DiseaseResponse>> CreateDisease(CreateDiseaseRequest createDiseaseRequest)
        {
            var disease = await _diseaseService.CreateDisease(createDiseaseRequest);

            return CreatedAtAction(nameof(GetDiseaseById), new { id = disease.DiseaseId }, disease);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDisease(int id, UpdateDiseaseRequest updateDiseaseRequest)
        {
            var disease = await _diseaseService.UpdateDisease(id, updateDiseaseRequest);

            if (disease == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDisease(int id)
        {
            bool deleted = await _diseaseService.DeleteDisease(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}