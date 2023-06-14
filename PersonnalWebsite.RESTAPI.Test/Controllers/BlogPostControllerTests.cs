using Moq;
using Xunit;
using FluentAssertions;
using PersonnalWebsite.RESTAPI.Interfaces;
using PersonnalWebsite.RESTAPI.Model;
using PersonnalWebsite.RESTAPI.Test.TestHelper;
using PersonnalWebsite.RESTAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PersonnalWebsite.RESTAPI.CustomExceptions;

namespace PersonnalWebsite.RESTAPI.Test.Controllers
{
    public class BlogPostControllerTests
    {
        #region init
        private readonly Mock<IBlogPostService> _mockBlogPostService;
        private readonly BlogPostController _blogPostController;

        public BlogPostControllerTests()
        {
            _mockBlogPostService = new Mock<IBlogPostService>();
            _blogPostController = new BlogPostController(_mockBlogPostService.Object);
        }
        #endregion

        #region GetBlogPosts
        [Fact]
        public void GetBlogPosts_ReturnsStatusCode200_WhenBlogPostsAreSuccessfullyFetched()
        {
            // Arrange
            List<BlogPostModel> blogPosts = BlogPostHelper.GenerateBlogPosts().Select(bp => bp.ToModel()).ToList();
            _mockBlogPostService.Setup(service => service.GetBlogPosts()).Returns(blogPosts);
            int expectedCount = 5;

            // Act
            ActionResult<List<BlogPostModel>> actionResult = _blogPostController.GetBlogPosts();
            var okObjectResult = actionResult.Result as OkObjectResult;

            // Assert
            okObjectResult.Should().NotBeNull();
            var returnedBlogPosts = okObjectResult.Value as IEnumerable<BlogPostModel>;
            returnedBlogPosts.Should().NotBeNullOrEmpty();
            returnedBlogPosts.Count().Should().Be(expectedCount);
            returnedBlogPosts.Should().BeEquivalentTo(blogPosts.OrderBy(bp => bp.Author));
        }

        [Fact]
        public void GetBlogPosts_ReturnsStatusCode500_WhenUnexpectedErrorOccurs()
        {
            // Arrange
            _mockBlogPostService.Setup(service => service.GetBlogPosts()).Throws(new Exception());

            // Act
            ActionResult<List<BlogPostModel>> actionResult = _blogPostController.GetBlogPosts();
            var statusCodeResult = actionResult.Result as ObjectResult;

            // Assert
            statusCodeResult.Should().NotBeNull();
            statusCodeResult.Value.Should().Be("An error occured while getting blog posts");
            statusCodeResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
        #endregion

        #region GetBlogpostsByID
        public void GetBlogPostByID_ReturnsBlogPost_WhenBlogPostExists()
        {
            // Arrange
            List<BlogPostModel> blogPosts = BlogPostHelper.GenerateBlogPosts().Select(bp => bp.ToModel()).ToList();
            BlogPostModel blogPostToFind = BlogPostHelper.GenerateBlogPost().ToModel();
            Guid blogPostID = blogPostToFind.BlogPostID;
            blogPosts.Add(blogPostToFind);
            int expectedCount = 1;
            _mockBlogPostService.Setup(service => service.GetBlogPostByID(blogPostID)).Returns(blogPostToFind);

            // Act
            ActionResult<BlogPostModel> actionResult = _blogPostController.GetBlogPostById(blogPostID);
            var okObjectResult = actionResult.Result as OkObjectResult;

            // Assert
            okObjectResult.Should().NotBeNull();
            var returnedBlogPosts = okObjectResult.Value as IEnumerable<BlogPostModel>;
            returnedBlogPosts.Should().NotBeNullOrEmpty();
            returnedBlogPosts.Count().Should().Be(expectedCount);
            returnedBlogPosts.FirstOrDefault().BlogPostID.Should().Be(blogPostID);
        }

        public void GetBlogPostByID_ReturnsNotFound_WhenBlogPostDoesNotExist()
        {
            // Arrange
            List<BlogPostModel> blogPosts = BlogPostHelper.GenerateBlogPosts().Select(bp => bp.ToModel()).ToList();
            BlogPostModel blogPostToFind = BlogPostHelper.GenerateBlogPost().ToModel();
            Guid blogPostID = blogPostToFind.BlogPostID;
            _mockBlogPostService.Setup(service => service.GetBlogPostByID(blogPostID)).Throws(new BlogpostNotFoundException());

            // Act
            ActionResult<BlogPostModel> actionResult = _blogPostController.GetBlogPostById(blogPostID);
            var notFoundObjectResult = actionResult.Result as NotFoundObjectResult;

            // Assert
            notFoundObjectResult.Should().NotBeNull();
            notFoundObjectResult.Value.Should().Be($"Could not find blog post with ID: {blogPostID}");
            notFoundObjectResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public void GetBlogPostByID_Returns500Exception_WhenUnexpectedExceptionIsThrown()
        {
            // Arrange
            Guid blogPostID = Guid.NewGuid();
            _mockBlogPostService.Setup(service => service.GetBlogPostByID(blogPostID)).Throws(new Exception());

            // Act
            ActionResult<BlogPostModel> actionResult = _blogPostController.GetBlogPostById(blogPostID);
            var statusCodeResult = actionResult.Result as ObjectResult;

            // Assert
            statusCodeResult.Should().NotBeNull();
            statusCodeResult.Value.Should().Be($"An error occurred while retrieving the blog post.");
            statusCodeResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
        #endregion

        #region GetBlogPostsByUsername
        [Fact]
        public void GetBlogPostsByUsername_ReturnsBlogPosts_WhenUserExists()
        {
            // Arrange
            string username = "username";
            List<BlogPostModel> blogPosts = BlogPostHelper.GenerateBlogPosts().Select(bp => bp.ToModel()).ToList();
            _mockBlogPostService.Setup(service => service.GetBlogPostsByUsername(username)).Returns(blogPosts);

            // Act
            ActionResult<IEnumerable<BlogPostModel>> actionResult = _blogPostController.GetBlogPostsByUsername(username);
            var okObjectResult = actionResult.Result as OkObjectResult;

            // Assert
            okObjectResult.Should().NotBeNull();
            var returnedBlogPosts = okObjectResult.Value as IEnumerable<BlogPostModel>;
            returnedBlogPosts.Should().NotBeNullOrEmpty();
            returnedBlogPosts.Count().Should().Be(blogPosts.Count);
            returnedBlogPosts.Should().BeEquivalentTo(blogPosts);
        }

        [Fact]
        public void GetBlogPostsByUsername_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            string username = "username";
            _mockBlogPostService.Setup(service => service.GetBlogPostsByUsername(username)).Throws(new UserNotFoundException());

            // Act
            ActionResult<IEnumerable<BlogPostModel>> actionResult = _blogPostController.GetBlogPostsByUsername(username);
            var notFoundObjectResult = actionResult.Result as NotFoundObjectResult;

            // Assert
            notFoundObjectResult.Should().NotBeNull();
            notFoundObjectResult.Value.Should().Be($"Could not find user with username: {username}");
            notFoundObjectResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public void GetBlogPostsByUsername_Returns500Exception_WhenUnexpectedExceptionIsThrown()
        {
            // Arrange
            string username = "username";
            _mockBlogPostService.Setup(service => service.GetBlogPostsByUsername(username)).Throws(new Exception());

            // Act
            ActionResult<IEnumerable<BlogPostModel>> actionResult = _blogPostController.GetBlogPostsByUsername(username);
            var statusCodeResult = actionResult.Result as ObjectResult;

            // Assert
            statusCodeResult.Should().NotBeNull();
            statusCodeResult.Value.Should().Be($"An error occurred while retrieving blog posts.");
            statusCodeResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
        #endregion

        #region CreateBlogpost
        [Fact]
        public void CreateBlogPost_ReturnsCreatedAtActionResult_WhenBlogPostIsSuccessfullyCreated()
        {
            // Arrange
            BlogPostModel blogPostToCreate = BlogPostHelper.GenerateBlogPost().ToModel();
            Guid loggedInUserID = Guid.NewGuid();

            _blogPostController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = UserHelper.GenerateClaimsPrincipal(loggedInUserID) }
            };

            _mockBlogPostService.Setup(service => service.CreateBlogPost(loggedInUserID, blogPostToCreate)).Returns(blogPostToCreate);

            // Act
            ActionResult<BlogPostModel> actionResult = _blogPostController.CreateBlogPost(blogPostToCreate);
            var createdAtActionResult = actionResult.Result as CreatedAtActionResult;

            // Assert
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.Value.Should().BeEquivalentTo(blogPostToCreate);
            createdAtActionResult.ActionName.Should().Be(nameof(_blogPostController.GetBlogPostById));
            createdAtActionResult.RouteValues["id"].Should().Be(blogPostToCreate.BlogPostID);
        }

        [Fact]
        public void CreateBlogPost_ReturnsStatusCode403_WhenLoggedInUserIsNotAuthorizedToCreateBlogPost()
        {
            // Arrange
            BlogPostModel blogPostToCreate = BlogPostHelper.GenerateBlogPost().ToModel();
            Guid loggedInUserID = Guid.NewGuid();

            _blogPostController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = UserHelper.GenerateClaimsPrincipal(loggedInUserID) }
            };

            _mockBlogPostService.Setup(service => service.CreateBlogPost(loggedInUserID, blogPostToCreate)).Throws(new UnauthorizedActionException());

            // Act
            ActionResult<BlogPostModel> actionResult = _blogPostController.CreateBlogPost(blogPostToCreate);
            var forbiddenObjectResult = actionResult.Result as ObjectResult;

            // Assert
            forbiddenObjectResult.Should().NotBeNull();
            forbiddenObjectResult.Value.Should().Be("Current logged in user does not have the authorization to create this blog post");
            forbiddenObjectResult.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        public void CreateBlogPost_ReturnsStatusCode404_WhenUserNotFound()
        {
            // Arrange
            BlogPostModel blogPostToCreate = BlogPostHelper.GenerateBlogPost().ToModel();
            Guid loggedInUserID = Guid.NewGuid();

            _blogPostController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = UserHelper.GenerateClaimsPrincipal(loggedInUserID) }
            };

            _mockBlogPostService.Setup(service => service.CreateBlogPost(loggedInUserID, blogPostToCreate)).Throws(new UserNotFoundException());

            // Act
            ActionResult<BlogPostModel> actionResult = _blogPostController.CreateBlogPost(blogPostToCreate);
            var notFoundObjectResult = actionResult.Result as NotFoundObjectResult;

            // Assert
            notFoundObjectResult.Should().NotBeNull();
            notFoundObjectResult.Value.Should().Be("Could not find author of the blog post");
            notFoundObjectResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public void CreateBlogPost_ReturnsStatusCode500_WhenUnexpectedErrorOccurs()
        {
            // Arrange
            BlogPostModel blogPostToCreate = BlogPostHelper.GenerateBlogPost().ToModel();
            Guid loggedInUserID = Guid.NewGuid();

            _blogPostController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = UserHelper.GenerateClaimsPrincipal(loggedInUserID) }
            };

            _mockBlogPostService.Setup(service => service.CreateBlogPost(loggedInUserID, blogPostToCreate)).Throws(new Exception());

            // Act
            ActionResult<BlogPostModel> actionResult = _blogPostController.CreateBlogPost(blogPostToCreate);
            var statusCodeResult = actionResult.Result as ObjectResult;

            // Assert
            statusCodeResult.Should().NotBeNull();
            statusCodeResult.Value.Should().Be("An error occurred while creating the blog post.");
            statusCodeResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
        #endregion

        #region UpdateBlogPost
        [Fact]
        public void UpdateBlogPost_ReturnsOkResult_WithUpdatedBlogPost()
        {
            // Arrange
            BlogPostModel blogPostToUpdate = BlogPostHelper.GenerateBlogPost().ToModel();
            Guid blogPostId = blogPostToUpdate.BlogPostID;
            Guid loggedInUserID = Guid.NewGuid();

            _blogPostController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = UserHelper.GenerateClaimsPrincipal(loggedInUserID) }
            };

            _mockBlogPostService.Setup(service => service.UpdateBlogPost(loggedInUserID, blogPostToUpdate)).Returns(blogPostToUpdate);

            // Act
            ActionResult<BlogPostModel> actionResult = _blogPostController.UpdateBlogPost(blogPostId, blogPostToUpdate);
            var okResult = actionResult.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            okResult.Should().BeOfType<OkObjectResult>();
            var returnedBlogPost = okResult.Value as BlogPostModel;
            returnedBlogPost.Should().BeEquivalentTo(blogPostToUpdate);
        }

        [Fact]
        public void UpdateBlogPost_ReturnsBadRequest_WhenBlogPostIdMismatch()
        {
            // Arrange
            BlogPostModel blogPostToUpdate = BlogPostHelper.GenerateBlogPost().ToModel();
            Guid blogPostId = Guid.NewGuid();
            Guid loggedInUserID = Guid.NewGuid();

            _blogPostController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = UserHelper.GenerateClaimsPrincipal(loggedInUserID) }
            };

            // Act
            ActionResult<BlogPostModel> actionResult = _blogPostController.UpdateBlogPost(blogPostId, blogPostToUpdate);
            var badRequestObjectResult = actionResult.Result as BadRequestObjectResult;

            // Assert
            badRequestObjectResult.Should().NotBeNull();
            badRequestObjectResult.Value.Should().Be("Mismatched blog post ID.");
            badRequestObjectResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public void UpdateBlogPost_ReturnsNotFoundResult_WhenBlogPostToBeUpdatedDoesNotExist()
        {
            // Arrange
            BlogPostModel blogPostToUpdate = BlogPostHelper.GenerateBlogPost().ToModel();
            Guid blogPostId = blogPostToUpdate.BlogPostID;
            Guid loggedInUserID = Guid.NewGuid();

            _blogPostController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = UserHelper.GenerateClaimsPrincipal(loggedInUserID) }
            };

            _mockBlogPostService.Setup(service => service.UpdateBlogPost(loggedInUserID, blogPostToUpdate)).Throws<BlogpostNotFoundException>();

            // Act
            ActionResult<BlogPostModel> actionResult = _blogPostController.UpdateBlogPost(blogPostId, blogPostToUpdate);
            var notFoundObjectResult = actionResult.Result as NotFoundObjectResult;

            // Assert
            notFoundObjectResult.Should().NotBeNull();
            notFoundObjectResult.Value.Should().Be("The blog post could not be found");
            notFoundObjectResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public void UpdateBlogPost_ReturnsStatusCode403_WhenLoggedInUserIsNotAuthorizedToUpdateBlogPost()
        {
            // Arrange
            BlogPostModel blogPostToUpdate = BlogPostHelper.GenerateBlogPost().ToModel();
            Guid blogPostId = blogPostToUpdate.BlogPostID;
            Guid loggedInUserID = Guid.NewGuid();

            _blogPostController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = UserHelper.GenerateClaimsPrincipal(loggedInUserID) }
            };

            _mockBlogPostService.Setup(service => service.UpdateBlogPost(loggedInUserID, blogPostToUpdate)).Throws(new UnauthorizedActionException());

            // Act
            ActionResult<BlogPostModel> actionResult = _blogPostController.UpdateBlogPost(blogPostId, blogPostToUpdate);
            var forbiddenObjectResult = actionResult.Result as ObjectResult;

            // Assert
            forbiddenObjectResult.Should().NotBeNull();
            forbiddenObjectResult.Value.Should().Be("Current logged in user does not have the authorization to update this blog post");
            forbiddenObjectResult.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        public void UpdateBlogPost_ReturnsStatusCode500_WhenUnexpectedErrorOccurs()
        {
            // Arrange
            BlogPostModel blogPostToUpdate = BlogPostHelper.GenerateBlogPost().ToModel();
            Guid blogPostId = blogPostToUpdate.BlogPostID;
            Guid loggedInUserID = Guid.NewGuid();

            _blogPostController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = UserHelper.GenerateClaimsPrincipal(loggedInUserID) }
            };

            _mockBlogPostService.Setup(service => service.UpdateBlogPost(loggedInUserID, blogPostToUpdate)).Throws(new Exception());

            // Act
            ActionResult<BlogPostModel> actionResult = _blogPostController.UpdateBlogPost(blogPostId, blogPostToUpdate);
            var statusCodeResult = actionResult.Result as ObjectResult;

            // Assert
            statusCodeResult.Should().NotBeNull();
            statusCodeResult.Value.Should().Be("An error occurred while updating the blog post.");
            statusCodeResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
        #endregion

        #region DeleteBlogpost
        [Fact]
        public void DeleteBlogPost_ReturnsNoContentResult_WhenBlogPostIsSuccessfullyDeleted()
        {
            // Arrange
            Guid blogPostId = Guid.NewGuid();
            Guid loggedInUserID = Guid.NewGuid();

            _blogPostController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = UserHelper.GenerateClaimsPrincipal(loggedInUserID) }
            };

            _mockBlogPostService.Setup(service => service.DeleteBlogPost(loggedInUserID, blogPostId)).Verifiable();

            // Act
            IActionResult actionResult = _blogPostController.DeleteBlogPost(blogPostId);

            // Assert
            _mockBlogPostService.Verify();
            Assert.IsType<NoContentResult>(actionResult);
        }

        [Fact]
        public void DeleteBlogPost_ReturnsNotFoundResult_WhenBlogPostToBeDeletedDoesNotExist()
        {
            // Arrange
            Guid blogPostId = Guid.NewGuid();
            Guid loggedInUserID = Guid.NewGuid();

            _blogPostController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = UserHelper.GenerateClaimsPrincipal(loggedInUserID) }
            };

            _mockBlogPostService.Setup(service => service.DeleteBlogPost(loggedInUserID, blogPostId)).Throws<BlogpostNotFoundException>();

            // Act
            IActionResult actionResult = _blogPostController.DeleteBlogPost(blogPostId);
            var notFoundObjectResult = actionResult as NotFoundObjectResult;

            // Assert
            notFoundObjectResult.Should().NotBeNull();
            notFoundObjectResult.Value.Should().Be($"Could not find the blog post with id: {blogPostId}");
            notFoundObjectResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public void DeleteBlogPost_ReturnsStatusCode403_WhenLoggedInUserIsNotAuthorizedToDeleteBlogPost()
        {
            // Arrange
            Guid blogPostId = Guid.NewGuid();
            Guid loggedInUserID = Guid.NewGuid();

            _blogPostController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = UserHelper.GenerateClaimsPrincipal(loggedInUserID) }
            };

            _mockBlogPostService.Setup(service => service.DeleteBlogPost(loggedInUserID, blogPostId)).Throws(new UnauthorizedActionException());

            // Act
            IActionResult actionResult = _blogPostController.DeleteBlogPost(blogPostId);
            var forbiddenObjectResult = actionResult as ObjectResult;

            // Assert
            forbiddenObjectResult.Should().NotBeNull();
            forbiddenObjectResult.Value.Should().Be("Current logged in user does not have the authorization to delete this blog post");
            forbiddenObjectResult.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        public void DeleteBlogPost_ReturnsStatusCode500_WhenUnexpectedErrorOccurs()
        {
            // Arrange
            Guid blogPostId = Guid.NewGuid();
            Guid loggedInUserID = Guid.NewGuid();

            _blogPostController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = UserHelper.GenerateClaimsPrincipal(loggedInUserID) }
            };

            _mockBlogPostService.Setup(service => service.DeleteBlogPost(loggedInUserID, blogPostId)).Throws(new Exception());

            // Act
            IActionResult actionResult = _blogPostController.DeleteBlogPost(blogPostId);
            var statusCodeResult = actionResult as ObjectResult;

            // Assert
            statusCodeResult.Should().NotBeNull();
            statusCodeResult.Value.Should().Be("An error occurred while deleting the blog post.");
            statusCodeResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
        #endregion
    }
}
