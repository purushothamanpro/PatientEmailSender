using EmailSenderApp.Models;

namespace EmailSenderApp.Service.Interface
{
    public interface IVisitService
    {
        Task<Visit> GetRecentVisitDetailsAsync(string patientId, string accessToken);
    }
}
