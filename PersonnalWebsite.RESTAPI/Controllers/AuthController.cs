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

        [HttpPost()]
        [Route("login")]
        public ActionResult<string> Login(UserLoginModel request)
        {
            try
            {
                string JSWToken = _authService.Login(request.Email, request.Password);
                return Ok(JSWToken);
            }
            catch (Exception ex)
            {
                return BadRequest("There was an error while logging in");
            }
        }
    }
}
