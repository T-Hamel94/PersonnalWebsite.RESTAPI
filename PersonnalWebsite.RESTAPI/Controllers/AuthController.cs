using Microsoft.AspNetCore.Mvc;
using PersonnalWebsite.RESTAPI.Interfaces;
using PersonnalWebsite.RESTAPI.Model;

namespace PersonnalWebsite.RESTAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Login(UserLoginModel request)
        {
            string JSWToken = _authService.Login(request.Email, request.Password);

            if(JSWToken == null)
            {
                return BadRequest("User not found");
            }

            return Ok(JSWToken);
        }
    }
}
