using AutoCrmApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace AutoCrmApi.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class PartController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public PartController(DatabaseContext context) {
            _context = context;
        }
        [Authorize(Roles = "Manager,Admin")]
        [HttpGet("all_parts")]
        public ActionResult<List<PartDto>> GetParts() {
            return _context.Parts.Select(p=> new PartDto {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
            }).ToList();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("new-part")]
        public ActionResult CreatePart(PartDto _part) {
            Part part = _part.ToPart();
            _context.Parts.Add(part);
            _context.SaveChanges();
            return Created(nameof(CreatePart), new { id = part.Id, });
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("update-part")]
        public ActionResult UpdatePart(PartDto _part) {
            if (_part.Id is not null) {
                Part? part = _context.Parts.FirstOrDefault(p =>p.Id ==_part.Id);
                if(part is not null) {
                    part.Name = _part.Name;
                    part.Price= _part.Price;
                    _context.SaveChanges();
                    return Created(nameof(CreatePart), new { id = part.Id, });
                } else return Conflict("There is no part with this id!");

            }
            else return Conflict("Id of part wasnt provided!");
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("/by-partid/{id}")]
        public ActionResult DeletePart(int id) {
            Part? part = _context.Parts.Find(id);
            if (part is not null) {
                _context.Parts.Remove(part);
                _context.SaveChanges();
                return NoContent();
            }
            else return Conflict("There is no part with this id!");

        }
    }
}
