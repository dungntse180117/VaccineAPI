//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;
//using VaccineAPI.BusinessLogic.Interface;
//using VaccineAPI.Shared.Request;
//using VaccineAPI.Shared.Response;

//namespace VaccineAPI.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class VaccineController : ControllerBase
//    {
//        private readonly IVaccineService _vaccineService;
//        private readonly ILogger<VaccineController> _logger;

//        public VaccineController(IVaccineService vaccineService, ILogger<VaccineController> logger)
//        {
//            _vaccineService = vaccineService;
//            _logger = logger;
//        }

//        [HttpPost]
//        public async Task<IActionResult> CreateVaccine([FromBody] VaccineRequest request)
//        {
//            if (!ModelState.IsValid)
//            {
//                _logger.LogError("Invalid create request");
//                return BadRequest(ModelState);
//            }

//            var response = await _vaccineService.CreateVaccineAsync(request);

//            if (!response.IsSuccess)
//            {
//                _logger.LogError($"Create vaccine failed: {response.Message}");
//                return BadRequest(response);
//            }

//            return CreatedAtAction(nameof(GetVaccine), new { vaccinationId = response.VaccinationId }, response);
//        }

//        [HttpGet("{vaccinationId}")]
//        public async Task<IActionResult> GetVaccine(int vaccinationId)
//        {
//            var response = await _vaccineService.GetVaccineAsync(vaccinationId);

//            if (!response.IsSuccess)
//            {
//                _logger.LogWarning($"Vaccine not found: {vaccinationId}");
//                return NotFound(response);
//            }

//            return Ok(response);
//        }

//        [HttpPut("{vaccinationId}")]
//        public async Task<IActionResult> UpdateVaccine(int vaccinationId, [FromBody] VaccineRequest request)
//        {
//            if (!ModelState.IsValid)
//            {
//                _logger.LogError($"Invalid update request for VaccinationId: {vaccinationId}");
//                return BadRequest(ModelState);
//            }


//            var response = await _vaccineService.UpdateVaccineAsync(vaccinationId, request);

//            if (!response.IsSuccess)
//            {
//                _logger.LogWarning($"Update vaccine failed: {response.Message}, VaccinationId: {vaccinationId}");
//                return NotFound(response);
//            }

//            return Ok(response);
//        }

//        [HttpDelete("{vaccinationId}")]
//        public async Task<IActionResult> DeleteVaccine(int vaccinationId)
//        {
//            var response = await _vaccineService.DeleteVaccineAsync(vaccinationId);

//            if (!response.IsSuccess)
//            {
//                _logger.LogWarning($"Delete vaccine failed: {response.Message}, VaccinationId: {vaccinationId}");
//                return NotFound(response);
//            }

//            return Ok(response);
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAllVaccines()
//        {
//            var vaccines = await _vaccineService.GetAllVaccinesAsync();
//            return Ok(vaccines);
//        }

//        [HttpPost("manage/{vaccinationId}/{action}")]
//        public async Task<IActionResult> ManageVaccine(int vaccinationId, string action, [FromBody] VaccineRequest? updateRequest = null)  //Made updateRequest Nullable
//        {
//            if (string.IsNullOrEmpty(action))
//            {
//                return BadRequest("Action must be specified");
//            }
//            if (action.ToLower() == "update" && updateRequest == null)
//            {
//                return BadRequest("UpdateRequest needed for Update action.");
//            }

//            var response = await _vaccineService.ManageVaccineAsync(vaccinationId, action, updateRequest);

//            if (!response.IsSuccess)
//            {
//                return NotFound(response);
//            }

//            return Ok(response);
//        }
//    }
//}