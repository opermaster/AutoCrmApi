using AutoCrmApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace AutoCrmApi.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class AutoController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public AutoController(DatabaseContext context) {
            _context = context;
        }
        [Authorize(Roles = "Manager")]
        [HttpPost("new_auto")]
        public ActionResult CreateAuto(AutoDto _auto) {
            if (_context.Autos.Any(a => a.Number == _auto.Number)) return Conflict("Auto with this number already exist!");
            Auto auto = _auto.ToAuto();
            Client? client = _context.Clients.FirstOrDefault(c => c.Phone == _auto.PhoneNumber);
            if (client is null) return Conflict("User with this Phone number does not exist!");

            auto.ClientId = client.Id;
            _context.Autos.Add(auto);
            _context.SaveChanges();

            return Created(nameof(CreateAuto), new { id = auto.Id, });
        }
    }
}
