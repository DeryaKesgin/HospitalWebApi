using webapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly SampleDBContext _context;

        public DoctorController(SampleDBContext context)
        {
            _context = context;
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
        public ActionResult<Doctor> GetDoctorByPassword(string sifre)
        {
            var doctor = _context.Doctor.FirstOrDefault(d => d.Password == sifre);
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
                return Ok(new { doctorInfo = (Doctor)null, success = false });
            }

            return Ok(new { doctorInfo = doctor, doctorInfo1 = doctor.FirstName, doctorInfo2 = doctor.LastName, success = true });
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
        //sonnnnnxdcf


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
    }

    public class DoctorRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
