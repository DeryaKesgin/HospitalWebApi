using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models
{
    public class Doctor {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DoctorId { get; set; }

            [MaxLength(50)]
            public string FirstName { get; set; }

            [MaxLength(50)]
            public string LastName { get; set; }

            [MaxLength(100)]
            public string Email { get; set; }

            [MaxLength(100)]
            public string Password { get; set; }

        public bool Activity { get; set; } = true;

        public Doctor()
        {
            Activity = true;
        }




    }

}

