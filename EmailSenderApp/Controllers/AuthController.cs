using EmailSenderApp.Auth.Models;
using EmailSenderApp.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EmailSenderApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IFhirApiRepository _fhirApiRepository;
        public AuthController(IFhirApiRepository fhirApiRepository)
        {
            _fhirApiRepository = fhirApiRepository;
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> Callback(string code)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    return BadRequest("Authorization code is missing");
                }

                var tokenResponse = await ExchangeCodeForToken(code);

                HttpContext.Session.SetString("FHIRAccessToken", tokenResponse.AccessToken);

                return Ok("User authenticated and access token obtained.");
            }
            catch(HttpRequestException httpEx)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "External service error. Please try again later.");
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }

        private async Task<TokenResponse> ExchangeCodeForToken(string code)
        {
            try
            {
                return await _fhirApiRepository.GetFHIRToken(code);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
