using EmailSenderApp.Auth.Models;
using EmailSenderApp.Models;

namespace EmailSenderApp.Repository.Interface
{
    public interface IFhirApiRepository
    {
        Task<TokenResponse>GetFHIRToken(string code);
        Task<Visit> GetPatientVisitDetails(string patientId, string accessToken);
    }
}
