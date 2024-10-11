using EmailSenderApp.Repository;

namespace EmailSenderApp.Models
{
    public class Encounter
    {
        public string Id { get; set; }
        public Period Period { get; set; }
        public Participant[] Participant { get; set; }
    }

    public class Participant
    {
        public IndividualReference Individual { get; set; }
    }
    public class Period
    {
        public DateTime Start { get; set; }
    }
    public class IndividualReference
    {
        public string Reference { get; set; }
    }
}
