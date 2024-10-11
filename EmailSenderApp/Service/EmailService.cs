using EmailSenderApp.Models;
using EmailSenderApp.Service.Interface;

namespace EmailSenderApp.Service
{
    public class EmailService: IEmailService
    {
        private readonly IEmailClient _emailClient;

        public EmailService(IEmailClient emailClient)
        {
            _emailClient = emailClient;
        }

        public async Task<int> SendMedicationEmailAsync(Visit visit)
        {
            try
            {
                var emailContent = GenerateEmailContent(visit);
                if (visit.PatientEmail != "No email found")
                {
                    await _emailClient.SendEmailAsync(visit.PatientEmail, "Your Medication Summary", emailContent);
                    return StatusCodes.Status200OK;
                }
                else
                {
                    return StatusCodes.Status404NotFound;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string GenerateEmailContent( Visit visit)
        {
            return $@"
            Hello {visit.PatientFullName},

            Thank you for visiting our clinic. Below are the details of your recent visit.

            Practitioner: {visit.PractitionerName}
            Visit Date: {visit.VisitDate}

            Medications Prescribed:
            {string.Join("\n", visit.Medications.Select(m => $"{m.Name} - {m.Dosage}"))}

            Conditions Noted: 
            {string.Join(", ", visit.Conditions)}

            Best regards,
            Clinic Team";
        }

    }
}
