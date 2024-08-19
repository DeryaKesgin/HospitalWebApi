namespace webapi.Models
{
    public class ExaminationView : Examination
    {
        public string HastaFirstName { get; set; }  // Hasta adı
        public string HastaLastName { get; set; }   // Hasta soyadı
        public string DoctorFirstName { get; set; }
        public string DoctorLastName { get; set; }
    }
}


