using webapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly SampleDBContext _context;

        public PatientController(SampleDBContext context)
        {
            _context = context;
        }

        [HttpGet("WithFilter")]
        public IActionResult GetPatients([FromQuery] string? firstname, [FromQuery] string? lastname)
        {
            IQueryable<Patient> patients = _context.Patient;

            List<Patient> tempList = new List<Patient>();

            if (!string.IsNullOrEmpty(firstname))
            {
                tempList.AddRange(patients.Where(h => h.FirstName.Contains(firstname)).ToList());
            }

            if (!string.IsNullOrEmpty(lastname))
            {
                tempList.AddRange(patients.Where(h => h.LastName.Contains(lastname)).ToList());
            }

            if (string.IsNullOrEmpty(firstname) && string.IsNullOrEmpty(lastname))
            {
                tempList = patients.ToList();
            }

            return Ok(tempList);
        }

        [HttpGet("WithFilterForDoctor")]
        public IActionResult GetPatientsWithFilterForDoctor([FromQuery] string? firstname, [FromQuery] string? lastname)
        {
            IQueryable<Patient> patients = _context.Patient.Where(h => h.Activity == true);

            List<Patient> tempList = new List<Patient>();

            if (!string.IsNullOrEmpty(firstname))
            {
                tempList.AddRange(patients.Where(h => h.FirstName.Contains(firstname)).ToList());
            }

            if (!string.IsNullOrEmpty(lastname))
            {
                tempList.AddRange(patients.Where(h => h.LastName.Contains(lastname)).ToList());
            }

            if (string.IsNullOrEmpty(firstname) && string.IsNullOrEmpty(lastname))
            {
                tempList = patients.ToList();
            }

            return Ok(tempList);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Patient>> GetPatientList()
        {
            return _context.Patient.ToList();
        }

        [HttpGet("UserName/{UserName}")]
        public ActionResult<Patient> GetPatientByUserName(string username)
        {
            var patient = _context.Patient.FirstOrDefault(h => h.FirstName == username && h.Activity == true);
            if (patient == null)
            {
                return NotFound();
            }
            return patient;
        }

        [HttpGet("Password/{Password}")]
        public ActionResult<Patient> GetPatientByPassword(string password)
        {
            var patient = _context.Patient.FirstOrDefault(h => h.Password == password && h.Activity == true);
            if (patient == null)
            {
                return NotFound();
            }
            return patient;
        }

        [HttpGet("{id}")]
        public ActionResult<Patient> GetPatientById(int id)
        {
            var patient = _context.Patient.Find(id);
            if (patient == null || !patient.Activity)
            {
                return NotFound();
            }
            return patient;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] PatientRequest loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var patient = _context.Patient
                .FirstOrDefault(a => a.Email == loginModel.Email && a.Password == loginModel.Password && a.Activity == true);

            if (patient == null)
            {
                return Ok(new { PatientInfo = (Patient)null, success = false });
            }

            return Ok(new { PatientInfo = patient, success = true });
        }

        [HttpPost]
        public ActionResult<Patient> CreatePatient(Patient patient)
        {
            if (patient == null)
            {
                return BadRequest();
            }
            _context.Patient.Add(patient);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePatient(int id, [FromBody] Patient updatedPatient)
        {
            if (updatedPatient == null || id <=0)
            {
                return BadRequest();
            }

            var existingPatient = _context.Patient.Find(id);
            if (existingPatient == null )
            {
                return NotFound();
            }
            //  existingHasta.HastaId = updatedHasta.HastaId;

            existingPatient.FirstName = updatedPatient.FirstName;
            existingPatient.LastName = updatedPatient.LastName;
            existingPatient.Email = updatedPatient.Email;
            existingPatient.Password = updatedPatient.Password;
            existingPatient.Activity = updatedPatient.Activity;

            _context.Patient.Update(existingPatient);
            _context.SaveChanges();

            return Ok("Güncelleme başarılı.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePatient(int id)
        {
            var existingPatient = _context.Patient.Find(id);
            if (existingPatient == null)
            {
                return NotFound();
            }

            existingPatient.Activity = false;
            _context.Patient.Update(existingPatient);
            _context.SaveChanges();

            return Ok("Silme işlemi başarılı.");
        }
    }

    public class PatientRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
