using Microsoft.AspNetCore.Mvc;
using PersonnalWebsite.RESTAPI.Model;
using Srv_PersonnalWebsite;
using Srv_PersonnalWebsite.Entity;

namespace PersonnalWebsite.RESTAPI.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UserContoller : ControllerBase
    {
        private IUserRepo UserRepo;

        public UserContoller (IUserRepo userRepo)
        {
            UserRepo = userRepo;
        }

        // api/users{guid}
        [HttpGet("userID")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<UserModel> Get(Guid id)
        {
            User userFound = UserRepo.GetUserByID(id);

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
            IEnumerable<User> users = UserRepo.GetUsers();

            if (users == null || users.Count() < 1)
            {
                return NotFound();
            }

            return Ok(users);
        }

    }
}
