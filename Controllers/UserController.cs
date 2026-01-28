using AutoCrmApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace AutoCrmApi.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public UserController(DatabaseContext context) {
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult<List<UserDto>> GetUsers() {
            return _context.Users.Select(p => new UserDto {
                Id = p.Id,
                Login = p.Login,
                Role = p.Role,
            }).ToList();
        }
        [Authorize(Roles = "Manager")]
        [HttpGet("masters")]
        public ActionResult<List<UserDto>> GetMasters() {
            return _context.Users.Where(u=>u.Role==UserRole.Master).Select(p => new UserDto {
                Id = p.Id,
                Login = p.Login,
                Role = p.Role,
            }).ToList();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateUser(UserDto _user) {
            User user = _user.ToUser();
            _context.Users.Add(user);
            _context.SaveChanges();
            return Created(nameof(CreateUser), new { id = user.Id, });
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id) {           
            User? user = _context.Users.Find(id);
            if (user is not null) {
                _context.Users.Remove(user);
                _context.SaveChanges();
                return NoContent();
            }
            else return Conflict("There is no user with this id!");

        }
    }
}
