using Microsoft.AspNetCore.Mvc;
using PersonnalWebsite.RESTAPI.Interfaces;
using PersonnalWebsite.RESTAPI.Model;

namespace PersonnalWebsite.RESTAPI.Controllers
{
    // Received User from the Service class and returns UserModel
    [Route("api/Users")]
    [ApiController]
    public class UserContoller : ControllerBase
    {
        private IUserService _userService;

        public UserContoller (IUserService userService)
        {
            _userService = userService;
        }

        // api/users{guid}
        [HttpGet("userID")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<UserModel> Get(Guid userID)
        {
            UserModel userFound = _userService.GetUserByID(userID);

            if(userFound == null)
            {
                return NotFound();
            }

            return Ok(userFound);
        }

        // api/users
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<UserModel>> Get()
        {
            IEnumerable<UserModel> users = _userService.GetUsers();

            if (users == null || users.Count() < 1)
            {
                return NotFound();
            }

            return Ok(users);
        }
    }
}
