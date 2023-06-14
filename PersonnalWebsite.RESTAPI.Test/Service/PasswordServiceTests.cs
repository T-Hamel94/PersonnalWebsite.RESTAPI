using Xunit;
using FluentAssertions;
using PersonnalWebsite.RESTAPI.Service;

namespace PersonnalWebsite.RESTAPI.Test.Service
{
    public class PasswordServiceTests
    {
        private readonly PasswordService _passwordService;

        public PasswordServiceTests()
        {
            _passwordService = new PasswordService();
        }

        [Fact]
        public void CreatePasswordHash_ValidPassword_ShouldCreateDifferentHashAndSalt()
        {
            // Arrange
            string password = "TestPassword";

            // Act
            _passwordService.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            // Assert
            passwordHash.Should().NotBeNullOrEmpty();
            passwordSalt.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void VerifyPasswordHash_WithCreatedHashAndSalt_ShouldReturnTrue()
        {
            // Arrange
            string password = "TestPassword";
            _passwordService.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            // Act
            var result = _passwordService.VerifyPasswordHash(password, passwordHash, passwordSalt);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void VerifyComplexPasswordHash_WithCreatedHashAndSalt_ShouldReturnTrue()
        {
            // Arrange
            string password = "EddwDJ%^&2edEI9384u2943__-dadew!";
            _passwordService.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            // Act
            var result = _passwordService.VerifyPasswordHash(password, passwordHash, passwordSalt);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void VerifyPasswordHash_WithWrongHash_ShouldReturnFalse()
        {
            // Arrange
            string password = "TestPassword";
            _passwordService.CreatePasswordHash(password, out byte[] correctPasswordHash, out byte[] passwordSalt);

            byte[] wrongPasswordHash = new byte[64]; 

            // Act
            var result = _passwordService.VerifyPasswordHash(password, wrongPasswordHash, passwordSalt);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void VerifyPasswordHash_WithWrongSalt_ShouldReturnFalse()
        {
            // Arrange
            string password = "TestPassword";
            _passwordService.CreatePasswordHash(password, out byte[] passwordHash, out byte[] correctPasswordSalt);

            byte[] wrongPasswordSalt = new byte[64]; 

            // Act
            var result = _passwordService.VerifyPasswordHash(password, passwordHash, wrongPasswordSalt);

            // Assert
            result.Should().BeFalse();
        }
    }
}
