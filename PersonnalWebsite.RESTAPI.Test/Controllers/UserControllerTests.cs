using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Moq;
using Xunit;
using PersonnalWebsite.RESTAPI.Controllers;
using PersonnalWebsite.RESTAPI.Interfaces;
using PersonnalWebsite.RESTAPI.Model;
using PersonnalWebsite.RESTAPI.Test.TestHelper;
using Microsoft.AspNetCore.Http;
using PersonnalWebsite.RESTAPI.CustomExceptions;

namespace PersonnalWebsite.RESTAPI.Test.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UserController _userController;
        private readonly List<UserPublicModel> _userPublicModelList;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _userController = new UserController(_mockUserService.Object);
            _userPublicModelList = UserHelper.GenerateListOf5UsersPublicModel();
        }

        #region Get Methods
        [Fact]
        public void GetUsers_ReturnsOkResult_WithUserList()
        {
            // Arrange
            _mockUserService.Setup(service => service.GetUsers()).Returns(_userPublicModelList);

            // Act
            ActionResult<IEnumerable<UserPublicModel>> actionResult = _userController.GetUsers();
            var okResult = actionResult.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            okResult.Should().BeOfType<OkObjectResult>();
            List<UserPublicModel> models = okResult.Value as List<UserPublicModel>;
            models.Should().NotBeNull();
            models.Count().Should().Be(5);
        }

        [Fact]
        public void GetUserById_ReturnsOkResult_WithUserModel_WhenUserIsFound()
        {
            // Arrange
            UserModel user = UserHelper.GenerateUser().ToModel();
            Guid userId = user.Id;

            _mockUserService.Setup(service => service.GetUserByID(userId)).Returns(user);

            // Act
            ActionResult<UserModel> actionResult = _userController.GetUserById(userId);
            var okResult = actionResult.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            okResult.Should().BeOfType<OkObjectResult>();
            var returnedUser = okResult.Value as UserModel;
            returnedUser.Should().BeEquivalentTo(user);
        }


        [Fact]
        public void GetUserByEmail_ReturnsOkResult_WithUserModel_WhenUserIsFound()
        {
            // Arrange
            UserModel user = UserHelper.GenerateUser().ToModel();
            string userEmail = user.Email;

            _mockUserService.Setup(service => service.GetUserByEmail(userEmail)).Returns(user);

            // Act
            ActionResult<UserModel> actionResult = _userController.GetUserByEmail(userEmail);
            OkObjectResult okResult = actionResult.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            okResult.Should().BeOfType<OkObjectResult>();
            var returnedUser = okResult.Value as UserModel;
            returnedUser.Should().BeEquivalentTo(user);
        }
        #endregion

        #region CreateUser
        [Fact]
        public void CreateUser_ReturnsCreatedResult_WithNewUser()
        {
            // Arrange
            UserModel newUser = UserHelper.GenerateUser().ToModel();
            Guid userId = newUser.Id;

            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = UserHelper.GenerateClaimsPrincipal(userId) }
            };
            _mockUserService.Setup(service => service.CreateUser(userId, newUser)).Returns(newUser);

            // Act
            ActionResult<UserModel> actionResult = _userController.CreateUser(newUser);
            var createdResult = actionResult.Result as CreatedResult;

            // Assert
            createdResult.Should().NotBeNull();
            createdResult.Should().BeOfType<CreatedResult>();
            var returnedUser = createdResult.Value as UserModel;
            returnedUser.Should().BeEquivalentTo(newUser);
        }
        [Fact]
        public void CreateUser_ReturnsBadRequest_WhenNewUserIsNull()
        {
            // Arrange
            UserModel newUser = null;

            // Act
            ActionResult<UserModel> actionResult = _userController.CreateUser(newUser);
            var badRequestResult = actionResult.Result as BadRequestResult;

            // Assert
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public void CreateUser_ReturnsForbidResult_WhenUserIsNotAuthorizedToCreateUser()
        {
            // Arrange
            UserModel newUser = UserHelper.GenerateUser().ToModel();
            Guid userId = newUser.Id;

            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = UserHelper.GenerateClaimsPrincipal(userId) }
            };
            _mockUserService.Setup(service => service.CreateUser(userId, newUser)).Throws<UnauthorizedActionException>();

            // Act
            ActionResult<UserModel> actionResult = _userController.CreateUser(newUser);
            var forbiddenObjectResult = actionResult.Result as ObjectResult;

            // Assert
            forbiddenObjectResult.Should().NotBeNull();
            forbiddenObjectResult.Value.Should().Be("Logged in user is not authorized to create another user");
            forbiddenObjectResult.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        public void CreateUser_ReturnsStatusCode500_WhenUnexpectedErrorOccurs()
        {
            // Arrange
            UserModel newUser = UserHelper.GenerateUser().ToModel();
            Guid userId = newUser.Id;

            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = UserHelper.GenerateClaimsPrincipal(userId) }
            };
            _mockUserService.Setup(service => service.CreateUser(userId, newUser)).Throws<Exception>();

            // Act
            ActionResult<UserModel> actionResult = _userController.CreateUser(newUser);
            var statusCodeResult = actionResult.Result as ObjectResult;

            // Assert
            statusCodeResult.Should().NotBeNull();
            statusCodeResult.Value.Should().Be("An error occured while creating the user");
            statusCodeResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
        #endregion

        #region RegisterUser
        [Fact]
        public void RegisterUser_ReturnsCreatedResult_WithUserModel_WhenRegistrationIsSuccessful()
        {
            // Arrange
            UserModel newUser = UserHelper.GenerateUser().ToModel();
            UserRegistrationModel newUserRegistration = newUser.ToEntity().ToRegistrationModel();

            _mockUserService.Setup(service => service.RegisterUser(newUserRegistration)).Returns(newUser);

            // Act
            ActionResult<UserModel> actionResult = _userController.RegisterUser(newUserRegistration);
            var createdResult = actionResult.Result as CreatedResult;

            // Assert
            createdResult.Should().NotBeNull();
            createdResult.Should().BeOfType<CreatedResult>();
            var registeredUser = createdResult.Value as UserModel;
            registeredUser.Should().BeEquivalentTo(newUser);
        }

        [Fact]
        public void RegisterUser_ReturnsBadRequest_WhenUserIsNull()
        {
            // Arrange
            UserRegistrationModel newUserRegistration = null;

            // Act
            ActionResult<UserModel> actionResult = _userController.RegisterUser(newUserRegistration);
            var badRequestResult = actionResult.Result as BadRequestResult;

            // Assert
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public void RegisterUser_ReturnsStatusCode500_WhenUnexpectedErrorOccurs()
        {
            // Arrange
            UserModel newUser = UserHelper.GenerateUser().ToModel();
            UserRegistrationModel newUserRegistration = newUser.ToEntity().ToRegistrationModel();

            _mockUserService.Setup(service => service.RegisterUser(newUserRegistration)).Throws(new Exception());

            // Act
            ActionResult<UserModel> actionResult = _userController.RegisterUser(newUserRegistration);
            var statusCodeResult = actionResult.Result as ObjectResult;

            // Assert
            statusCodeResult.Should().NotBeNull();
            statusCodeResult.Value.Should().Be("An error occured while registering the user");
            statusCodeResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
        #endregion

        #region updateUser
        [Fact]
        public void UpdateUser_ReturnsOkResult_WithUpdatedUser()
        {
            // Arrange
            UserModel userToUpdate = UserHelper.GenerateUser().ToModel();
            Guid userId = userToUpdate.Id;

            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = UserHelper.GenerateClaimsPrincipal(userId) }
            };

            _mockUserService.Setup(service => service.UpdateUser(userId, userToUpdate)).Returns(userToUpdate);

            // Act
            ActionResult<UserModel> actionResult = _userController.UpdateUser(userToUpdate);
            var okResult = actionResult.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            okResult.Should().BeOfType<OkObjectResult>();
            var returnedUser = okResult.Value as UserModel;
            returnedUser.Should().BeEquivalentTo(userToUpdate);
        }

        [Fact]
        public void UpdateUser_ReturnsBadRequest_WhenUserIsNull()
        {
            // Arrange
            UserModel userToUpdate = null;

            // Act
            ActionResult<UserModel> actionResult = _userController.UpdateUser(userToUpdate);
            var badRequestObjectResult = actionResult.Result as BadRequestObjectResult;

            // Assert
            badRequestObjectResult.Should().NotBeNull();
            badRequestObjectResult.Value.Should().Be("Invalid user data");
            badRequestObjectResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public void UpdateUser_ReturnsNotFoundResult_WhenUserToBeUpdatedDoesNotExist()
        {
            // Arrange
            UserModel userToUpdate = UserHelper.GenerateUser().ToModel();
            Guid userId = userToUpdate.Id;

            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = UserHelper.GenerateClaimsPrincipal(userId) }
            };

            _mockUserService.Setup(service => service.UpdateUser(userId, userToUpdate)).Throws<UserNotFoundException>();

            // Act
            ActionResult<UserModel> actionResult = _userController.UpdateUser(userToUpdate);
            var notFoundObjectResult = actionResult.Result as NotFoundObjectResult;

            // Assert
            notFoundObjectResult.Should().NotBeNull();
            notFoundObjectResult.Value.Should().Be("User to update could not be found");
            notFoundObjectResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public void UpdateUser_ReturnsStatusCode403_WhenLoggedInUserIsNotAuthorizedToUpdateUser()
        {
            // Arrange
            UserModel userToUpdate = UserHelper.GenerateUser().ToModel();
            Guid userId = userToUpdate.Id;

            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = UserHelper.GenerateClaimsPrincipal(userId) }
            };

            _mockUserService.Setup(service => service.UpdateUser(userId, userToUpdate)).Throws(new UnauthorizedActionException());

            // Act
            ActionResult<UserModel> actionResult = _userController.UpdateUser(userToUpdate);
            var forbiddenObjectResult = actionResult.Result as ObjectResult;

            // Assert
            forbiddenObjectResult.Should().NotBeNull();
            forbiddenObjectResult.Value.Should().Be("Current logged in user does not have the authorization this user");
            forbiddenObjectResult.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        public void UpdateUser_ReturnsStatusCode500_WhenUnexpectedErrorOccurs()
        {
            // Arrange
            UserModel userToUpdate = UserHelper.GenerateUser().ToModel();
            Guid userId = userToUpdate.Id;

            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = UserHelper.GenerateClaimsPrincipal(userId) }
            };

            _mockUserService.Setup(service => service.UpdateUser(userId, userToUpdate)).Throws(new Exception());

            // Act
            ActionResult<UserModel> actionResult = _userController.UpdateUser(userToUpdate);
            var statusCodeResult = actionResult.Result as ObjectResult;

            // Assert
            statusCodeResult.Should().NotBeNull();
            statusCodeResult.Value.Should().Be("An error occurred while updating the user");
            statusCodeResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
        #endregion

        #region deleteUser
        [Fact]
        public void DeleteUser_ReturnsBadRequest_WhenUserIdIsEmpty()
        {
            // Arrange
            Guid userToDeleteId = Guid.Empty;
            Guid loggedInUserId = Guid.NewGuid();

            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = UserHelper.GenerateClaimsPrincipal(loggedInUserId) }
            };

            // Act
            var actionResult = _userController.DeleteUser(userToDeleteId);

            // Assert
            var badRequestObjectResult = actionResult as BadRequestObjectResult;
            badRequestObjectResult.Should().NotBeNull();
            badRequestObjectResult.Value.Should().Be("Invalid user Id");
            badRequestObjectResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public void DeleteUser_ReturnsNoContentResult_WhenUserIsSuccessfullyDeleted()
        {
            // Arrange
            Guid userToDeleteId = Guid.NewGuid();
            Guid loggedInUserId = Guid.NewGuid();

            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = UserHelper.GenerateClaimsPrincipal(loggedInUserId) }
            };

            _mockUserService.Setup(service => service.DeleteUser(loggedInUserId, userToDeleteId)).Verifiable();

            // Act
            var actionResult = _userController.DeleteUser(userToDeleteId);

            // Assert
            var noContentResult = actionResult as NoContentResult;
            noContentResult.Should().NotBeNull();
            _mockUserService.Verify();
        }

        [Fact]
        public void DeleteUser_ReturnsNotFoundResult_WhenUserToBeDeletedDoesNotExist()
        {
            // Arrange
            Guid userToDeleteId = Guid.NewGuid();
            Guid loggedInUserId = Guid.NewGuid();

            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = UserHelper.GenerateClaimsPrincipal(loggedInUserId) }
            };

            _mockUserService.Setup(service => service.DeleteUser(loggedInUserId, userToDeleteId)).Throws<UserNotFoundException>();

            // Act
            var actionResult = _userController.DeleteUser(userToDeleteId);

            // Assert
            var notFoundObjectResult = actionResult as NotFoundObjectResult;
            notFoundObjectResult.Should().NotBeNull();
            notFoundObjectResult.Value.Should().Be("User to delete could not be found");
            notFoundObjectResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }


        [Fact]
        public void DeleteUser_ReturnsForbidResult_WhenLoggedInUserIsNotAuthorizedToDeleteUser()
        {
            // Arrange
            Guid userToDeleteId = Guid.NewGuid();
            Guid loggedInUserId = Guid.NewGuid();

            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = UserHelper.GenerateClaimsPrincipal(loggedInUserId) }
            };

            _mockUserService.Setup(service => service.DeleteUser(loggedInUserId, userToDeleteId)).Throws(new UnauthorizedActionException());

            // Act
            IActionResult actionResult = _userController.DeleteUser(userToDeleteId);
            var forbiddenObjectResult = actionResult as ObjectResult;

            // Assert
            forbiddenObjectResult.Should().NotBeNull();
            forbiddenObjectResult.Value.Should().Be("Current logged in user does not have the authorization to delete this user");
            forbiddenObjectResult.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        public void DeleteUser_ReturnsStatusCode500_WhenUnexpectedErrorOccurs()
        {
            // Arrange
            Guid userToDeleteId = Guid.NewGuid();
            Guid loggedInUserId = Guid.NewGuid();

            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = UserHelper.GenerateClaimsPrincipal(loggedInUserId) }
            };

            _mockUserService.Setup(service => service.DeleteUser(loggedInUserId, userToDeleteId)).Throws(new Exception());

            // Act
            var actionResult = _userController.DeleteUser(userToDeleteId);

            // Assert
            var objectResult = actionResult as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            objectResult.Value.Should().Be("An error occurred while deleting the user");
        }
        #endregion

    }
}
