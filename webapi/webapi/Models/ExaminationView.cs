namespace webapi.Models
{
    public class ExaminationView : Examination
    {
        public string PatientFirstName { get; set; }  // Hasta adı
        public string PatientLastName { get; set; }   // Hasta soyadı
        public string DoctorFirstName { get; set; }
        public string DoctorLastName { get; set; }
    }
}


