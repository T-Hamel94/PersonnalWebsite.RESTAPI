using FluentAssertions;
using Moq;
using Xunit;
using PersonnalWebsite.RESTAPI.Entities;
using PersonnalWebsite.RESTAPI.Interfaces;
using PersonnalWebsite.RESTAPI.Service;
using PersonnalWebsite.RESTAPI.Model;
using PersonnalWebsite.RESTAPI.Test.TestHelper;
using PersonnalWebsite.RESTAPI.CustomExceptions;

namespace PersonnalWebsite.RESTAPI.Test.Service
{
    public class UserServiceTests
    {
        #region init
        private readonly Mock<IUserRepo> _mockUserRepo;
        private readonly Mock<IPasswordService> _mockPasswordService;
        private readonly UserService _userService;
        private readonly List<User> _users;

        public UserServiceTests()
        {
            _mockUserRepo = new Mock<IUserRepo>();
            _mockPasswordService = new Mock<IPasswordService>();
            _userService = new UserService(_mockUserRepo.Object, _mockPasswordService.Object);
            _users = UserHelper.GenerateListOf5Users();
        }
        #endregion init

        #region GetUsers
        [Fact]
        public void GetUsers_ReturnsUserPublicModelList_OrderedByUsername()
        {
            // Arrange
            _mockUserRepo.Setup(repo => repo.GetUsers()).Returns(_users);

            // Act
            IEnumerable<UserPublicModel> result = _userService.GetUsers();

            // Assert
            result.Should().BeAssignableTo<IEnumerable<UserPublicModel>>();
            result.Should().NotBeNull();
            result.Should().HaveCount(5);
            result.Select(r => r.Username.ToLower()).Should().BeInAscendingOrder();
        }

        [Fact]
        public void GetUsers_ReceivedEmptyUserList_ReturnsEmptyList()
        {
            // Arrange
            _mockUserRepo.Setup(repo => repo.GetUsers()).Returns(new List<User>());

            // Act
            IEnumerable<UserPublicModel> result = _userService.GetUsers();

            // Assert
            result.Should().BeAssignableTo<IEnumerable<UserPublicModel>>();
            result.Should().NotBeNull();
            result.Should().HaveCount(0);
        }
        #endregion

        #region GetUserByID
        [Fact]
        public void GetUserByID_ValidID_ReturnsUser()
        {
            // Arrange
            User expectedUser = UserHelper.GenerateUser();

            _mockUserRepo.Setup(repo => repo.GetUserByID(expectedUser.Id)).Returns(expectedUser);

            // Act
            UserModel userReceived = _userService.GetUserByID(expectedUser.Id);

            // Assert
            userReceived.Should().NotBeNull();
            userReceived.Should().BeOfType<UserModel>();
            userReceived.Id.Should().Be(expectedUser.Id);
            _mockUserRepo.Verify(repo => repo.GetUserByID(It.IsAny<Guid>()), Times.Once);
        }
        #endregion

        #region GetUserByEmail
        [Fact]
        public void GetUserByEmail_ValidEmail_ReturnsUser()
        {
            // Arrange
            User expectedUser = UserHelper.GenerateUser();

            _mockUserRepo.Setup(repo => repo.GetUserByEmail(expectedUser.Email)).Returns(expectedUser);

            // Act
            UserModel userReceived = _userService.GetUserByEmail(expectedUser.Email);

            // Assert
            userReceived.Should().NotBeNull();
            userReceived.Should().BeOfType<UserModel>();
            userReceived.Email.Should().Be(expectedUser.Email);
            _mockUserRepo.Verify(repo => repo.GetUserByEmail(It.IsAny<string>()), Times.Once);
        }
        #endregion

        #region CreateUser
        [Fact]
        public void CreateUser_AdminUserRequest_CreatesUser()
        {
            // Arrange
            User newUser = UserHelper.GenerateUser();
            User adminUser = UserHelper.GenerateAdminUser();

            _mockUserRepo.Setup(repo => repo.GetUserByID(adminUser.Id)).Returns(adminUser);
            _mockUserRepo.Setup(repo => repo.CreateUser(It.Is<User>(u => u.Id == newUser.Id))).Returns(newUser);

            // Act
            UserModel userReceived = _userService.CreateUser(adminUser.Id, newUser.ToModel());

            // Assert
            userReceived.Should().NotBeNull();
            userReceived.Should().BeOfType<UserModel>();
            adminUser.IsAdmin.Should().BeTrue();
            userReceived.Id.Should().Be(newUser.Id);
            _mockUserRepo.Verify(repo => repo.CreateUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public void CreateUser_NonAdminUserRequest_ThrowsUnauthorizedActionException()
        {
            // Arrange
            UserModel newUser = UserHelper.GenerateUser().ToModel();
            User nonAdminUser = UserHelper.GenerateUser();

            _mockUserRepo.Setup(repo => repo.GetUserByID(nonAdminUser.Id)).Returns(nonAdminUser);

            // Act
            Action act = () => _userService.CreateUser(nonAdminUser.Id, newUser);

            // Assert
            act.Should().Throw<UnauthorizedActionException>();
            nonAdminUser.IsAdmin.Should().BeFalse();
        }
        #endregion

        #region RegisterUser
        [Fact]
        public void RegisterUser_ValidUser_CreatesAndReturnsUser()
        {
            // Arrange
            UserRegistrationModel newUserRegModel = UserHelper.GenerateUserRegistrationModel();
            User expectedUser = new User()
            {
                Id = newUserRegModel.Id, // Will be overwritten by the service class
                FirstName = newUserRegModel.FirstName,
                LastName = newUserRegModel.LastName,
                Username = newUserRegModel.Username,
                Email = newUserRegModel.Email,
                Birthdate = newUserRegModel.Birthdate,
                IsAdmin = true, // Will be overwritten by the service class
                CreatedAt = DateTime.Now.AddMinutes(5), // Will be overwritten by the service class
                LastModifiedAt = DateTime.Now.AddMinutes(5), // Will be overwritten by the service class
            };

            byte[] passwordHash = Convert.FromBase64String("c29tZVJhbmRvbUJhc2U2NFN0cmluZw==");
            byte[] passwordSalt = Convert.FromBase64String("c29tZU90aGVyUmFuZG9tQmFzZTY0U3RyaW5n");

            _mockPasswordService.Setup(service => service.CreatePasswordHash(newUserRegModel.Password, out It.Ref<byte[]>.IsAny, out It.Ref<byte[]>.IsAny))
                .Callback(new PasswordHashSaltCallback((string password, out byte[] hash, out byte[] salt) =>
                {
                    hash = passwordHash;
                    salt = passwordSalt;
                    expectedUser.PasswordHash = passwordHash;
                    expectedUser.PasswordSalt = passwordSalt;
                }));

            _mockUserRepo.Setup(repo => repo.CreateUser(It.Is<User>(u => u.Username == newUserRegModel.Username))).Returns(expectedUser);

            // Act
            UserModel userReceived = _userService.RegisterUser(newUserRegModel);

            // Assert
            userReceived.Should().NotBeNull();
            userReceived.Should().BeOfType<UserModel>();
            userReceived.Id.Should().NotBe(expectedUser.Id); // User ID will be generated by the service class
            userReceived.IsAdmin.Should().BeFalse(); // IsAdmin is by default always false when delared by the service class
            userReceived.CreatedAt.Should().BeBefore(expectedUser.CreatedAt); // Created at should be declared by the server at the time of creation
            userReceived.LastModifiedAt.Should().BeBefore(expectedUser.LastModifiedAt); // LastModified at should be declared by the server at the time of creation
            _mockUserRepo.Verify(repo => repo.CreateUser(It.IsAny<User>()), Times.Once);
        }


        delegate void PasswordHashSaltCallback(string password, out byte[] hash, out byte[] salt);
        #endregion

        #region UpdateUser
        [Fact]
        public void UpdateUsers_ReceivesValidUser_ReturnsUpdatedUser()
        {
            // Arrange 
            UserModel userToUpdate = UserHelper.GenerateUser().ToModel();
            Guid loggedInUserID = userToUpdate.Id;

            User userModified = userToUpdate.ToEntity();
            userModified.LastModifiedAt = DateTime.Now.AddMinutes(1);
            _mockUserRepo.Setup(repo => repo.UpdateUser(It.Is<User>(u => u.Id == userToUpdate.Id))).Returns(userModified);


            // Act
            UserModel userReceived = _userService.UpdateUser(loggedInUserID, userToUpdate);

            // Assert 
            userReceived.Should().NotBeNull();
            userReceived.Should().BeOfType<UserModel>();
            userReceived.LastModifiedAt.Should().NotBeOnOrBefore(userToUpdate.LastModifiedAt);
            userReceived.Id.Should().Be(loggedInUserID);
            userReceived.Id.Should().Be(userToUpdate.Id);
        }

        [Fact]
        public void UpdateUsers_ReceivesDifferentLoggins_ThrowUnauthorizedError()
        {
            // Arrange 
            UserModel userToUpdate = UserHelper.GenerateUser().ToModel();
            Guid loggedInUserID = Guid.NewGuid();

            // Act
            Action act = () => _userService.UpdateUser(loggedInUserID, userToUpdate);

            // Assert
            act.Should().Throw<UnauthorizedActionException>();
        }
        #endregion

        #region DeleteUser
        [Fact]
        public void DeleteUser_ValidUser_UserIsDeleted()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            _mockUserRepo.Setup(repo => repo.DeleteUser(userId));

            // Act
            Action act = () => _userService.DeleteUser(userId, userId);

            // Assert
            act.Should().NotThrow();
            _mockUserRepo.Verify(repo => repo.DeleteUser(userId), Times.Once);
        }

        [Fact]
        public void DeleteUser_DifferentUserIds_ThrowsUnauthorizedActionException()
        {
            // Arrange
            Guid loggedInUserId = Guid.NewGuid();
            Guid userToDeleteId = Guid.NewGuid();

            // Act
            Action act = () => _userService.DeleteUser(loggedInUserId, userToDeleteId);

            // Assert
            act.Should().Throw<UnauthorizedActionException>();
        }
        #endregion
    }
}
