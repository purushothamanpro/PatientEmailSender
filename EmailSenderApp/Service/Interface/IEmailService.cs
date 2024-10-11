using EmailSenderApp.Models;

namespace EmailSenderApp.Service.Interface
{
    public interface IEmailService
    {
        Task<int> SendMedicationEmailAsync(Visit visit);
    }
}
