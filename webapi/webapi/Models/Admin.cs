using System.ComponentModel.DataAnnotations;

namespace webapi.Models
{
    public class Admin
    {
        public int AdminId { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]

        public string Password { get; set; }
    }
}
