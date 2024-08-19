using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly SampleDBContext _context;
        public PrescriptionController(SampleDBContext context)
        {
            _context = context;
        }





        [HttpPost("addPrescription")]
        public IActionResult AddPrescription([FromBody] Prescription prescription)
        {
            if (prescription == null)
            {
                return BadRequest("Prescription is null");
            }

            var patient = _context.Hasta.FirstOrDefault(h => h.HastaId == prescription.HastaId);
            var doctor = _context.Doctor.FirstOrDefault(d => d.DoktorId == prescription.DoktorId);

            if (patient == null || doctor == null)
            {
                return BadRequest("Invalid patient or doctor ID");
            }

            _context.Prescription.Add(prescription);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("ByPatient/{hastaId}")]
        public IActionResult GetPrescriptionsByPatient(int hastaId)
        {
            var prescriptions = _context.Prescription.Where(p => p.HastaId == hastaId).ToList();
            return Ok(prescriptions);
        }


    }
}
