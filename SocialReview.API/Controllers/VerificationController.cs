using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialReview.BLL.Verification.Interfaces;
using SocialReview.BLL.Verification.Models;
using System.ComponentModel.Design;

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
        [Authorize]
        public async Task<IActionResult> SendCode(SendingType sendingType)
        {
            await _verificationService.SendCodeAsync(HttpContext, sendingType);
            return Ok();
        }

        [HttpPost("verify")]
        [Authorize]
        public async Task<ActionResult> Verify(SendingType sendingType, string code)
        {
            await _verificationService.VerifyUserAsync(HttpContext, sendingType, code);
            return Ok("You are successfully verified.");
        }
    }
}