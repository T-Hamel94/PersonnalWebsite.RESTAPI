using FluentAssertions;
using Moq;
using Xunit;
using PersonnalWebsite.RESTAPI.Entities;
using PersonnalWebsite.RESTAPI.Interfaces;
using PersonnalWebsite.RESTAPI.Service;
using PersonnalWebsite.RESTAPI.Model;
using PersonnalWebsite.RESTAPI.Test.TestHelper;

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
            result.Should().HaveCount(3);
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




        #region SetUp
        #endregion
    }
}
