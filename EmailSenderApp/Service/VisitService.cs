using EmailSenderApp.Models;
using EmailSenderApp.Repository.Interface;
using EmailSenderApp.Service.Interface;

namespace EmailSenderApp.Service
{
    public class VisitService : IVisitService
    {
        private readonly IFhirApiRepository _fhirApiRepository;

        public VisitService(IFhirApiRepository fhirApiRepository)
        {
            _fhirApiRepository = fhirApiRepository;
        }

        public async Task<Visit> GetRecentVisitDetailsAsync(string patientId, string accessToken)
        {
            try
            {
                return await _fhirApiRepository.GetPatientVisitDetails(patientId, accessToken);
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
