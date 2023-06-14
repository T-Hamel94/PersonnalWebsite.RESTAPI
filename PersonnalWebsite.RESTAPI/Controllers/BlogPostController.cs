using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using log4net;
using PersonnalWebsite.RESTAPI.CustomExceptions;
using PersonnalWebsite.RESTAPI.Interfaces;
using PersonnalWebsite.RESTAPI.Model;

namespace PersonnalWebsite.RESTAPI.Controllers
{
    [Route("api/blogposts")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(UserController));
        private IBlogPostService _blogPostService;

        public BlogPostController(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        // GET api/blogposts
        [HttpGet]
        [ProducesResponseType(200)]
        public ActionResult<List<BlogPostModel>> GetBlogPosts()
        {
            try
            {
                IEnumerable<BlogPostModel> blogPosts = _blogPostService.GetBlogPosts();

                return Ok(blogPosts.OrderBy(bp => bp.Author));
            }
            catch (Exception ex)
            {
                Log.Error($"GetBlogPosts: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occured while getting blog posts");
            }
        }

        // GET api/blogposts
        [HttpGet("unapproved"), Authorize]
        [ProducesResponseType(200)]
        public ActionResult<List<BlogPostModel>> GetUnapprovedBlogPosts()
        {
            try
            {
                IEnumerable<BlogPostModel> blogPosts = _blogPostService.GetUnapprovedBlogPosts();

                return Ok(blogPosts.OrderBy(bp => bp.Author));
            }
            catch (Exception ex)
            {
                Log.Error($"GetBlogPosts: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occured while getting blog posts");
            }
        }

        // GET api/blogposts/{id}
        [HttpGet("id/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<BlogPostModel> GetBlogPostById(Guid id)
        {
            try
            {
                BlogPostModel blogPost = _blogPostService.GetBlogPostByID(id);

                return Ok(blogPost);
            }
            catch (BlogpostNotFoundException ex)
            {
                Log.Info($"GetBlogPostById: {ex}");
                return NotFound($"Could not find blog post with ID: {id}");
            }
            catch (Exception ex)
            {
                Log.Error($"GetBlogPostById: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the blog post.");
            }
        }

        // GET api/blogposts/{username}
        [HttpGet("username/{username}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<BlogPostModel>> GetBlogPostsByUsername(string username)
        {
            try
            {
                IEnumerable<BlogPostModel> blogPosts = _blogPostService.GetBlogPostsByUsername(username);

                return Ok(blogPosts);
            }
            catch (UserNotFoundException ex)
            {
                Log.Info($"GetBlogPostByUsername: {ex}");
                return NotFound($"Could not find user with username: {username}");
            }
            catch (Exception ex)
            {
                Log.Error($"GetBlogPostByUsername: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving blog posts.");
            }
        }

        // POST api/blogposts
        [HttpPost, Authorize]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public ActionResult<BlogPostModel> CreateBlogPost([FromBody] BlogPostModel blogPost)
        {
            Guid loggedInUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            try
            {
                var createdBlogPost = _blogPostService.CreateBlogPost(loggedInUserId, blogPost);
                return CreatedAtAction(nameof(GetBlogPostById), new { id = createdBlogPost.BlogPostID }, createdBlogPost);
            }
            catch (UnauthorizedActionException ex)
            {
                Log.Error($"CreateBlogPost: {ex}");
                return StatusCode(StatusCodes.Status403Forbidden, "Current logged in user does not have the authorization to create this blog post");
            }
            catch (UserNotFoundException ex)
            {
                Log.Info($"CreateBlogPost: {ex}");
                return NotFound("Could not find author of the blog post");
            }
            catch (Exception ex)
            {
                Log.Error($"CreateBlogPost: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the blog post.");
            }
        }

        // PUT api/blogposts/{id}
        [HttpPut("{id}"), Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public ActionResult<BlogPostModel> UpdateBlogPost(Guid id, [FromBody] BlogPostModel blogPost)
        {
            Guid loggedInUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            try
            {
                if (id != blogPost.BlogPostID)
                {
                    Log.Warn("UpdateBlogPost: The id of the blog post passed as a parameter does not match the id of the blog post to update");
                    return BadRequest("Mismatched blog post ID.");
                }

                BlogPostModel updatedBlogPost = _blogPostService.UpdateBlogPost(loggedInUserId, blogPost);

                return Ok(updatedBlogPost);
            }
            catch (BlogpostNotFoundException ex)
            {
                Log.Info("UpdateBlogPost: The blog post to update could not be found");
                return NotFound("The blog post could not be found");
            }
            catch (UnauthorizedActionException ex)
            {
                Log.Warn($"UpdateBlogPost: {ex}");
                return StatusCode(StatusCodes.Status403Forbidden, "Current logged in user does not have the authorization to update this blog post");
            }
            catch (Exception ex)
            {
                Log.Error($"UpdateBlogPost: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the blog post.");
            }
        }

        // PUT api/blogposts/{id}
        [HttpPut("approve/{id}"), Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public ActionResult<BlogPostModel> ApproveBlogPost(Guid id)
        {
            Guid loggedInUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            try
            {
                BlogPostModel updatedBlogPost = _blogPostService.ApproveBlogPost(loggedInUserId, id);

                return Ok(updatedBlogPost);
            }
            catch (BlogpostNotFoundException ex)
            {
                Log.Info("UpdateBlogPost: The blog post to update could not be found");
                return NotFound("The blog post could not be found");
            }
            catch (UnauthorizedActionException ex)
            {
                Log.Warn($"UpdateBlogPost: {ex}");
                return StatusCode(StatusCodes.Status403Forbidden, "Current logged in user does not have the authorization to update this blog post");
            }
            catch (Exception ex)
            {
                Log.Error($"UpdateBlogPost: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the blog post.");
            }
        }

        // DELETE api/blogposts/{id}
        [HttpDelete("{id}"), Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public IActionResult DeleteBlogPost(Guid id)
        {
            Guid loggedInUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            try
            {
                _blogPostService.DeleteBlogPost(loggedInUserId, id);
                return NoContent();
            }
            catch (BlogpostNotFoundException ex)
            {
                Log.Info($"DeleteBlogPost: {ex}");
                return NotFound($"Could not find the blog post with id: {id}");
            }
            catch (UnauthorizedActionException ex)
            {
                Log.Warn($"DeleteBlogPost: {ex}");
                return StatusCode(StatusCodes.Status403Forbidden, "Current logged in user does not have the authorization to delete this blog post");
            }
            catch (Exception ex)
            {
                Log.Error($"DeleteBlogPost: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the blog post.");
            }
        }
    }
}