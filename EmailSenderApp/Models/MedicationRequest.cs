using EmailSenderApp.Repository;

namespace EmailSenderApp.Models
{
    public class MedicationRequest
    {
        public string Id { get; set; }
        public CodeableConcept MedicationCodeableConcept { get; set; }
        public DosageInstruction[] DosageInstruction { get; set; }
    }
    public class CodeableConcept
    {
        public string Text { get; set; }
    }
    public class DosageInstruction
    {
        public string Text { get; set; }
    }
}
