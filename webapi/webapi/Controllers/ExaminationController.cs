using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Models;
using System.Linq;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExaminationController : ControllerBase
    {
        private readonly SampleDBContext _context;
        public ExaminationController(SampleDBContext context)
        {
            _context = context;
        }

        // Yeni muayene eklemek için
        [HttpPost("addExamination")]
        public IActionResult AddExamination([FromBody] Examination examination)
        {
            if (examination == null)
            {
                return BadRequest("Examination is null");
            }

            if (string.IsNullOrEmpty(examination.Complaint) ||
                string.IsNullOrEmpty(examination.Diagnosis) ||
                string.IsNullOrEmpty(examination.Prescription))
            {
                return BadRequest("Examination data is incomplete");
            }

            var patient = _context.Hasta.FirstOrDefault(h => h.HastaId == examination.HastaId);
            var doctor = _context.Doctor.FirstOrDefault(d => d.DoktorId == examination.DoktorId);

            if (patient == null)
            {
                return BadRequest($"Patient with ID {examination.HastaId} does not exist");
            }

            if (doctor == null)
            {
                return BadRequest($"Doctor with ID {examination.DoktorId} does not exist");
            }

            try
            {
                _context.Examination.Add(examination);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return Ok("Examination successfully added");
        }

        [HttpGet("WithFilter")]
        public IActionResult GetExaminations([FromQuery] string? diagnosis)
        {
            List<Hasta> hastaList = _context.Hasta.ToList();
            List<Doctor> doctorList = _context.Doctor.ToList();
            List<Examination> examinationList = _context.Examination.ToList();
            if (!string.IsNullOrEmpty(diagnosis))
            {
                examinationList = examinationList.Where(x => x.Diagnosis.Contains(diagnosis)).ToList();
            }

            List<ExaminationView> tempList = new List<ExaminationView>();
 
            foreach (Examination item in examinationList)
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(item);
                ExaminationView obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ExaminationView>(json);

                List<Hasta> list = hastaList.Where(x => x.HastaId == item.HastaId).ToList();
                if (list.Count > 0)
                {
                    obj.HastaFirstName = list[0].FirstName;
                    obj.HastaLastName = list[0].LastName;
                }
                List<Doctor> list2 = doctorList.Where(x => x.DoktorId == item.DoktorId).ToList();
                if (list2.Count > 0)
                {
                    obj.DoctorFirstName = list2[0].FirstName;
                    obj.DoctorLastName = list2[0].LastName;
                }
                tempList.Add(obj);
            }

            return Ok(tempList);
        }




        // Hasta ID'sine göre muayene geçmişini getirmek için
        [HttpGet("history/{patientId}")]
        public IActionResult GetExaminationHistory(int patientId)
        {
            var doctorIds = _context.Examination
                .Where(e => e.HastaId == patientId)
                .Select(e => e.DoktorId)
                .Distinct()
                .ToList();

            var doctors = _context.Doctor
                .Where(d => doctorIds.Contains(d.DoktorId))
                .ToDictionary(d => d.DoktorId);

            var examinations = _context.Examination
                .Where(e => e.HastaId == patientId)
                .Select(e => new ExaminationView
                {
                    Id = e.Id,
                    HastaId = e.HastaId,
                    DoktorId = e.DoktorId,
                    Complaint = e.Complaint,
                    Diagnosis = e.Diagnosis,
                    Prescription = e.Prescription,
                    DateCreated = e.DateCreated,
                    DoctorFirstName = doctors.ContainsKey(e.DoktorId) ? doctors[e.DoktorId].FirstName : "Unknown",
                    DoctorLastName = doctors.ContainsKey(e.DoktorId) ? doctors[e.DoktorId].LastName : "Unknown"
                })
                .ToList();

            if (examinations == null || examinations.Count == 0)
            {
                return NotFound("No examination history found for this patient");
            }

            return Ok(examinations);
        }

    }
}
