using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonnalWebsite.RESTAPI.CustomExceptions;
using PersonnalWebsite.RESTAPI.Interfaces;
using PersonnalWebsite.RESTAPI.Model;
using System.Security.Claims;

namespace PersonnalWebsite.RESTAPI.Controllers
{
    // Received User from the Service class and returns UserModel
    [Route("api/Users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(UserController));
        private IUserService _userService;

        public UserController(IUserService userService)
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
                    Log.Warn($"User not found with user id : {userID}");
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
        public ActionResult<UserModel> GetUserByEmail(string userEmail)
        {
            try
            {
                UserModel userFound = _userService.GetUserByEmail(userEmail);

                if (userFound == null)
                {
                    Log.Warn($"User not found with user email: {userEmail}");
                    return NotFound();
                }

                return Ok(userFound);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occured while getting user with email:{userEmail}");
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
                Log.Error(ex);
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
                Log.Error(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while creating the user");
            }
        }

        // POST api/users/register
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Route("register")]
        public ActionResult<UserModel> RegisterUser(UserRegistrationModel user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            try
            {
                UserModel createdUser = _userService.RegisterUser(user);

                return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while registering the user");
            }
        }

        // PUT api/users
        [HttpPut, Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<UserModel> UpdateUser(UserModel user)
        {
            if (user == null || user.Id == Guid.Empty)
            {
                Log.Error("Bad Request: Invalid user data with" + user.ToString()); ;
                return BadRequest("Invalid user data");
            }

            Guid loggedInUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            try
            {
                UserModel updatedUser = _userService.UpdateUser(loggedInUserId, user);

                if (updatedUser == null)
                {
                    return NotFound("User not found");
                }

                return Ok(updatedUser);
            }
            catch (UnauthorizedActionException ex)
            {
                Log.Error(ex);
                return StatusCode(StatusCodes.Status403Forbidden, "Unauthorized action");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the user");
            }
        }

        // DELETE api/users/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult DeleteUser(Guid id)
        {
            if (id == Guid.Empty)
            {
                Log.Error("User ID cannot be null or empty: " + id);
                return BadRequest("Invalid user Id");
            }

            try
            {
                _userService.DeleteUser(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                if (ex.Message == "User not found")
                {
                    Log.Error("User not found with ID: " + id);
                    return NotFound(ex.Message);
                }

                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the user");
            }
        }
    }
}
