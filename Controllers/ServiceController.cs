using AutoCrmApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoCrmApi.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public ServiceController(DatabaseContext context) {
            _context = context;
        }
        [Authorize(Roles = "Manager,Admin")]
        public ActionResult<List<ServiceDto>> GetServices() {
            return _context.Services.Select(p => new ServiceDto {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                EstimatedTime = p.EstimatedTime
            }).ToList();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateService(ServiceDto _service) {
            Service service = _service.ToService();
            _context.Services.Add(service);
            _context.SaveChanges();
            return Created(nameof(CreateService), new { id = service.Id, });
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult UpdateService(ServiceDto _service) {
            if (_service.Id is not null) {
                Service? service = _context.Services.FirstOrDefault(p => p.Id == _service.Id);
                if (service is not null) {
                    service.Name = _service.Name;
                    service.Price = _service.Price;
                    service.EstimatedTime = _service.EstimatedTime;
                    _context.SaveChanges();
                    return Created(nameof(CreateService), new { id = service.Id, });
                }
                else return Conflict("There is no service with this id!");

            }
            else return Conflict("Id of service wasnt provided!");
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public ActionResult DeleteService(int id) {
            Service? service = _context.Services.Find(id);
            if (service is not null) {
                _context.Services.Remove(service);
                _context.SaveChanges();
                return NoContent();
            }
            else return Conflict("There is no service with this id!");
        }
    }
}
