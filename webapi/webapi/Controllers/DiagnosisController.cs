using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosisController : ControllerBase
    {
        private readonly SampleDBContext _context;
        public DiagnosisController(SampleDBContext context)
        {
            _context = context;
        }




        [HttpPost("AddDiagnosis")]
        public IActionResult AddDiagnosis([FromBody] Diagnosis diagnosis)
        {
            if (diagnosis == null)
            {
                return BadRequest("Diagnosis is null");
            }

            var patient = _context.Patient.FirstOrDefault(h => h.PatientId == diagnosis.PatientId);
            var doctor = _context.Doctor.FirstOrDefault(d => d.DoctorId == diagnosis.DoctorId);

            if (patient == null || doctor == null)
            {
                return BadRequest("Invalid patient or doctor ID");
            }

            _context.Diagnosis.Add(diagnosis);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("ByPatient/{PatientId}")]
        public IActionResult GetDiagnosesByPatient(int PatientId)
        {
            var diagnoses = _context.Diagnosis.Where(d => d.PatientId == PatientId).ToList();
            return Ok(diagnoses);
        }


    }
}
