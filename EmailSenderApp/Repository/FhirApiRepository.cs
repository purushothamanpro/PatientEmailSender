using EmailSenderApp.Auth.Models;
using EmailSenderApp.Models;
using EmailSenderApp.Repository.Interface;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net.Http;



namespace EmailSenderApp.Repository
{
    public class FhirApiRepository : IFhirApiRepository
    {
        private readonly OAuthSettings _oauthSettings;

        private static readonly string FHIRBaseUrl = "https://[nextgen-api-base-url]";

        private static readonly HttpClient httpClient = new HttpClient();
        public FhirApiRepository( IOptions<OAuthSettings> oauthSettings)
        {
            _oauthSettings = oauthSettings.Value;
        }

        public async Task<TokenResponse> GetFHIRToken(string code)
        {
            try
            {
                using (httpClient)
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, _oauthSettings.TokenEndpoint);

                    var formData = new Dictionary<string, string>
                {
                { "client_id", _oauthSettings.ClientId },
                { "client_secret", _oauthSettings.ClientSecret },
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", _oauthSettings.RedirectUri }
                 };

                    request.Content = new FormUrlEncodedContent(formData);

                    var response = await httpClient.SendAsync(request);
                    var content = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<TokenResponse>(content);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
      
        public async Task<Visit> GetPatientVisitDetails(string patientId, string accessToken)
        {

            try
            {
                // Set the authorization header with the bearer token
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var lastVisitDetails = new Visit();


                // Step 1: Get the last Encounter details (visit details)
                var encounters = await GetEncounters(patientId);
                var lastEncounter = encounters.OrderByDescending(e => e.Period.Start).FirstOrDefault();

                if (lastEncounter != null)
                {
                    lastVisitDetails.VisitDate = lastEncounter.Period.Start;

                    // Step 2: Get Patient details
                    var patient = await GetPatient(patientId);

                    lastVisitDetails.PatientFullName = $"{patient.Name[0].Given[0]} {patient.Name[0].Family}";
                    lastVisitDetails.PatientEmail = patient.Telecom?.FirstOrDefault(t => t.System == "email")?.Value ?? "No email found";

                    // Step 3: Get Practitioner details for the encounter
                    if (lastEncounter.Participant != null)
                    {
                        var practitionerId = lastEncounter.Participant[0].Individual.Reference.Split("/")[1];
                        var practitioner = await GetPractitioner(practitionerId);
                        lastVisitDetails.PatientFullName = $"{practitioner.Name[0].Given[0]} {practitioner.Name[0].Family}";
                    }

                    // Step 4: Get MedicationRequest for the last encounter
                    var medications = await GetMedications(patientId, lastEncounter.Id);
                    List<Medication> medicationsDetails = new List<Medication>();
                    foreach (var med in medications)
                    {
                        medicationsDetails.Add(new Medication
                        {
                            Dosage = med.DosageInstruction[0].Text,
                            Name = med.MedicationCodeableConcept.Text
                        });
                    }
                    lastVisitDetails.Medications = medicationsDetails;

                    // Step 5: Get Condition (diagnoses) for the last encounter
                    var conditions = await GetConditions(patientId, lastEncounter.Id);

                    List<string> conditionDetails = new List<string>();
                    foreach (var condition in conditions)
                    {
                        conditionDetails.Add(condition.Code.Text);
                    }
                    lastVisitDetails.Conditions = conditionDetails;
                }
                return lastVisitDetails;
            }
            catch (Exception)
            {

                throw;
            }
        }
        private async Task<Patient> GetPatient(string patientId)
        {
            try
            {
                using (httpClient)
                {
                    var response = await httpClient.GetStringAsync($"{FHIRBaseUrl}/Patient/{patientId}");
                    return JsonConvert.DeserializeObject<Patient>(response);
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private async Task<List<Encounter>> GetEncounters(string patientId)
        {
            try
            {
                using (httpClient)
                {
                    var response = await httpClient.GetStringAsync($"{FHIRBaseUrl}/Encounter?patient={patientId}");
                    return JsonConvert.DeserializeObject<FHIRSearchResult<Encounter>>(response).Entry;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<Practitioner> GetPractitioner(string practitionerId)
        {
            try
            {
                using (httpClient)
                {
                    var response = await httpClient.GetStringAsync($"{FHIRBaseUrl}/Practitioner/{practitionerId}");
                    return JsonConvert.DeserializeObject<Practitioner>(response);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<List<MedicationRequest>> GetMedications(string patientId, string encounterId)
        {
            try
            {
                using (httpClient)
                {
                    var response = await httpClient.GetStringAsync($"{FHIRBaseUrl}/MedicationRequest?patient={patientId}&encounter={encounterId}");
                    return JsonConvert.DeserializeObject<FHIRSearchResult<MedicationRequest>>(response).Entry;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<List<Condition>> GetConditions(string patientId, string encounterId)
        {
            try
            {
                using (httpClient)
                {
                    var response = await httpClient.GetStringAsync($"{FHIRBaseUrl}/Condition?patient={patientId}&encounter={encounterId}");
                    return JsonConvert.DeserializeObject<FHIRSearchResult<Condition>>(response).Entry;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
    public class FHIRSearchResult<T>
    {
        public List<T> Entry { get; set; }
    }
}

