// Models/Diagnosis.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models
{
    public class Diagnosis
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }   
        public int HastaId { get; set; }
        public int DoktorId { get; set; }
        
        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
