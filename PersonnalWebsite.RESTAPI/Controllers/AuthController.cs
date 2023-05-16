using Microsoft.AspNetCore.Mvc;
using PersonnalWebsite.RESTAPI.Entities;
using PersonnalWebsite.RESTAPI.Interfaces;
using PersonnalWebsite.RESTAPI.Model;
using PersonnalWebsite.RESTAPI.Service;

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

        //[HttpPost]
        //public async Task<ActionResult<string>> Login(UserModel request)
        //{
        //    //string JSWToken = _authService.Login(request);

        //    //if(JSWToken == null)
        //    //{
        //    //    return BadRequest("User not found");
        //    //}

        //    //return Ok(JSWToken);
        //}
    }
}
