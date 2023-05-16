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

        public UserContoller(IUserService userService)
        {
            _userService = userService;
        }

        // GET api/users{guid}
        [HttpGet("userID")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<UserModel> GetUser(Guid userID)
        {
            try
            {
                UserModel userFound = _userService.GetUserByID(userID);

                if (userFound == null)
                {
                    return NotFound();
                }

                return Ok(userFound);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occured while getting user with ID:{userID}");
            }
        }

        // GET api/users{email}
        [HttpGet("email")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<UserModel> GetUserByEmail(string email)
        {
            try
            {
                UserModel userFound = _userService.GetUserByEmail(email);

                if (userFound == null)
                {
                    return NotFound();
                }

                return Ok(userFound);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occured while getting user with email:{email}");
            }
        }

        // GET api/users
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<UserModel>> GetUsers()
        {
            try
            {
                IEnumerable<UserModel> users = _userService.GetUsers();

                if (users == null || users.Count() < 1)
                {
                    return NotFound();
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while getting the users");
            }
        }

        // POST api/users
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public ActionResult<UserModel> CreateUser(UserModel user)
        {
            if(user == null)
            {
                return BadRequest();
            }

            try
            {
                UserModel createdUser = _userService.CreateUser(user);

                return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while creating the user");
            }
        }

        // PUT api/users
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<UserModel> UpdateUser(UserModel user)
        {
            if (user == null || user.Id == Guid.Empty)
            {
                return BadRequest("Invalid user data");
            }

            try
            {
                UserModel updatedUser = _userService.UpdateUser(user);

                if (updatedUser == null)
                {
                    return NotFound("User not found");
                }

                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the user");
            }
        }
    }
}
