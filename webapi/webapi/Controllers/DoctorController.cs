using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using webapi.Models;
using Microsoft.EntityFrameworkCore;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly SampleDBContext _context;
        private readonly IConfiguration _configuration;

        public DoctorController(SampleDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("WithFilter")]
        public IActionResult GetDoctors([FromQuery] string? firstname, [FromQuery] string? lastname)
        {
            IQueryable<Doctor> doctors = _context.Doctor;

            if (!string.IsNullOrEmpty(firstname))
            {
                doctors = doctors.Where(d => d.FirstName.Contains(firstname));
            }

            if (!string.IsNullOrEmpty(lastname))
            {
                doctors = doctors.Where(d => d.LastName.Contains(lastname));
            }

            return Ok(doctors.ToList());
        }

        [HttpGet]
        public ActionResult<IEnumerable<Doctor>> GetDoctorList()
        {
            return _context.Doctor.ToList();
        }

        [HttpGet("UserName/{UserName}")]
        public ActionResult<Doctor> GetDoctorByUserName(string UserName)
        {
            var doctor = _context.Doctor.FirstOrDefault(d => d.FirstName == UserName);
            if (doctor == null)
            {
                return NotFound();
            }
            return doctor;
        }

        [HttpGet("Password/{Password}")]
        public ActionResult<Doctor> GetDoctorByPassword(string Password)
        {
            var doctor = _context.Doctor.FirstOrDefault(d => d.Password == Password);
            if (doctor == null)
            {
                return NotFound();
            }
            return doctor;
        }

        [HttpGet("{id}")]
        public ActionResult<Doctor> GetDoctorById(int id)
        {
            var doctor = _context.Doctor.Find(id);
            if (doctor == null)
            {
                return NotFound();
            }

            return doctor;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] DoctorRequest LoginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var doctor = _context.Doctor
                .FirstOrDefault(a => a.Email == LoginModel.Email && a.Password == LoginModel.Password && a.Activity);

            if (doctor == null)
            {
                return Unauthorized(); // 401 Unauthorized
            }

            var token = GenerateJwtToken(doctor);
            return Ok(new { DoctorInfo = doctor, token });
        }

        [HttpPost]
        public ActionResult<Doctor> CreateDoctor(Doctor doctor)
        {
            if (doctor == null)
            {
                return BadRequest();
            }
            _context.Doctor.Add(doctor);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateDoctor(int id, [FromBody] Doctor updatedDoctor)
        {
            if (updatedDoctor == null || id <= 0)
            {
                return BadRequest();
            }

            var existingDoctor = _context.Doctor.Find(id);
            if (existingDoctor == null)
            {
                return NotFound();
            }

            existingDoctor.FirstName = updatedDoctor.FirstName;
            existingDoctor.LastName = updatedDoctor.LastName;
            existingDoctor.Email = updatedDoctor.Email;
            existingDoctor.Password = updatedDoctor.Password;
            existingDoctor.Activity = updatedDoctor.Activity;

            _context.Doctor.Update(existingDoctor);
            _context.SaveChanges();

            return Ok("Güncelleme başarılı.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDoctor(int id)
        {
            var existingDoctor = _context.Doctor.Find(id);
            if (existingDoctor == null)
            {
                return NotFound();
            }

            // Silmek yerine Activity değerini false yap
            existingDoctor.Activity = false;

            _context.SaveChanges();
            return Ok(new { message = "Doktor başarıyla silindi." });
        }

        private string GenerateJwtToken(Doctor doctor)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, doctor.Email)
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

    public class DoctorRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
