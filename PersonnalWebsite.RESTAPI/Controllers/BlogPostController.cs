using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonnalWebsite.RESTAPI.CustomExceptions;
using PersonnalWebsite.RESTAPI.Interfaces;
using PersonnalWebsite.RESTAPI.Model;
using System.Security.Claims;

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
        [ProducesResponseType(404)]
        public ActionResult<BlogPostModel> GetBLogPosts()
        {
            try
            {
                IEnumerable<BlogPostModel> blogPosts = _blogPostService.GetBlogPosts();

                if (blogPosts == null)
                {
                    return NotFound();
                }

                return Ok(blogPosts);
            }
            catch (Exception ex)
            {
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
                var blogPost = _blogPostService.GetBlogPostByID(id);

                if (blogPost == null)
                {
                    return NotFound();
                }

                return Ok(blogPost);
            }
            catch (Exception)
            {
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
                var blogPosts = _blogPostService.GetBlogPostsByUsername(username);

                if (blogPosts == null || !blogPosts.Any())
                {
                    return NotFound();
                }

                return Ok(blogPosts);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving blog posts.");
            }
        }

        // POST api/blogposts
        [HttpPost, Authorize]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public ActionResult<BlogPostModel> CreateBlogPost([FromBody] BlogPostModel blogPost)
        {
            Guid loggedInUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            try
            {
                var createdBlogPost = _blogPostService.CreateBlogPost(loggedInUserId, blogPost);
                return CreatedAtAction(nameof(GetBlogPostById), new { id = createdBlogPost.BlogPostID }, createdBlogPost);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the blog post.");
            }
        }

        // PUT api/blogposts/{id}
        [HttpPut("{id}"), Authorize]
        [ProducesResponseType(200)]
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

                if (updatedBlogPost == null)
                {
                    Log.Warn("UpdateBlogPost: The blog post to update could not be found");
                    return NotFound();
                }

                return Ok(updatedBlogPost);
            }
            catch (UnauthorizedActionException ex)
            {
                Log.Error($"UpdateBlogPost: {ex}");
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
        [ProducesResponseType(404)]
        public IActionResult DeleteBlogPost(Guid id)
        {
            Guid loggedInUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            try
            {
                _blogPostService.DeleteBlogPost(loggedInUserId, id);
                return NoContent();
            }
            catch (UnauthorizedActionException ex)
            {
                Log.Error($"DeleteBlogPost: {ex}");
                return StatusCode(StatusCodes.Status403Forbidden, "Current logged in user does not have the authorization to delete this blog post");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the blog post.");
            }
        }
    }
}