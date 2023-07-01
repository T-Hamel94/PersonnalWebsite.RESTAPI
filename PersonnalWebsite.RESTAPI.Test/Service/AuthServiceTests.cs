using FluentAssertions;
using Moq;
using Xunit;
using PersonnalWebsite.RESTAPI.CustomExceptions;
using PersonnalWebsite.RESTAPI.Entities;
using PersonnalWebsite.RESTAPI.Interfaces;
using PersonnalWebsite.RESTAPI.Service;
using Microsoft.Extensions.Configuration;
using PersonnalWebsite.RESTAPI.Test.TestHelper;

namespace PersonnalWebsite.RESTAPI.Test.Service
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepo> _userRepoMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<IPasswordService> _passwordServiceMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userRepoMock = new Mock<IUserRepo>();
            _configMock = new Mock<IConfiguration>();
            _passwordServiceMock = new Mock<IPasswordService>();
            _authService = new AuthService(_userRepoMock.Object, _configMock.Object, _passwordServiceMock.Object);
        }

        [Fact]
        public void Login_UserNotFound_ThrowsUserNotFoundException()
        {
            // Arrange
            string testEmail = "test@example.com";
            _userRepoMock.Setup(x => x.GetUserByEmail(testEmail)).Returns((User)null);

            // Act
            Action act = () => _authService.Login(testEmail, "test_password");

            // Assert
            act.Should().Throw<UserNotFoundException>();
        }

        [Fact]
        public void Login_InvalidPassword_ThrowsPasswordErrorException()
        {
            // Arrange
            string testEmail = "test@example.com";
            string testPassword = "test_password";
            User testUser = new User { Email = testEmail };
            _userRepoMock.Setup(x => x.GetUserByEmail(testEmail)).Returns(testUser);
            _passwordServiceMock.Setup(x => x.VerifyPasswordHash(testPassword, It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(false);

            // Act
            Action act = () => _authService.Login(testEmail, testPassword);

            // Assert
            act.Should().Throw<PasswordErrorException>();
        }

        [Fact]
        public void Login_ValidCredentials_ReturnsJWT()
        {
            // Arrange
            User testUser = UserHelper.GenerateUser();
            string email = testUser.Email;
            string password = "soleil1234";

            _userRepoMock.Setup(x => x.GetUserByEmail(testUser.Email)).Returns(testUser);
            _passwordServiceMock.Setup(x => x.VerifyPasswordHash(password, It.IsAny <byte[]>(), It.IsAny<byte[]>())).Returns(true);
            _configMock.Setup(x => x.GetSection("AppSettings:TokenKey").Value).Returns("my_sweet_test_key_2000");

            // Act
            string token = _authService.Login(email, password);

            // Assert
            token.Should().NotBeNullOrEmpty();
        }
    }
}
