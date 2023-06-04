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
                return Ok(userFound);
            }
            catch (UserNotFoundException ex)
            {
                Log.Error("User/GetUserByEmail: " + ex);
                return StatusCode(StatusCodes.Status404NotFound, "User not found");
            }
            catch (Exception ex)
            {
                Log.Error("User/GetUserByEmail: " + ex);
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
                return Ok(userFound);
            }
            catch (UserNotFoundException ex)
            {
                Log.Error("User/GetUserByEmail: " + ex);
                return StatusCode(StatusCodes.Status404NotFound, "User not found");
            }
            catch (Exception ex)
            {
                Log.Error("User/GetUserByEmail: " + ex);
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
                    Log.Warn("Could not find any users...");
                    return NotFound();
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                Log.Error("User/GetUsers: " + ex);
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
                Log.Warn($"User cannot be null: {user}");
                return BadRequest();
            }

            try
            {
                UserModel createdUser = _userService.CreateUser(user);

                return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                Log.Error("User/CreateUser: " + ex);
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
                Log.Warn($"User cannot be null: {user}");
                return BadRequest();
            }

            try
            {
                UserModel createdUser = _userService.RegisterUser(user);

                return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                Log.Error("User/Register: " + ex);
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
                Log.Warn($"User argument cannot be null and it's ID cannot be empty: {user}");
                return BadRequest("Invalid user data");
            }

            Guid loggedInUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            try
            {
                UserModel updatedUser = _userService.UpdateUser(loggedInUserId, user);

                if (updatedUser == null)
                {
                    Log.Warn($"Could not find the user to update with the object: {user}");
                    return NotFound("User not found");
                }

                return Ok(updatedUser);
            }
            catch (UnauthorizedActionException ex)
            {
                Log.Error("User/UpdateUsers: " + ex);
                return StatusCode(StatusCodes.Status403Forbidden, "Unauthorized action");
            }
            catch (Exception ex)
            {
                Log.Error("User/UpdateUsers: " + ex);
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
                Log.Warn($"User ID cannot be null or empty: {id}");
                return BadRequest("Invalid user Id");
            }

            try
            {
                _userService.DeleteUser(id);
                return NoContent();
            }
            catch (UserNotFoundException ex)
            {
                Log.Error("User/DeleteUser: " + ex);
                return NotFound("User to delete could not be found");
            }
            catch (Exception ex)
            {
                Log.Error("User/DeleteUser: " + ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the user");
            }
        }
    }
}
