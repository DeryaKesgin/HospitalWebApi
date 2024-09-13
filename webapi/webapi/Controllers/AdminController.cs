using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using webapi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly SampleDBContext _context;
        private readonly IConfiguration _configuration;

        public AdminController(SampleDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Admin
        [HttpGet]
        public ActionResult<IEnumerable<Admin>> GetList()
        {
            return _context.Admin.ToList();
        }

        // GET: api/Admin/UserName/{UserName}
        [HttpGet("UserName/{UserName}")]
        public ActionResult<Admin> GetUsername(string UserName)
        {
            var admin = _context.Admin.Where((hast) => hast.FirstName == UserName).FirstOrDefault();
            if (admin == null)
            {
                return NotFound();
            }
            return admin;
        }

        // GET: api/Admin/Password/{Password}
        [HttpGet("Password/{Password}")]
        public ActionResult<Admin> GetPassword(string Password)
        {
            var admin = _context.Admin.Where((hast) => hast.Password == Password).FirstOrDefault();
            if (admin == null)
            {
                return NotFound();
            }
            return admin;
        }

        // POST: api/Admin/Login
        [HttpPost("Login")]
        public IActionResult Login([FromBody] AdminRequest LoginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var admin = _context.Admin
                .FirstOrDefault(a => a.Email == LoginModel.Email && a.Password == LoginModel.Password);

            if (admin == null)
            {
                return Unauthorized(); // 401 Unauthorized
            }

            var token = GenerateJwtToken(admin);
            return Ok(new { AdminInfo = admin, token });
        }

        // POST: api/Admin
        [HttpPost]
        public ActionResult<Admin> CreateAdmin(Admin admin)
        {
            if (admin == null)
            {
                return BadRequest();
            }

            _context.Admin.Add(admin);
            _context.SaveChanges();
            return Ok();
        }

        private string GenerateJwtToken(Admin admin)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, admin.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Jwt:ExpiryMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public class AdminRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
