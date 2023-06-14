using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using PersonnalWebsite.RESTAPI.Controllers;
using PersonnalWebsite.RESTAPI.Interfaces;
using PersonnalWebsite.RESTAPI.Model;

namespace PersonnalWebsite.RESTAPI.Test.Controllers
{
    public class AuthControllerTests
    {
        private Mock<IAuthService> _mockAuthService;
        private AuthController _authController;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _authController = new AuthController(_mockAuthService.Object);
        }

        [Fact]
        public void Login_NullRequest_ReturnBadRequest()
        {
            // Arrange
            UserLoginModel nullLoginModel = null;

            // Act
            var loginResult = _authController.Login(nullLoginModel);
            var badRequestResult = loginResult.Result as BadRequestObjectResult;

            // Assert
            badRequestResult.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult.Value.Should().Be("There was a problem with the login request");
        }

        [Fact]
        public void Login_SuccessfulLogin_ReturnsOkWithToken()
        {
            // Arrange
            string expectedToken = "test_token";
            UserLoginModel loginModel = new UserLoginModel { Email = "test@example.com", Password = "test_password" };

            _mockAuthService.Setup(m => m.Login(loginModel.Email, loginModel.Password)).Returns(expectedToken);

            // Act
            var loginResult = _authController.Login(loginModel);
            var sucessfullResult = loginResult.Result as OkObjectResult;

            // Assert
            sucessfullResult.Should().BeOfType<OkObjectResult>();
            sucessfullResult.Value.Should().Be(expectedToken);
        }

        [Fact]
        public void Login_LoginThrowsException_ReturnsBadRequest()
        {
            // Arrange
            UserLoginModel loginModel = new UserLoginModel { Email = "test@example.com", Password = "test_password" };

            _mockAuthService.Setup(m => m.Login(loginModel.Email, loginModel.Password)).Throws(new Exception());

            // Act
            var loginResult = _authController.Login(loginModel);
            var badRequestResult = loginResult.Result as BadRequestObjectResult;

            // Assert
            badRequestResult.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult.Value.Should().Be("There was an error while logging in");
        }
    }
}
