using Microsoft.AspNetCore.Mvc;
using PersonnalWebsite.RESTAPI.Interfaces;
using PersonnalWebsite.RESTAPI.Model;

namespace PersonnalWebsite.RESTAPI.Controllers
{
    [Route("api/blogposts")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
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
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public ActionResult<BlogPostModel> CreateBlogPost([FromBody] BlogPostModel blogPost)
        {
            try
            {
                var createdBlogPost = _blogPostService.CreateBlogPost(blogPost);
                return CreatedAtAction(nameof(GetBlogPostById), new { id = createdBlogPost.BlogPostID }, createdBlogPost);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the blog post.");
            }
        }

        // PUT api/blogposts/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<BlogPostModel> UpdateBlogPost(Guid id, [FromBody] BlogPostModel blogPost)
        {
            try
            {
                if (id != blogPost.BlogPostID)
                {
                    return BadRequest("Mismatched blog post ID.");
                }

                var updatedBlogPost = _blogPostService.UpdateBlogPost(blogPost);

                if (updatedBlogPost == null)
                {
                    return NotFound();
                }

                return Ok(updatedBlogPost);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the blog post.");
            }
        }

        // DELETE api/blogposts/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteBlogPost(Guid id)
        {
            try
            {
                _blogPostService.DeleteBlogPost(id);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the blog post.");
            }
        }
    }
}