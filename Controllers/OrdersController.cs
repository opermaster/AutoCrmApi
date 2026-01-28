using AutoCrmApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AutoCrmApi.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public OrdersController(DatabaseContext context) {
            _context = context;
        }
        [Authorize(Roles = "Master,Admin")]
        [HttpGet]
        public ActionResult<List<OrderResponseDto>> GetOrders() {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var isAdmin = User.IsInRole("Admin");

            var orders = _context.Orders
                .Include(o => o.Auto)
                .Include(o => o.Services).ThenInclude(os => os.Service)
                .Include(o => o.Services).ThenInclude(os => os.Master)
                .Include(o => o.Parts).ThenInclude(op => op.Part)
                .AsNoTracking()
                .ToList();

            var result = orders
                .Select(o => OrderResponseDto.MapToDto(
                    o,
                    isAdmin ? null : userId,
                    isAdmin
                ))
                .Where(dto => isAdmin || dto.Services.Any())
                .ToList();

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public ActionResult DeleteOrder(int id) {
            Order? order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if(order is not null) {
                _context.Orders.Remove(order);
                _context.SaveChanges();
                return NoContent();
            }
            return BadRequest();
        }
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public ActionResult CreateOrder(OrderDto _order) {
            Auto? auto = _context.Autos.FirstOrDefault(a => a.Number == _order.Number);
            if (auto is null) return BadRequest("Car with this number does not exist!");
            Order order = _order.ToOrder();
            order.AutoId = auto.Id;

            _context.Orders.Add(order);
            _context.SaveChanges();

            return Created(nameof(CreateOrder), new { id = order.Id, });
        }
        [Authorize(Roles = "Manager")]
        [HttpPost("add-parts")]
        public ActionResult AddParts(OrderPartsDto _parts) {
            Order? order = _context.Orders.FirstOrDefault(o => o.Id == _parts.OrderId);

            if (order is null) return BadRequest("Order with this Id does not exist!");
            
            for(int i = 0; i < _parts.PartIds.Count; i++) {
                OrderPart op = new OrderPart {
                    OrderId = order.Id,
                    PartId = _parts.PartIds[i],
                    Quantity = _parts.Quantities[i],
                };
                _context.OrderParts.Add(op);
            }
            _context.SaveChanges();

            return Ok();
        }
        [Authorize(Roles = "Manager")]
        [HttpPost("add-services")]
        public ActionResult AddServices(OrderServicesDto _services) {
            Order? order = _context.Orders.FirstOrDefault(o => o.Id == _services.OrderId);

            if (order is null) return BadRequest("Order with this Id does not exist!");

            for (int i = 0; i < _services.ServiceIds.Count; i++) {
                OrderService op = new OrderService {
                    OrderId = order.Id,
                    ServiceId = _services.ServiceIds[i],
                    MasterId = _services.MasterIds[i],
                };
                _context.OrderServices.Add(op);
            }
            _context.SaveChanges();

            return Ok();
        }
        [Authorize(Roles = "Master")]
        [HttpPut]
        public ActionResult UpdateServices(OrderUpdateDto dto) {
            Order? order = _context.Orders.FirstOrDefault(o => o.Id == dto.OrderId);
            if (order is null) return BadRequest("Order with this Id does not exist!");
            _context.Orders
                    .Where(o => o.Id == dto.OrderId)
                    .ExecuteUpdate(setters => setters
                        .SetProperty(o => o.Status, dto.Status)
                        .SetProperty(o => o.Comment, dto.Comment)
                        .SetProperty(
                            o => o.CompletedAt,
                            dto.Status == OrderStatus.Completed ? DateTime.UtcNow : null
                        )
                    );

            var orderServices = _context.OrderServices
                .Where(os => os.OrderId == dto.OrderId)
                .ToList();

            foreach (var os in orderServices) {
                var dtoService = dto.ServicesDone
                    .FirstOrDefault(s => s.ServiceId == os.ServiceId);

                if (dtoService is not null) {
                    os.Done = dtoService.Done;
                }
            }
            _context.SaveChanges();
            return Ok();
        }
    }
}
