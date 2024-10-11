namespace EmailSenderApp.Models
{
    public class Visit
    {
        public string PatientFullName { get; set; }
        public DateTime VisitDate { get; set; }
        public string PractitionerName { get; set; }
        public List<Medication> Medications { get; set; }
        public List<string> Conditions { get; set; }
        public string PatientEmail { get; set; }
    }
}
