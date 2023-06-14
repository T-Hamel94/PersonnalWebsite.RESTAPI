using log4net;
using Microsoft.AspNetCore.Authorization;
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

        // GET api/users
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<UserPublicModel>> GetUsers()
        {
            try
            {
                IEnumerable<UserPublicModel> users = _userService.GetUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                Log.Error("GetUsers: " + ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while getting the users");
            }
        }

        // GET api/users/{guid}
        [HttpGet("userID/{userID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<UserModel> GetUserById(Guid userID)
        {
            try
            {
                UserModel userFound = _userService.GetUserByID(userID);
                return Ok(userFound);
            }
            catch (UserNotFoundException ex)
            {
                Log.Warn("GetUserByEmail: " + ex);
                return NotFound($"Could not find user with ID: {userID}");
            }
            catch (Exception ex)
            {
                Log.Error("GetUserByEmail: " + ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occured while getting user with ID:{userID}");
            }
        }

        // GET api/users{email}
        [HttpGet("email/{email}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<UserModel> GetUserByEmail(string email)
        {
            try
            {
                UserModel userFound = _userService.GetUserByEmail(email);
                return Ok(userFound);
            }
            catch (UserNotFoundException ex)
            {
                Log.Warn("GetUserByEmail: " + ex);
                return NotFound($"Could not find user with email: {email}");
            }
            catch (Exception ex)
            {
                Log.Error("GetUserByEmail: " + ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occured while getting user with email:{email}");
            }
        }

        // POST api/users
        [HttpPost, Authorize]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public ActionResult<UserModel> CreateUser(UserModel newUser)
        {
            if(newUser == null)
            {
                Log.Warn($"User cannot be null: {newUser}");
                return BadRequest();
            }

            Guid loggedInUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            try
            {
                UserModel createdUser = _userService.CreateUser(loggedInUserId, newUser);
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
            }
            catch (UnauthorizedActionException ex)
            {
                Log.Warn($"CreateUser: {ex}");
                return StatusCode(StatusCodes.Status403Forbidden, "Logged in user is not authorized to create another user");
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
        public ActionResult<UserModel> RegisterUser(UserRegistrationModel newUser)
        {
            if (newUser == null)
            {
                Log.Warn($"User cannot be null: {newUser}");
                return BadRequest();
            }

            try
            {
                UserModel createdUser = _userService.RegisterUser(newUser);
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                Log.Error("Register: " + ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while registering the user");
            }
        }

        // PUT api/users
        [HttpPut, Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public ActionResult<UserModel> UpdateUser(UserModel userToUpdate)
        {
            if (userToUpdate == null || userToUpdate.Id == Guid.Empty)
            {
                Log.Warn($"User argument cannot be null and it's ID cannot be empty: {userToUpdate}");
                return BadRequest("Invalid user data");
            }

            Guid loggedInUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            try
            {
                UserModel updatedUser = _userService.UpdateUser(loggedInUserId, userToUpdate);
                return Ok(updatedUser);
            }
            catch (UserNotFoundException ex)
            {
                Log.Info("UpdateUser: " + ex);
                return NotFound("User to update could not be found");
            }
            catch (UnauthorizedActionException ex)
            {
                Log.Warn("UpdateUsers: " + ex);
                return StatusCode(StatusCodes.Status403Forbidden, "Current logged in user does not have the authorization this user");
            }
            catch (Exception ex)
            {
                Log.Error("UpdateUsers: " + ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the user");
            }
        }

        // DELETE api/users/{id}
        [HttpDelete("{id}"), Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public ActionResult DeleteUser(Guid userID)
        {
            if (userID == Guid.Empty)
            {
                Log.Warn($"User ID cannot be null or empty: {userID}");
                return BadRequest("Invalid user Id");
            }

            Guid loggedInUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            try
            {
                _userService.DeleteUser(loggedInUserId, userID);
                return NoContent();
            }
            catch (UserNotFoundException ex)
            {
                Log.Info("DeleteUser: " + ex);
                return NotFound("User to delete could not be found");
            }
            catch (UnauthorizedActionException ex)
            {
                Log.Warn("DeleteUser: " + ex);
                return StatusCode(StatusCodes.Status403Forbidden, "Current logged in user does not have the authorization to delete this user");
            }
            catch (Exception ex)
            {
                Log.Error("DeleteUser: " + ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the user");
            }
        }
    }
}
