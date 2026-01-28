using AutoCrmApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AutoCrmApi.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly DatabaseContext _context;
        public AuthController(DatabaseContext context) {
            _context = context;
        }
        [Authorize(Roles="Admin")]
        [HttpPost("new_user")]
        public ActionResult Register(UserDto _user) {
            bool exist = _context.Users.Any(u => u.Login == _user.Login);
            if (exist) return Conflict("User with this login already exists");
            User user = _user.ToUser();
            _context.Users.Add(user);
            _context.SaveChanges();
            return Created(nameof(Register), new { id = user.Id, });
        }

        [HttpPost("login")]
        public ActionResult Login(UserDto _user) {
            //UserDto _u = new UserDto();
            //_u.Login = "admin";
            //_u.Password = "apass";
            //_u.Role = UserRole.Admin;
            //User u = _u.ToUser();
            //_context.Users.Add(u);
            //_context.SaveChanges();
            User? user = _context.Users.FirstOrDefault(u => u.Login == _user.Login);
            if(user is null ) return Unauthorized("Ivalid login");

            if(!UserDto.VerifyPassword(_user.Password, user.PasswordHash)) {
                return Unauthorized("Ivalid password");
            }
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(30)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            

            return Ok(new { Token = token, Role = user.Role.ToString() });

        }
    }
}
