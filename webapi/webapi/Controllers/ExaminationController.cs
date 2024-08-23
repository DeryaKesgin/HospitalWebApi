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
        [HttpPost("AddExamination")]
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

            var patient = _context.Patient.FirstOrDefault(h => h.PatientId == examination.PatientId);
            var doctor = _context.Doctor.FirstOrDefault(d => d.DoctorId == examination.DoctorId);

            if (patient == null)
            {
                return BadRequest($"Patient with ID {examination.PatientId} does not exist");
            }

            if (doctor == null)
            {
                return BadRequest($"Doctor with ID {examination.DoctorId} does not exist");
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
            List<Patient> patientList = _context.Patient.ToList();
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

                List<Patient> list = patientList.Where(x => x.PatientId == item.PatientId).ToList();
                if (list.Count > 0)
                {
                    obj.PatientFirstName = list[0].FirstName;
                    obj.PatientLastName = list[0].LastName;
                }
                List<Doctor> list2 = doctorList.Where(x => x.DoctorId == item.DoctorId).ToList();
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
        [HttpGet("History/{PatientId}")]
        public IActionResult GetExaminationHistory(int PatientId)
        {
            var doctorIds = _context.Examination
                .Where(e => e.PatientId == PatientId)
                .Select(e => e.DoctorId)
                .Distinct()
                .ToList();

            var doctors = _context.Doctor
                .Where(d => doctorIds.Contains(d.DoctorId))
                .ToDictionary(d => d.DoctorId);

            var examinations = _context.Examination
                .Where(e => e.PatientId == PatientId)
                .Select(e => new ExaminationView
                {
                    Id = e.Id,
                    PatientId = e.PatientId,
                    DoctorId = e.DoctorId,
                    Complaint = e.Complaint,
                    Diagnosis = e.Diagnosis,
                    Prescription = e.Prescription,
                    DateCreated = e.DateCreated,
                    DoctorFirstName = doctors.ContainsKey(e.DoctorId) ? doctors[e.DoctorId].FirstName : "Unknown",
                    DoctorLastName = doctors.ContainsKey(e.DoctorId) ? doctors[e.DoctorId].LastName : "Unknown"
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
