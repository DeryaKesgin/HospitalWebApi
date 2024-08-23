using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models
{
    public class Examination
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int PatientId { get; set; }
        public int DoctorId { get; set; }

        [MaxLength(500)]
        public string Complaint { get; set; }

        [MaxLength(500)]
        public string Diagnosis { get; set; }

        [MaxLength(500)]
        public string Prescription { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
