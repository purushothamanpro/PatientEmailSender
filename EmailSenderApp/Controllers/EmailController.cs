using EmailSenderApp.Models;
using EmailSenderApp.Service.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmailSenderApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IVisitService _visitService;
        private readonly IEmailService _emailService;
        public EmailController(IVisitService visitService,IEmailService emailService) { 
            _visitService = visitService;
            _emailService = emailService;
        }
        // POST api/<EmailController>
        [HttpPost("{patientId}/complete-visit")]
        public async Task<IActionResult> CompleteVisit(string patientId)
        {
            try
            {
                var accessToken = HttpContext.Session.GetString("AccessToken");

                if (string.IsNullOrEmpty(accessToken))
                {
                    return Unauthorized("Access token is missing");
                }
                var recentPatientVisit = await _visitService.GetRecentVisitDetailsAsync(patientId, accessToken);
                var statusCode = await _emailService.SendMedicationEmailAsync(recentPatientVisit);
                return StatusCode(statusCode);
            }
            catch (HttpRequestException httpEx)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "External service error. Please try again later.");
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }
    }
}
