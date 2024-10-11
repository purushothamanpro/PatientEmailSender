using EmailSenderApp.Repository;
using System.Net;

namespace EmailSenderApp.Models
{
    public class Patient
    {
        public string Id { get; set; }
        public HumanName[] Name { get; set; }
        public Telecom[] Telecom { get; set; }
    }
    public class Telecom
    {
        public string System { get; set; }  
        public string Value { get; set; }  
    }
}
