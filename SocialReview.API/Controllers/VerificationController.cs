using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialReview.BLL.Verification.Interfaces;
using SocialReview.BLL.Verification.Models;

namespace SocialReview.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VerificationController : ControllerBase
    {
        private readonly IVerificationService _verificationService;

        public VerificationController(IVerificationService verificationService) 
        {
            _verificationService = verificationService;
        }

        [HttpPost("send-code")]
        public async Task<IActionResult> SendCode(SendingType type)
        {
            await _verificationService.SendCodeAsync(type);
            return Ok();
        }

        [HttpPost("verify")]
        [Authorize]
        public async Task<ActionResult> Verify(SendingType type, string code)
        {
            await _verificationService.VerifyUserAsync(HttpContext, type, code);
            return Ok();
        }
    }
}