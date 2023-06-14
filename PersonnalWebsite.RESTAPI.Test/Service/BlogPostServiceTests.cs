using FluentAssertions;
using Moq;
using Xunit;
using PersonnalWebsite.RESTAPI.Entities;
using PersonnalWebsite.RESTAPI.Interfaces;
using PersonnalWebsite.RESTAPI.Service;
using PersonnalWebsite.RESTAPI.Model;
using PersonnalWebsite.RESTAPI.Test.TestHelper;
using PersonnalWebsite.RESTAPI.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonnalWebsite.RESTAPI.Test.Service
{
    public class BlogPostServiceTests
    {
        #region init
        private readonly Mock<IBlogPostRepo> _mockBlogPostRepo;
        private readonly Mock<IUserRepo> _mockUserRepo;
        private readonly BlogPostService _blogPostService;
        private readonly List<BlogPost> _blogPosts;

        public BlogPostServiceTests()
        {
            _mockBlogPostRepo = new Mock<IBlogPostRepo>();
            _mockUserRepo = new Mock<IUserRepo>();
            _blogPostService = new BlogPostService(_mockBlogPostRepo.Object, _mockUserRepo.Object);
            _blogPosts = BlogPostHelper.GenerateBlogPosts();
        }
        #endregion init

        #region GetBlogPosts
        [Fact]
        public void GetApprovedBlogPosts_ReturnsBlogPostModelList()
        {
            // Arrange
            _mockBlogPostRepo.Setup(repo => repo.GetBlogPosts()).Returns(_blogPosts);

            // Act
            IEnumerable<BlogPostModel> result = _blogPostService.GetBlogPosts();

            // Assert
            result.Should().BeAssignableTo<IEnumerable<BlogPostModel>>();
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
        }

        [Fact]
        public void GetApprovedBlogPosts_ReceivedEmptyBlogPostList_ReturnsEmptyList()
        {
            // Arrange
            _mockBlogPostRepo.Setup(repo => repo.GetBlogPosts()).Returns(new List<BlogPost>());

            // Act
            IEnumerable<BlogPostModel> result = _blogPostService.GetBlogPosts();

            // Assert
            result.Should().BeAssignableTo<IEnumerable<BlogPostModel>>();
            result.Should().NotBeNull();
            result.Should().HaveCount(0);
        }

        [Fact]
        public void GetUnapprovedBlogPosts_ReturnsUnnaprovedBlogPostModelList()
        {
            // Arrange
            _mockBlogPostRepo.Setup(repo => repo.GetBlogPosts()).Returns(_blogPosts);

            // Act
            IEnumerable<BlogPostModel> result = _blogPostService.GetUnapprovedBlogPosts();

            // Assert
            result.Should().BeAssignableTo<IEnumerable<BlogPostModel>>();
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public void GetUnapprovedBlogPosts_ReceivedEmptyBlogPostList_ReturnsEmptyList()
        {
            // Arrange
            _mockBlogPostRepo.Setup(repo => repo.GetBlogPosts()).Returns(new List<BlogPost>());

            // Act
            IEnumerable<BlogPostModel> result = _blogPostService.GetUnapprovedBlogPosts();

            // Assert
            result.Should().BeAssignableTo<IEnumerable<BlogPostModel>>();
            result.Should().NotBeNull();
            result.Should().HaveCount(0);
        }
        #endregion

        #region GetBlogPostByID
        [Fact]
        public void GetBlogPostByID_ReturnsBlogPostModel_WhenBlogPostExists()
        {
            // Arrange
            BlogPost blogPost = BlogPostHelper.GenerateBlogPost();
            Guid blogPostID = blogPost.BlogPostID;
            _mockBlogPostRepo.Setup(repo => repo.GetBlogPostByID(blogPostID)).Returns(blogPost);

            // Act
            BlogPostModel result = _blogPostService.GetBlogPostByID(blogPostID);

            // Assert
            result.Should().BeAssignableTo<BlogPostModel>();
            result.Should().NotBeNull();
            result.BlogPostID.Should().Be(blogPostID);
        }
        #endregion

        #region GetBlogPostsByUsername
        [Fact]
        public void GetBlogPostsByUsername_ReturnsBlogPostModelList_WhenUserExists()
        {
            // Arrange
            string username = "testUser";
            List<BlogPost> blogPosts = BlogPostHelper.GenerateBlogPosts();
            User user = new User { Username = username };

            _mockUserRepo.Setup(repo => repo.GetUserByUsername(username)).Returns(user);
            _mockBlogPostRepo.Setup(repo => repo.GetBlogPostsByUsername(username)).Returns(blogPosts);

            // Act
            IEnumerable<BlogPostModel> result = _blogPostService.GetBlogPostsByUsername(username);

            // Assert
            result.Should().BeAssignableTo<IEnumerable<BlogPostModel>>();
            result.Should().NotBeNull();
            result.Should().HaveCount(5);
        }

        [Fact]
        public void GetBlogPostsByUsername_ThrowsArgumentNullException_WhenUsernameIsNull()
        {
            // Arrange
            string username = null;

            // Act
            Action act = () => _blogPostService.GetBlogPostsByUsername(username);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        #region CreateBlogPost
        [Fact]
        public void CreateBlogPost_ReturnsBlogPostModel_WhenAuthorIsLoggedIn()
        {
            // Arrange
            User authorFound = UserHelper.GenerateUser();
            Guid loggedInUserId = authorFound.Id;

            BlogPostModel blogPostToCreate = BlogPostHelper.GenerateBlogPost().ToModel();
            blogPostToCreate.AuthorID = loggedInUserId;
            authorFound.Id = loggedInUserId;
            BlogPost createdBlogPost = blogPostToCreate.ToEntity();

            _mockUserRepo.Setup(repo => repo.GetUserByID(loggedInUserId)).Returns(authorFound);
            _mockBlogPostRepo.Setup(repo => repo.CreateBlogPost(It.IsAny<BlogPost>())).Returns(createdBlogPost);

            // Act
            BlogPostModel result = _blogPostService.CreateBlogPost(loggedInUserId, blogPostToCreate);

            // Assert
            result.Should().BeAssignableTo<BlogPostModel>();
            result.Should().NotBeNull();
            result.BlogPostID.Should().Be(createdBlogPost.BlogPostID);
            result.Author.Should().Be(authorFound.Username);
            result.AuthorID.Should().Be(authorFound.Id);
        }

        [Fact]
        public void CreateBlogPost_ReturnsBlogPostModelAsNotApproved_WhenAuthorIsNotAdmin()
        {
            // Arrange
            User authorFound = UserHelper.GenerateUser();
            Guid loggedInUserId = authorFound.Id;

            BlogPostModel blogPostToCreate = BlogPostHelper.GenerateBlogPost().ToModel();
            blogPostToCreate.AuthorID = loggedInUserId;
            authorFound.Id = loggedInUserId;
            BlogPost createdBlogPost = blogPostToCreate.ToEntity();

            _mockUserRepo.Setup(repo => repo.GetUserByID(loggedInUserId)).Returns(authorFound);
            _mockBlogPostRepo.Setup(repo => repo.CreateBlogPost(It.IsAny<BlogPost>())).Returns((BlogPost bp) => bp);

            // Act
            BlogPostModel result = _blogPostService.CreateBlogPost(loggedInUserId, blogPostToCreate);

            // Assert
            result.Should().BeAssignableTo<BlogPostModel>();
            result.Should().NotBeNull();
            result.IsApproved.Should().BeFalse();
        }

        [Fact]
        public void CreateBlogPost_ReturnsBlogPostModelAsApproved_WhenAuthorIsAdmin()
        {
            // Arrange
            User authorFound = UserHelper.GenerateAdminUser();
            Guid loggedInUserId = authorFound.Id;

            BlogPostModel blogPostToCreate = BlogPostHelper.GenerateBlogPost().ToModel();
            blogPostToCreate.AuthorID = loggedInUserId;
            authorFound.Id = loggedInUserId;

            _mockUserRepo.Setup(repo => repo.GetUserByID(loggedInUserId)).Returns(authorFound);
            _mockBlogPostRepo.Setup(repo => repo.CreateBlogPost(It.IsAny<BlogPost>())).Returns((BlogPost bp) => bp); 

            // Act
            BlogPostModel result = _blogPostService.CreateBlogPost(loggedInUserId, blogPostToCreate);

            // Assert
            result.Should().BeAssignableTo<BlogPostModel>();
            result.Should().NotBeNull();
            result.IsApproved.Should().BeTrue();
            _mockBlogPostRepo.Verify(repo => repo.CreateBlogPost(It.Is<BlogPost>(bp => bp.IsApproved == true)), Times.Once); 
        }


        [Fact]
        public void CreateBlogPost_ThrowsArgumentNullException_WhenBlogPostIsNull()
        {
            // Arrange
            Guid loggedInUserId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _blogPostService.CreateBlogPost(loggedInUserId, null));
        }

        [Fact]
        public void CreateBlogPost_ThrowsUnauthorizedActionException_WhenAuthorIDDoesNotMatchLoggedInUser()
        {
            // Arrange
            Guid loggedInUserId = Guid.NewGuid();
            BlogPostModel blogPostToCreate = BlogPostHelper.GenerateBlogPost().ToModel();

            // Act & Assert
            Assert.Throws<UnauthorizedActionException>(() => _blogPostService.CreateBlogPost(loggedInUserId, blogPostToCreate));
        }

        [Fact]
        public void CreateBlogPost_ThrowsUserNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            Guid loggedInUserId = Guid.NewGuid();
            BlogPostModel blogPostToCreate = BlogPostHelper.GenerateBlogPost().ToModel();
            blogPostToCreate.AuthorID = loggedInUserId;

            _mockUserRepo.Setup(repo => repo.GetUserByID(loggedInUserId)).Returns<User>(null);

            // Act & Assert
            Assert.Throws<UserNotFoundException>(() => _blogPostService.CreateBlogPost(loggedInUserId, blogPostToCreate));
        }

        [Fact]
        public void CreateBlogPost_ThrowsException_WhenBlogPostWasNotCreated()
        {
            // Arrange
            Guid loggedInUserId = Guid.NewGuid();
            BlogPostModel blogPostToCreate = BlogPostHelper.GenerateBlogPost().ToModel();
            blogPostToCreate.AuthorID = loggedInUserId;
            User authorFound = UserHelper.GenerateUser();
            authorFound.Id = loggedInUserId;

            _mockUserRepo.Setup(repo => repo.GetUserByID(loggedInUserId)).Returns(authorFound);
            _mockBlogPostRepo.Setup(repo => repo.CreateBlogPost(It.IsAny<BlogPost>())).Returns<BlogPost>(null);

            // Act & Assert
            Assert.Throws<Exception>(() => _blogPostService.CreateBlogPost(loggedInUserId, blogPostToCreate));
        }
        #endregion

        #region UpdateBlogPost
        [Fact]
        public void UpdateBlogPost_ReturnsBlogPostModel_WhenSuccessful()
        {
            // Arrange
            Guid loggedInUserId = Guid.NewGuid();
            BlogPostModel blogPostToUpdate = BlogPostHelper.GenerateBlogPost().ToModel();
            blogPostToUpdate.AuthorID = loggedInUserId;
            User authorFound = UserHelper.GenerateUser();
            authorFound.Id = loggedInUserId;
            BlogPost updatedBlogPost = blogPostToUpdate.ToEntity();

            _mockUserRepo.Setup(repo => repo.GetUserByUsername(blogPostToUpdate.Author)).Returns(authorFound);
            _mockBlogPostRepo.Setup(repo => repo.UpdateBlogPost(It.IsAny<BlogPost>())).Returns(updatedBlogPost);

            // Act
            BlogPostModel result = _blogPostService.UpdateBlogPost(loggedInUserId, blogPostToUpdate);

            // Assert
            result.Should().BeAssignableTo<BlogPostModel>();
            result.Should().NotBeNull();
            result.BlogPostID.Should().Be(updatedBlogPost.BlogPostID);
            result.Author.Should().Be(authorFound.Username);
            result.AuthorID.Should().Be(authorFound.Id);
        }

        [Fact]
        public void UpdateBlogPost_ThrowsArgumentNullException_WhenBlogPostIsNull()
        {
            // Arrange
            Guid loggedInUserId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _blogPostService.UpdateBlogPost(loggedInUserId, null));
        }

        [Fact]
        public void UpdateBlogPost_ThrowsUnauthorizedActionException_WhenAuthorIDDoesNotMatchLoggedInUser()
        {
            // Arrange
            Guid loggedInUserId = Guid.NewGuid();
            BlogPostModel blogPostToUpdate = BlogPostHelper.GenerateBlogPost().ToModel();
            blogPostToUpdate.Author = "SomeOtherUser";

            User authorFound = UserHelper.GenerateUser();
            authorFound.Id = Guid.NewGuid();
            _mockUserRepo.Setup(repo => repo.GetUserByUsername(blogPostToUpdate.Author)).Returns(authorFound);

            // Act & Assert
            Assert.Throws<UnauthorizedActionException>(() => _blogPostService.UpdateBlogPost(loggedInUserId, blogPostToUpdate));
        }
        #endregion

        #region DeleteBlogPost
        [Fact]
        public void DeleteBlogPost_ExecutesWithoutException_WhenSuccessful()
        {
            // Arrange
            Guid loggedInUserId = Guid.NewGuid();
            Guid blogPostId = Guid.NewGuid();
            BlogPost blogPostToDelete = BlogPostHelper.GenerateBlogPost();
            blogPostToDelete.AuthorID = loggedInUserId;

            _mockBlogPostRepo.Setup(repo => repo.GetBlogPostByID(blogPostId)).Returns(blogPostToDelete);

            // Act
            Action act = () => _blogPostService.DeleteBlogPost(loggedInUserId, blogPostId);

            // Assert
            act.Should().NotThrow();
            _mockBlogPostRepo.Verify(repo => repo.DeleteBlogPost(blogPostId), Times.Once);
        }

        [Fact]
        public void DeleteBlogPost_ThrowsUnauthorizedActionException_WhenAuthorIDDoesNotMatchLoggedInUser()
        {
            // Arrange
            Guid loggedInUserId = Guid.NewGuid();
            Guid blogPostId = Guid.NewGuid();
            BlogPost blogPostToDelete = BlogPostHelper.GenerateBlogPost();
            blogPostToDelete.AuthorID = Guid.NewGuid(); // Different from loggedInUserId

            _mockBlogPostRepo.Setup(repo => repo.GetBlogPostByID(blogPostId)).Returns(blogPostToDelete);

            // Act
            Action act = () => _blogPostService.DeleteBlogPost(loggedInUserId, blogPostId);

            // Assert
            act.Should().Throw<UnauthorizedActionException>();
        }
        #endregion
    }
}
