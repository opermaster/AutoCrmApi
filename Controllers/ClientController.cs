using AutoCrmApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace AutoCrmApi.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class ClientController:ControllerBase
    {
        private readonly DatabaseContext _context;
        public ClientController(DatabaseContext context) {
            _context = context;
        }
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public ActionResult CreateClient(ClientDto _client) {
            bool exist = _context.Clients.Any(u => u.Phone == _client.Phone);
            if (exist) return Conflict("User with this phone number already exists");
            Client client = _client.ToClient();
            _context.Clients.Add(client);
            _context.SaveChanges();
            return Created(nameof(CreateClient), new { id = client.Id, });
        }
    }
}
