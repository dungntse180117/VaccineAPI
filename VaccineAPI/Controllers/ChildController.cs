// Controllers/ChildController.cs
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VaccineAPI.DataAccess.Models;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;
namespace VaccineAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChildController : ControllerBase
    {
        private readonly IChildService _childService;

        public ChildController(IChildService childService)
        {
            _childService = childService;
        }

        [HttpGet]
        public ActionResult<List<ChildResponse>> GetAllChildren()
        {
            return Ok(_childService.GetAllChildren());
        }

        [HttpGet("{id}")]
        public ActionResult<ChildResponse> GetChildById(int id)
        {
            var child = _childService.GetChildById(id);
            if (child == null)
            {
                return NotFound();
            }
            return Ok(child);
        }

        [HttpPost]
        public ActionResult<ChildResponse> CreateChild(ChildRequest child)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newChild = _childService.CreateChild(child);
            return CreatedAtAction(nameof(GetChildById), new { id = newChild.ChildId }, newChild);
        }

        [HttpPut("{id}")]
        public ActionResult<ChildResponse> UpdateChild(int id, ChildRequest child)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedChild = _childService.UpdateChild(id, child);
            if (updatedChild == null)
            {
                return NotFound();
            }
            return Ok(updatedChild);
        }

        [HttpDelete("{id}")]
        public ActionResult NoContent(int id)
        {
            _childService.DeleteChild(id);
            return NoContent();
        }
    }
}