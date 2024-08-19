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




        [HttpPost("addDiagnosis")]
        public IActionResult AddDiagnosis([FromBody] Diagnosis diagnosis)
        {
            if (diagnosis == null)
            {
                return BadRequest("Diagnosis is null");
            }

            var patient = _context.Hasta.FirstOrDefault(h => h.HastaId == diagnosis.HastaId);
            var doctor = _context.Doctor.FirstOrDefault(d => d.DoktorId == diagnosis.DoktorId);

            if (patient == null || doctor == null)
            {
                return BadRequest("Invalid patient or doctor ID");
            }

            _context.Diagnosis.Add(diagnosis);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("ByPatient/{hastaId}")]
        public IActionResult GetDiagnosesByPatient(int hastaId)
        {
            var diagnoses = _context.Diagnosis.Where(d => d.HastaId == hastaId).ToList();
            return Ok(diagnoses);
        }


    }
}
