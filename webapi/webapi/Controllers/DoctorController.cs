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

        [HttpGet("kullaniciAdi/{kullaniciAdi}")]
        public ActionResult<Doctor> GetDoctorByUserName(string kullaniciAdi)
        {
            var doctor = _context.Doctor.FirstOrDefault(d => d.FirstName == kullaniciAdi);
            if (doctor == null)
            {
                return NotFound();
            }
            return doctor;
        }

        [HttpGet("sifre/{sifre}")]
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

        [HttpPost("login")]
        public IActionResult Login([FromBody] DoctorRequest loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var doktor = _context.Doctor
                .FirstOrDefault(a => a.Email == loginModel.Email && a.Password == loginModel.Password && a.Activity);

            if (doktor == null)
            {
                return Ok(new { doktorInfo = (Doctor)null, success = false });
            }

            return Ok(new { doktorInfo = doktor, doktorInfo1 = doktor.FirstName, doktorInfo2 = doktor.LastName, success = true });
        }

        [HttpPost]
        public ActionResult<Doctor> CreateDoctor(Doctor doktor)
        {
            if (doktor == null)
            {
                return BadRequest();
            }
            _context.Doctor.Add(doktor);
            _context.SaveChanges();
            return Ok();
        }
        //birdeneme
        [HttpPut("{id}")]
        public IActionResult UpdateDoctor(int id, [FromBody] Doctor updatedDoctor)
        {
            // Mevcut doktoru getir
            var existingDoctor = _context.Doctor.Find(id);
            if (existingDoctor == null)
            {
                return NotFound("Doktor bulunamadı.");
            }

            // Güncelleme yapılacak alanları mevcut bilgilerle güncelle
            existingDoctor.FirstName = updatedDoctor.FirstName ?? existingDoctor.FirstName;
            existingDoctor.LastName = updatedDoctor.LastName ?? existingDoctor.LastName;
            existingDoctor.Email = updatedDoctor.Email ?? existingDoctor.Email;
            existingDoctor.Password = updatedDoctor.Password ?? existingDoctor.Password;
            existingDoctor.Activity = updatedDoctor.Activity;

            _context.Entry(existingDoctor).State = EntityState.Modified;
            _context.SaveChanges();

            // Başarılı güncelleme mesajı gönder
            return Ok(new { message = "Doktor bilgileri başarıyla güncellendi." });
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
