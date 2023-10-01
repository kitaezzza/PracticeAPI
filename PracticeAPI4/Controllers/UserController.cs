using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PracticeAPI4.DB;
using PracticeAPI4.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PracticeAPI4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly APIContext _context;
        private readonly IConfiguration _configuration;

        public UserController(APIContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("REGISTER")]
        public IActionResult Register([FromBody] User user)
        {
            try
            {
                var existentuser = _context.Users.FirstOrDefault(u => u.Name == user.Name);
                if (existentuser != null)
                    return BadRequest("This user is existing already.");

                _context.Users.Add(user);
                _context.SaveChanges();
                return Ok("User was registered successful.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost("LOGIN")]
        public IActionResult Authorize([FromBody] User user)
        {
            try
            {
                var existentuser = _context.Users.SingleOrDefault(u => u.Name == user.Name && u.Password == user.Password);
                if (existentuser == null)
                    return Unauthorized("Uncorrent username or password. Try again.");

                string token = GenerateToken(existentuser);
                existentuser.Token = token;
                _context.SaveChanges();

                return Ok(existentuser.Token);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        private string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name!),
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("APIContext:Token").Value!));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
